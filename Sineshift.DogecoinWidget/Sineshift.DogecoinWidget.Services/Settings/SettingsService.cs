using Newtonsoft.Json;
using Sineshift.DogecoinWidget.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Sineshift.DogecoinWidget.Services
{
	public class SettingsService
	{
		readonly string settingsPath;
		readonly DispatcherTimer saveTimer;
		WidgetSettings currentSettings;

		public SettingsService()
		{
			settingsPath = EnvironmentUtil.GetAppDataPath("Settings.json");
			saveTimer = new DispatcherTimer();
			saveTimer.Interval = TimeSpan.FromMilliseconds(250);
			saveTimer.Tick += OnSaveSettings;

			LoadSettings();
		}

		public WidgetSettings CurrentSettings
		{
			get { return currentSettings; }
		}

		private void OnSettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			saveTimer.Stop();
			saveTimer.Start();
		}

		private void OnSaveSettings(object sender, EventArgs e)
		{
			saveTimer.Stop();
			SaveSettings();
		}

		private void LoadSettings()
		{
			try
			{
				if (File.Exists(settingsPath))
				{
					var settingsString = File.ReadAllText(settingsPath);
					currentSettings = JsonConvert.DeserializeObject<WidgetSettings>(settingsString);
					currentSettings.IsFirstStart = false;
				}
				else
				{
					currentSettings = new WidgetSettings();
					currentSettings.IsFirstStart = true;
					SaveSettings();
				}
			}
			catch (Exception ex)
			{
				currentSettings = new WidgetSettings();
				ExceptionUtil.LogAndShowWarning("Could not load settings. Default settings will be used.", ex);
			}

			currentSettings.PropertyChanged += OnSettingsChanged;
		}

		private void SaveSettings()
		{
			try
			{
				var settingsString = JsonConvert.SerializeObject(currentSettings);
				File.WriteAllText(settingsPath, settingsString);
				UpdateAutoStartState();
				Logger.Current.Debug("Settings saved");
			}
			catch (Exception ex)
			{
				ExceptionUtil.LogAndShowWarning("Could not save settings.", ex);
			}
		}

		private void UpdateAutoStartState()
		{
			try
			{
				if (currentSettings.AutoStart && !RegistryUtil.IsStartupApp())
				{
					RegistryUtil.SetStartupApp(true);
				}
				else if (!currentSettings.AutoStart && RegistryUtil.IsStartupApp())
				{
					RegistryUtil.SetStartupApp(false);
				}
			}
			catch(Exception ex)
			{
				ExceptionUtil.LogAndShowWarning("Could not change autostart state.", ex);
			}
		}
	}
}

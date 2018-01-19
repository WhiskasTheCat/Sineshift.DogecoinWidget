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
		readonly WidgetSettings currentSettings;
		readonly string settingsPath;
		readonly DispatcherTimer saveTimer;

		public SettingsService()
		{
			settingsPath = EnvironmentUtil.GetAppDataPath("Settings.json");
			saveTimer = new DispatcherTimer();
			saveTimer.Interval = TimeSpan.FromMilliseconds(500);
			saveTimer.Tick += OnSaveSettings;

			try
			{
				currentSettings = File.Exists(settingsPath) ? JsonConvert.DeserializeObject<WidgetSettings>(File.ReadAllText(settingsPath)) : new WidgetSettings();
			}
			catch(Exception ex)
			{
				currentSettings = new WidgetSettings();
				Logger.Current.Error("Could not load settings. Default settings will be used.", ex);
			}

			currentSettings.PropertyChanged += OnSettingsChanged;
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

			try
			{
				var settingsString = JsonConvert.SerializeObject(currentSettings);
				File.WriteAllText(settingsPath, settingsString);
				Logger.Current.Debug("Settings saved");
			}
			catch (Exception ex)
			{
				Logger.Current.Error("Could not save settings.", ex);
			}
		}
	}
}

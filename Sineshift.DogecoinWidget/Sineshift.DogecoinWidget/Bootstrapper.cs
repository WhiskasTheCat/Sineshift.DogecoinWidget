using Sineshift.DogecoinWidget.Common;
using Sineshift.DogecoinWidget.Common.UI;
using Sineshift.DogecoinWidget.Services;
using Sineshift.DogecoinWidget.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Sineshift.DogecoinWidget
{
	public class Bootstrapper
	{
		SettingsService settingsService;

		public MainWindow Run()
		{
			try
			{
				// US culture for price formatting
				var usCulture = new CultureInfo("en-US");
				Thread.CurrentThread.CurrentCulture = usCulture;
				Thread.CurrentThread.CurrentUICulture = usCulture;

				Logger.Current.Info("Running bootstrapper...");

				ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(30000));
				Application.Current.DispatcherUnhandledException += OnDispatcherException;
				AppDomain.CurrentDomain.UnhandledException += OnException;

				ServiceLocator.Current
					.SetSingleton<ILogger>(Logger.Current)
					.SetSingleton<SettingsService>()
					.SetSingleton<CoinMarketService>()
					.SetSingleton<Navigator>();

				settingsService = ServiceLocator.Current.Get<SettingsService>();

				// Update the current app path if we are set as startup app
				Logger.Current.Info("Checking autostart status...");
				ExceptionUtil.IgnoreException(() =>
				{
					if (settingsService.CurrentSettings.AutoStart)
					{
						RegistryUtil.SetStartupApp(true);
					}
				});
			
				var mainWindow = ServiceLocator.Current.Get<MainWindow>();
				mainWindow.Top = settingsService.CurrentSettings.Top;
				mainWindow.Left = settingsService.CurrentSettings.Left;
				mainWindow.LocationChanged += OnWindowLocationChanged;
				mainWindow.SourceInitialized += OnWindowSourceInitialized;
				mainWindow.Closing += OnWindowClosing;
				mainWindow.StateChanged += OnWindowStateChanged;

				Logger.Current.Info("Bootstrapper done.");

				return mainWindow;
			}
			catch(Exception ex)
			{
				ExceptionUtil.LogAndShowError("There was an error initializing the app, it will be shut down.", ex);
				Application.Current.Shutdown();
			}

			return null;
		}

		public bool EnsureLicense()
		{
			if (!settingsService.CurrentSettings.AcceptedLicense)
			{
				Logger.Current.Info("Showing license");
				var setupWindow = ServiceLocator.Current.Get<SetupWindow>();
				//setupWindow.Owner = Application.Current.MainWindow;
				setupWindow.ShowDialog();
			}

			return settingsService.CurrentSettings.AcceptedLicense;
		}

		// We only allow "normal" state
		private void OnWindowStateChanged(object sender, EventArgs e)
		{
			var window = Application.Current.MainWindow;
			if (window.WindowState != WindowState.Normal)
			{
				Logger.Current.Warning($"Window state changed to {window.WindowState}, this should not happen.");
				Application.Current.Dispatcher.BeginInvoke(new Action(() =>
				{
					window.WindowState = WindowState.Normal;
					window.Activate();
				}), DispatcherPriority.Background, null);
				
			}
			else
			{
				window.Activate();
			}
		}

		private void OnWindowSourceInitialized(object sender, EventArgs e)
		{
			try
			{
				// Attaching to desktop is currently bugged on Windows 7
				// Unknown if its fixable, so we deactivate it there for the time being
				if (EnvironmentUtil.IsWindows7())
				{
					return;
				}
				WindowUtil.AttachToDesktop(Application.Current.MainWindow);
			}
			catch (Exception ex)
			{
				ExceptionUtil.LogAndShowWarning("Could not attach widget to desktop.", ex);
			}
		}

		private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				WindowUtil.DetachFromDesktop(Application.Current.MainWindow);
			}
			catch (Exception ex)
			{
				Logger.Current.Warning("Could not detach widget from desktop.", ex);
			}
		}

		private void OnWindowLocationChanged(object sender, EventArgs e)
		{
			var window = Application.Current.MainWindow;

			if (!window.IsLoaded)
			{
				return;
			}

			settingsService.CurrentSettings.Top = window.Top;
			settingsService.CurrentSettings.Left = window.Left;
		}

		private void OnException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = e.ExceptionObject as Exception;
			if (ex == null)
			{
				return;
			}

			ExceptionUtil.IgnoreException(() => ExceptionUtil.LogAndShowError($"A critical error occured and the application will be closed now: {ex.Message}", ex));
			ExceptionUtil.IgnoreException(() => Application.Current.Shutdown(1));
		}

		private void OnDispatcherException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			ExceptionUtil.IgnoreException(() => ExceptionUtil.LogAndShowError($"A critical error occured and the application will be closed now: {e.Exception.Message}", e.Exception));
			ExceptionUtil.IgnoreException(() => Application.Current.Shutdown(1));
		}
	}
}

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

		public FrameworkElement Run()
		{
			try
			{
				Logger.Current.Info("Running bootstrapper...");

				// US culture for price formatting
				var usCulture = new CultureInfo("en-US");
				Thread.CurrentThread.CurrentCulture = usCulture;
				Thread.CurrentThread.CurrentUICulture = usCulture;

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
			
				var window = Application.Current.MainWindow;
				window.Top = settingsService.CurrentSettings.Top;
				window.Left = settingsService.CurrentSettings.Left;
				window.LocationChanged += OnLocationChanged;
				//window.Loaded += OnWindowLoaded;
				window.SourceInitialized += OnSourceInitialized;
				window.Closing += OnWindowClosing;
				Logger.Current.Info("Creating shell view...");

				var shell = ServiceLocator.Current.Get<ShellView>();

				Logger.Current.Info("Bootstrapper done.");

				return shell;
			}
			catch(Exception ex)
			{
				ExceptionUtil.LogAndShowError("There was an error initializing the app, it will be shut down.", ex);
				Application.Current.Shutdown();
			}

			return null;
		}

		private void OnSourceInitialized(object sender, EventArgs e)
		{
			try
			{
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

		private void OnLocationChanged(object sender, EventArgs e)
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

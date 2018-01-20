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

namespace Sineshift.DogecoinWidget
{
	public class Bootstrapper
	{
		SettingsService settingsService;

		public FrameworkElement Run()
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

			var window = Application.Current.MainWindow;
			window.Top = settingsService.CurrentSettings.Top;
			window.Left = settingsService.CurrentSettings.Left;
			window.LocationChanged += OnLocationChanged;
			window.Loaded += OnWindowLoaded;
			Logger.Current.Info("Creating shell view...");

			var shell = ServiceLocator.Current.Get<ShellView>();

			Logger.Current.Info("Bootstrapper done.");

			return shell;
		}

		private void OnWindowLoaded(object sender, RoutedEventArgs e)
		{
			try
			{
				WindowUtil.AttachToDesktop(Application.Current.MainWindow);
			}
			catch(Exception ex)
			{
				Logger.Current.Error("Could not attach widget to desktop.", ex);
				MessageBox.Show("Could not attach widget to desktop.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

			var text = ExceptionUtil.GetExceptionText(ex);
			Logger.Current.Error(text);
			ExceptionUtil.IgnoreException(() => MessageBox.Show($"A critical error occured and the application will be closed now: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
			ExceptionUtil.IgnoreException(() => Application.Current.Shutdown(1));
		}

		private void OnDispatcherException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			var text = ExceptionUtil.GetExceptionText(e.Exception);
			Logger.Current.Error(text);
			ExceptionUtil.IgnoreException(() => MessageBox.Show($"A critical error occured and the application will be closed now: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
			ExceptionUtil.IgnoreException(() => Application.Current.Shutdown(1));
		}
	}
}

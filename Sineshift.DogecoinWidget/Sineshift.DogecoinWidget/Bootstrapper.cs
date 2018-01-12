using Sineshift.DogecoinWidget.Common;
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

namespace Sineshift.DogecoinWidget
{
	public class Bootstrapper
	{
		public FrameworkElement Run()
		{
			// US culture for price formatting
			var usCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentCulture = usCulture;
			Thread.CurrentThread.CurrentUICulture = usCulture;

			Logger.Current.Info("Running bootstrapper...");

			Application.Current.DispatcherUnhandledException += OnDispatcherException;
			AppDomain.CurrentDomain.UnhandledException += OnException;

			ServiceLocator.Current.SetSingleton<ILogger>(Logger.Current);
			ServiceLocator.Current.SetSingleton<CoinMarketService>();

			Logger.Current.Info("Creating shell view...");

			var shell = ServiceLocator.Current.Get<ShellView>();

			Logger.Current.Info("Bootstrapper done.");

			return shell;
		}

		private void OnException(object sender, UnhandledExceptionEventArgs e)
		{
			var ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				var text = ExceptionUtil.GetExceptionText(e.ExceptionObject as Exception);
				Logger.Current.Error(text);
			}
		}

		private void OnDispatcherException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			var text = ExceptionUtil.GetExceptionText(e.Exception);
			Logger.Current.Error(text);
		}
	}
}

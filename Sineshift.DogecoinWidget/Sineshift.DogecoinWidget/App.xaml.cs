using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sineshift.DogecoinWidget
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			this.ShutdownMode = ShutdownMode.OnMainWindowClose;
			this.Startup += OnStartup;
		}

		private void OnStartup(object sender, StartupEventArgs e)
		{
			var bootstrapper = new Bootstrapper();
			this.MainWindow = bootstrapper.Run();

			if (bootstrapper.EnsureLicense())
			{
				this.MainWindow.Show();
				this.MainWindow.Activate();
			}
			else
			{
				this.Shutdown();
			}
		}
	}
}

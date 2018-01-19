using Sineshift.DogecoinWidget.Common;
using Sineshift.DogecoinWidget.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sineshift.DogecoinWidget.UI
{
	public class ShellViewModel : ObservableObject
	{
		Navigator navigator;

		public ShellViewModel(Navigator navigator)
		{
			this.navigator = navigator;

			navigator.Navigate<OverviewView>();
	
			CloseApplicationCommand = new RelayCommand(() => Application.Current.Shutdown());
			ShowAboutCommand = new RelayCommand(ShowAboutDialog);
			ShowSettingsCommand = new RelayCommand(ShowSettingsDialog);
		}

		public ICommand ShowAboutCommand
		{
			get;
			private set;
		}

		public ICommand ShowSettingsCommand
		{
			get;
			private set;
		}

		public ICommand CloseApplicationCommand
		{
			get;
			private set;
		}

		private void ShowAboutDialog()
		{
			var dialog = ServiceLocator.Current.Get<AboutWindow>();
			dialog.Owner = Application.Current.MainWindow;
			dialog.ShowDialog();
		}

		private void ShowSettingsDialog()
		{
			var dialog = ServiceLocator.Current.Get<SettingsWindow>();
			dialog.Owner = Application.Current.MainWindow;
			dialog.ShowDialog();
		}
	}
}

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
		object currentView;

		public ShellViewModel()
		{
			currentView = ServiceLocator.Current.Get<OverviewView>();
			CloseApplicationCommand = new RelayCommand(() => Application.Current.Shutdown());
		}

		public ICommand CloseApplicationCommand
		{
			get;
			private set;
		}

		public object CurrentView
		{
			get { return currentView; }
		}
	}
}

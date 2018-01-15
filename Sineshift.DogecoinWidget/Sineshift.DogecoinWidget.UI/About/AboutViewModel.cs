using Sineshift.DogecoinWidget.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sineshift.DogecoinWidget.UI
{
	public class AboutViewModel : ObservableObject
	{
		Navigator navigator;

		public AboutViewModel(Navigator navigator)
		{
			this.navigator = navigator;

			GoBackCommand = new RelayCommand(() => navigator.Navigate<OverviewView>());
		}

		public string Version
		{
			get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
		}

		public ICommand GoBackCommand
		{
			get;
			private set;
		}
	}
}

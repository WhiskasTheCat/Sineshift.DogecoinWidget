using Sineshift.DogecoinWidget.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sineshift.DogecoinWidget.UI
{
	public class AboutViewModel : ObservableObject
	{
		public AboutViewModel(Window parent)
		{
			GoBackCommand = new RelayCommand(() => parent.Close());
		}

		public string Version
		{
			get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); }
		}

		public ICommand GoBackCommand
		{
			get;
			private set;
		}
	}
}

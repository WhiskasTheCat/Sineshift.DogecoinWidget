using Sineshift.DogecoinWidget.Common.UI;
using Sineshift.DogecoinWidget.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sineshift.DogecoinWidget.UI
{
	public class SettingsViewModel : ObservableObject
	{
		readonly SettingsService settingsService;

		public SettingsViewModel(Window parent, SettingsService settingsService)
		{
			this.settingsService = settingsService;

			GoBackCommand = new RelayCommand(() => parent.Close());
		}

		public WidgetSettings CurrentSettings
		{
			get { return settingsService.CurrentSettings; }
		}

		public ICommand GoBackCommand
		{
			get;
			private set;
		}
	}
}

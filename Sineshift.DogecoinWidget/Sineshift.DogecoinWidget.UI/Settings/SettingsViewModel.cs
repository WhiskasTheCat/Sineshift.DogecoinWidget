using Sineshift.DogecoinWidget.Common.UI;
using Sineshift.DogecoinWidget.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			UIScales = new ObservableCollection<int>() { 50, 75, 100, 125, 150, 175, 200 };
		}

		public ObservableCollection<int> UIScales
		{
			get;
			private set;
		}

		public int SelectedUIScale
		{
			get { return (int)(CurrentSettings.UIScale * 100); }
			set { CurrentSettings.UIScale = value / 100.0; RaisePropertyChanged(); }
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

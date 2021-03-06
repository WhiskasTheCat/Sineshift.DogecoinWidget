﻿using Sineshift.DogecoinWidget.Common;
using Sineshift.DogecoinWidget.Services;
using Sineshift.DogecoinWidget.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sineshift.DogecoinWidget
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		SettingsService settingsService;

		public MainWindow(ShellView shellView, SettingsService settingsService)
		{
			InitializeComponent();

			this.mainRegion.Content = shellView;
			this.settingsService = settingsService;

			// Binding settings to window doesn't work for some reason, so we set it here
			settingsService.CurrentSettings.PropertyChanged += OnSettingsChanged;
			UpdateWindowScale();
		}

		private void OnSettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			UpdateWindowScale();
		}

		private void UpdateWindowScale()
		{
			this.Width = 320.0 * settingsService.CurrentSettings.UIScale;
			this.Height = 320.0 * settingsService.CurrentSettings.UIScale;
			this.windowScale.ScaleX = settingsService.CurrentSettings.UIScale;
			this.windowScale.ScaleY = settingsService.CurrentSettings.UIScale;
		}
	}
}

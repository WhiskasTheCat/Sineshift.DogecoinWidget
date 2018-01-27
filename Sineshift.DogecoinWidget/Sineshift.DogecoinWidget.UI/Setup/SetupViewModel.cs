using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Sineshift.DogecoinWidget.Common.UI;
using Sineshift.DogecoinWidget.Services;

namespace Sineshift.DogecoinWidget.UI
{
	public class SetupViewModel : ObservableObject
	{
		readonly SettingsService settingsService;
		readonly Window parent;
		readonly string licenseText = @"MIT License
Copyright (c) 2018 sineshift.com

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

		bool acceptedLicense;

		public SetupViewModel(Window parent, SettingsService settingsService)
		{
			this.parent = parent;
			this.settingsService = settingsService;

			ContinueCommand = new RelayCommand(Continue, CanContinue);
			ShutdownCommand = new RelayCommand(ContinueWithShutdown);
		}

		public bool AcceptedLicense
		{
			get { return acceptedLicense; }
			set { acceptedLicense = value; RaisePropertyChanged(); }
		}

		public WidgetSettings CurrentSettings
		{
			get { return settingsService.CurrentSettings; }
		}

		public ICommand ShutdownCommand
		{
			get;
			private set;
		}

		public ICommand ContinueCommand
		{
			get;
			private set;
		}

		public string LicenseText
		{
			get { return licenseText; }
		}

		private void Continue()
		{
			settingsService.CurrentSettings.AcceptedLicense = true;
			parent.Close();
		}

		private bool CanContinue()
		{
			return AcceptedLicense;
		}

		private void ContinueWithShutdown()
		{
			parent.Close();
		}
	}
}

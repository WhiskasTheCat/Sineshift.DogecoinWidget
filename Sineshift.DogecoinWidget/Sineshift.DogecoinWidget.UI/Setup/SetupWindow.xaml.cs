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
using System.Windows.Shapes;
using Sineshift.DogecoinWidget.Services;

namespace Sineshift.DogecoinWidget.UI
{
	/// <summary>
	/// Interaction logic for SetupWindow.xaml
	/// </summary>
	public partial class SetupWindow : Window
	{
		public SetupWindow(SettingsService settingsService)
		{
			InitializeComponent();

			this.DataContext = new SetupViewModel(this, settingsService);
		}
	}
}

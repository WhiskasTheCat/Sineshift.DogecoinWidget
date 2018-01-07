using Sineshift.DogecoinWidget.Common;
using Sineshift.DogecoinWidget.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sineshift.DogecoinWidget
{
	public class Bootstrapper
	{
		public FrameworkElement Run()
		{
			return ServiceLocator.Current.Get<OverviewView>();
		}
	}
}

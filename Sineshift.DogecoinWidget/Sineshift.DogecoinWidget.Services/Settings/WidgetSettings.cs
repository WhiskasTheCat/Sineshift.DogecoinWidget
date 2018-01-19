using Sineshift.DogecoinWidget.Common.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Services
{
	public class WidgetSettings : ObservableObject
	{
		double portfolioDogecoins;
		bool autoStart;

		public double PortfolioDogecoins
		{
			get { return portfolioDogecoins; }
			set { portfolioDogecoins = value; RaisePropertyChanged(); }
		}

		public bool AutoStart
		{
			get { return autoStart; }
			set { autoStart = value; RaisePropertyChanged(); }
		}
	}
}

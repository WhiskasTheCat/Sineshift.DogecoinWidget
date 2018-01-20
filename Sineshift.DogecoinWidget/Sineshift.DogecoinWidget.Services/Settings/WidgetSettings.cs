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
		double top;
		double left;
		bool autoStart;

		public double PortfolioDogecoins
		{
			get { return portfolioDogecoins; }
			set { portfolioDogecoins = value; RaisePropertyChanged(); }
		}

		public double Top
		{
			get { return top; }
			set { top = value; RaisePropertyChanged(); }
		}

		public double Left
		{
			get { return left; }
			set { left = value; RaisePropertyChanged(); }
		}

		public bool AutoStart
		{
			get { return autoStart; }
			set { autoStart = value; RaisePropertyChanged(); }
		}
	}
}

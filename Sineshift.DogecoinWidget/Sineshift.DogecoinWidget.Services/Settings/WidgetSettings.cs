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
		double uiScale;
		bool autoStart;
		bool isFirstStart;
		bool acceptedLicense;
		int selectedTabIndex;
		WidgetPlacement placement;

		public WidgetSettings()
		{
			isFirstStart = true;
			acceptedLicense = false;
			uiScale = 1.0;
			selectedTabIndex = 1;
			top = 100;
			left = 100;
			autoStart = false;
			portfolioDogecoins = 0;
			placement = WidgetPlacement.OnDesktop;
		}

		public WidgetPlacement Placement
		{
			get { return placement; }
			set { placement = value; RaisePropertyChanged(); }
		}

		public bool IsFirstStart
		{
			get { return isFirstStart; }
			set { isFirstStart = value; RaisePropertyChanged(); }
		}

		public bool AcceptedLicense
		{
			get { return acceptedLicense; }
			set { acceptedLicense = value; RaisePropertyChanged(); }
		}

		public int SelectedTabIndex
		{
			get { return selectedTabIndex; }
			set { selectedTabIndex = value; RaisePropertyChanged(); }
		}

		public double UIScale
		{
			get { return uiScale; }
			set { uiScale = value; RaisePropertyChanged(); }
		}

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

using Sineshift.DogecoinWidget.Common;
using Sineshift.DogecoinWidget.Common.UI;
using Sineshift.DogecoinWidget.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Sineshift.DogecoinWidget.UI
{
	public class OverviewViewModel : ObservableObject
	{
		readonly CoinMarketService marketService;
		readonly DispatcherTimer timer;
		CoinMarketInfo currentMarketInfo;
		bool showedError = false;

		public OverviewViewModel(CoinMarketService marketService)
		{
			this.marketService = marketService;

			UpdateMarketInfo();

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(10);
			timer.Tick += OnTick;
			timer.Start();
		}

		public CoinMarketInfo CurrentMarketInfo
		{
			get { return currentMarketInfo; }
			private set { currentMarketInfo = value; RaisePropertyChanged(); }
		}

		private void OnTick(object sender, EventArgs e)
		{
			UpdateMarketInfo();
		}

		private async void UpdateMarketInfo()
		{
			try
			{
				CurrentMarketInfo = await marketService.GetCurrentMarketInfo();
			}
			catch(Exception ex)
			{
				Logger.Current.Error("Could not update market info", ex);
				if (!showedError)
				{
					showedError = true;
					MessageBox.Show("Could not update market info. Make sure you are connected to the internet. You may also need to add a firewall exception for this program.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}
	}
}

using Sineshift.DogecoinWidget.Common;
using Sineshift.DogecoinWidget.Common.UI;
using Sineshift.DogecoinWidget.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public OverviewViewModel(CoinMarketService marketService)
		{
			this.marketService = marketService;
			System.Diagnostics.Debug.WriteLine(DateTime.Now.ToEpochTime());

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(60);
			timer.Tick += OnTick;
			timer.Start();

			UpdateMarketInfo();
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

				Logger.Current.Debug($"PriceInfo: BTC = {CurrentMarketInfo.PriceBTC}, USD = {CurrentMarketInfo.PriceUSD}");
			}
			catch(Exception ex)
			{
				Logger.Current.Error("Could not update market info", ex);
				timer.Stop();
				MessageBox.Show($"Could not update market info.\nReason: {ex.Message}.\nThis can have several reasons:\n1. You are not connected to the internet.\n2. You need to add a firewall exception for this application.\n3. Our price data source is down.\nThe program will reattempt to get market data in 1 minute or next time it is restarted.\nYou can find the technical reason for this error in the log file located here:\n{Logger.Current.LogPath}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				timer.Start();
			}
		}
	}
}

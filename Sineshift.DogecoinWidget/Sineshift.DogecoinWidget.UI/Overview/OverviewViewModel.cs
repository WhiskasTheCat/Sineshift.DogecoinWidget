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
		BitcoinDollarPair lastPair;

		public OverviewViewModel(CoinMarketService marketService)
		{
			this.marketService = marketService;
			System.Diagnostics.Debug.WriteLine(DateTime.Now.ToEpochTime());
			PricePairs = new ObservableCollection<BitcoinDollarPair>();
			MaxPricePairs = 60;

			UpdateMarketInfo();

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(60);
			timer.Tick += OnTick;
			timer.Start();
		}

		public ObservableCollection<BitcoinDollarPair> PricePairs
		{
			get;
			private set;
		}

		public int MaxPricePairs
		{
			get;
			private set;
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

				var newPair = new BitcoinDollarPair()
				{
					PriceBTC = CurrentMarketInfo.PriceBTC,
					PriceUSD = CurrentMarketInfo.PriceUSD
				};

				Logger.Current.Debug($"PriceInfo: BTC = {CurrentMarketInfo.PriceBTC}, USD = {CurrentMarketInfo.PriceUSD}");

				// If we get the exact same price again ignore it. This can happen if data on the price server is updated slower than our update interval
				if (lastPair != null && lastPair.Equals(newPair))
				{
					return;
				}

				PricePairs.Add(newPair);
				lastPair = newPair;

				if (PricePairs.Count > MaxPricePairs)
				{
					PricePairs.RemoveAt(0);
				}
			}
			catch(Exception ex)
			{
				Logger.Current.Error("Could not update market info", ex);
				MessageBox.Show("Could not update market info.\nMake sure you are connected to the internet.\nYou may also need to add a firewall exception for this program.\nIf it still does not work after multiple attempts, our price data provider might have issues.\nThe program will reattempt to get market data in 1 minute or next time it is restarted.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
	}
}

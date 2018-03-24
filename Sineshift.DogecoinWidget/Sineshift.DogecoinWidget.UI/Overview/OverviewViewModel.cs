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
	public class OverviewViewModel : ObservableObject, INavigationObserver
	{
		readonly CoinMarketService marketService;
		readonly DispatcherTimer timer;
		readonly SettingsService settingsService;
		CoinMarketInfo currentMarketInfo;
		IReadOnlyList<BitcoinDollarPair> prices1H;
		IReadOnlyList<BitcoinDollarPair> prices1D;
		IReadOnlyList<BitcoinDollarPair> prices7D;
		IReadOnlyList<BitcoinDollarPair> prices1M;
		bool isDataLoaded;
		string statusText;

		public OverviewViewModel(CoinMarketService marketService, SettingsService settingsService)
		{
			this.marketService = marketService;
			this.settingsService = settingsService;

			settingsService.CurrentSettings.PropertyChanged += (sender, e) => UpdatePortfolio();

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(60);
			timer.Tick += OnTick;

			UpdateMarketInfo();
		}

		public string StatusText
		{
			get { return statusText; }
			private set { statusText = value; RaisePropertyChanged(); }
		}

		public bool IsDataLoaded
		{
			get { return isDataLoaded; }
			private set { isDataLoaded = value; RaisePropertyChanged(); }
		}

		public WidgetSettings CurrentSettings
		{
			get { return settingsService.CurrentSettings; }
		}

		public double PortfolioPriceUSD
		{
			get { return CurrentMarketInfo == null ? 0 : CurrentMarketInfo.PriceUSD * CurrentSettings.PortfolioDogecoins; }
		}

		public double PortfolioPriceBTC
		{
			get { return CurrentMarketInfo == null ? 0 : CurrentMarketInfo.PriceBTC * CurrentSettings.PortfolioDogecoins; }
		}

		public IReadOnlyList<BitcoinDollarPair> Prices1H
		{
			get { return prices1H; }
			private set { prices1H = value; RaisePropertyChanged(); }
		}

		public IReadOnlyList<BitcoinDollarPair> Prices1D
		{
			get { return prices1D; }
			private set { prices1D = value; RaisePropertyChanged(); }
		}

		public IReadOnlyList<BitcoinDollarPair> Prices7D
		{
			get { return prices7D; }
			private set { prices7D = value; RaisePropertyChanged(); }
		}

		public IReadOnlyList<BitcoinDollarPair> Prices1M
		{
			get { return prices1M; }
			private set { prices1M = value; RaisePropertyChanged(); }
		}

		public CoinMarketInfo CurrentMarketInfo
		{
			get { return currentMarketInfo; }
			private set { currentMarketInfo = value; RaisePropertyChanged(); }
		}

		public void NavigatedFrom()
		{
			timer.Stop();
		}

		public void NavigatedTo(object parameter)
		{
			timer.Start();
		}

		private void OnTick(object sender, EventArgs e)
		{
			UpdateMarketInfo();
		}

		private async void UpdateMarketInfo()
		{
			IsDataLoaded = false;
			StatusText = "Loading much data...";

			try
			{
				Logger.Current.Debug("Getting market info");
				var taskMarket = marketService.GetCurrentMarketInfo();
				Logger.Current.Debug("Getting prices");
				var task1D = marketService.GetPrices1D();
				var task7D = marketService.GetPrices7D();
				var task1M = marketService.GetPrices1M();

				await Task.WhenAll(taskMarket, task1D, task7D, task1M);

				CurrentMarketInfo = taskMarket.Result;
				//Prices1H = task1H.Result;
				Prices1D = task1D.Result;
				Prices7D = task7D.Result;
				Prices1M = task1M.Result;

				IsDataLoaded = CurrentMarketInfo != null;
				UpdatePortfolio();
				Logger.Current.Debug($"PriceInfo: BTC = {CurrentMarketInfo.PriceBTC}, USD = {CurrentMarketInfo.PriceUSD}");
			}
			catch(Exception ex)
			{
				timer.Stop();
				StatusText = $"Could not update market info and will try again in 1 minute: {ex.Message}.";
				//ExceptionUtil.LogAndShowWarning("Could not update market info", ex);
				Logger.Current.Error("Could not update market info", ex);
				timer.Start();
			}
		}

		private void UpdatePortfolio()
		{
			RaisePropertyChanged(nameof(PortfolioPriceBTC));
			RaisePropertyChanged(nameof(PortfolioPriceUSD));
		}
	}
}

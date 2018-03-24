using CryptoCompare;
using NoobsMuc.Coinmarketcap.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Services
{
	public class CoinMarketService
	{
		readonly ICoinmarketcapClient marketCapClient;
		readonly CryptoCompareClient cryptoCompareClient;
		readonly CultureInfo usCulture = new CultureInfo("en-US");
		readonly string exchange = "Bittrex";

		public CoinMarketService()
		{
			marketCapClient = new CoinmarketcapClient();
			cryptoCompareClient = new CryptoCompareClient();
			exchange = "Bittrex";
		}

		public Task<CoinMarketInfo> GetCurrentMarketInfo()
		{
			return Task.Run(() =>
			{
				var currency = marketCapClient.GetCurrencyById("dogecoin");

				return new CoinMarketInfo()
				{
					PriceUSD = double.Parse(currency.PriceUsd, usCulture),
					PriceBTC = double.Parse(currency.PriceBtc, usCulture),
					PercentChange1H = double.Parse(currency.PercentChange1h, usCulture),
					PercentChange24H = double.Parse(currency.PercentChange24h, usCulture),
					PercentChange7D = double.Parse(currency.PercentChange7d, usCulture),
					MarketCapUSD = double.Parse(currency.MarketCapUsd, usCulture),
					Volume24HUSD = double.Parse(currency.Volume24hUsd, usCulture),
					Rank = int.Parse(currency.Rank)
				};
			});

		}

		public async Task<IReadOnlyList<BitcoinDollarPair>> GetPrices1H()
		{
			var dogeBtcTask = cryptoCompareClient.History.MinuteAsync("DOGE", "BTC", null, exchange, DateTime.UtcNow);

			// We don't get DOGE-USD directly from the API, so we will do a conversion later. The conversion flag in the API doesn't work either.
			var btcUsdTask = cryptoCompareClient.History.MinuteAsync("BTC", "USD", null, exchange, DateTime.UtcNow);

			await Task.WhenAll(dogeBtcTask, btcUsdTask);

			CheckDataTasks(dogeBtcTask, btcUsdTask);

			return GetPriceList(dogeBtcTask, btcUsdTask, DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)), DateTime.UtcNow);
		}

		public async Task<IReadOnlyList<BitcoinDollarPair>> GetPrices1D()
		{
			var dogeBtcTask = cryptoCompareClient.History.HourAsync("DOGE", "BTC", null, exchange, DateTime.UtcNow);

			// We don't get DOGE-USD directly from the API, so we will do a conversion later. The conversion flag in the API doesn't work either.
			var btcUsdTask = cryptoCompareClient.History.HourAsync("BTC", "USD", null, exchange, DateTime.UtcNow);

			await Task.WhenAll(dogeBtcTask, btcUsdTask);

			CheckDataTasks(dogeBtcTask, btcUsdTask);

			return GetPriceList(dogeBtcTask, btcUsdTask, DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)), DateTime.UtcNow);

			// if we want to go for minute prices every nth minute use this. But we will also have to compute averages
			//return list.Where((x, i) => (i + 1) % 15 == 0).ToList();
		}

		public async Task<IReadOnlyList<BitcoinDollarPair>> GetPrices7D()
		{
			var dogeBtcTask = cryptoCompareClient.History.HourAsync("DOGE", "BTC", null, exchange, DateTime.UtcNow);

			// We don't get DOGE-USD directly from the API, so we will do a conversion later. The conversion flag in the API doesn't work either.
			var btcUsdTask = cryptoCompareClient.History.HourAsync("BTC", "USD", null, exchange, DateTime.UtcNow);

			await Task.WhenAll(dogeBtcTask, btcUsdTask);

			CheckDataTasks(dogeBtcTask, btcUsdTask);

			return GetPriceList(dogeBtcTask, btcUsdTask, DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)), DateTime.UtcNow);
		}

		public async Task<IReadOnlyList<BitcoinDollarPair>> GetPrices1M()
		{
			var dogeBtcTask = cryptoCompareClient.History.DayAsync("DOGE", "BTC", null, exchange, DateTime.UtcNow);

			// We don't get DOGE-USD directly from the API, so we will do a conversion later. The conversion flag in the API doesn't work either.
			var btcUsdTask = cryptoCompareClient.History.DayAsync("BTC", "USD", null, exchange, DateTime.UtcNow);

			await Task.WhenAll(dogeBtcTask, btcUsdTask);

			CheckDataTasks(dogeBtcTask, btcUsdTask);

			return GetPriceList(dogeBtcTask, btcUsdTask, DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)), DateTime.UtcNow);
		}

		private void CheckDataTasks(Task<HistoryResponse> dogeBtcTask, Task<HistoryResponse> btcUsdTask)
		{
			if (dogeBtcTask.IsFaulted || btcUsdTask.IsFaulted || !dogeBtcTask.Result.IsSuccessfulResponse || !btcUsdTask.Result.IsSuccessfulResponse)
			{
				throw new InvalidOperationException("Could not get data from CryptoCompare.");
			}

			if (dogeBtcTask.Result.Data.Count != btcUsdTask.Result.Data.Count)
			{
				throw new InvalidOperationException($"There is a different amount of results for DOGE ({dogeBtcTask.Result.Data.Count}) and BTC ({btcUsdTask.Result.Data.Count}) returned by CryptoCompare.");
			}
		}

		private IReadOnlyList<BitcoinDollarPair> GetPriceList(Task<HistoryResponse> dogeBtcTask, Task<HistoryResponse> btcUsdTask, DateTime fromDate, DateTime toDate)
		{
			var dogeBtcPrices = dogeBtcTask.Result.Data
				.OrderBy(p => p.Time.ToUnixTimeSeconds())
				.Where(p => p.Time.DateTime >= fromDate && p.Time.DateTime <= toDate)
				.ToList();
			var btcUsdPrices = btcUsdTask.Result.Data
				.OrderBy(p => p.Time.ToUnixTimeSeconds())
				.Where(p => p.Time.DateTime >= fromDate && p.Time.DateTime <= toDate)
				.ToList();

			var priceHistory = new List<BitcoinDollarPair>();

			for (int i = 0; i < dogeBtcPrices.Count; i++)
			{
				var btcUsdPrice = btcUsdPrices[i].Close;
				var dogeBtcPrice = dogeBtcPrices[i].Close;

				// Convert BTC-USD to DOGE-USD
				var dogeUsdPrice = dogeBtcPrice * btcUsdPrice;

				var pair = new BitcoinDollarPair()
				{
					PriceBTC = (double)dogeBtcPrice,
					PriceUSD = (double)dogeUsdPrice,
					Volume = (double)dogeBtcPrices[i].VolumeFrom
				};

				priceHistory.Add(pair);
			}

			return priceHistory;
		}
	}
}

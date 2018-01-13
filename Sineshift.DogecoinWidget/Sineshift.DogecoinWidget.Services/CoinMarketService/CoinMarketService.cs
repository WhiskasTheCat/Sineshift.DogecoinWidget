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
		readonly ICoinmarketcapClient client;
		readonly CryptoCompareClient client2;
		readonly CultureInfo usCulture = new CultureInfo("en-US");
		readonly IReadOnlyList<string> exchanges = new List<string>() { "Bittrex" };

		public CoinMarketService()
		{
			client = new CoinmarketcapClient();
			client2 = new CryptoCompareClient();
		}

		public async Task<CoinMarketInfo> GetCurrentMarketInfo()
		{
			var marketCapTask = GetCoinMarketInfo();
			var dogeBtcTask = client2.History.MinuteAsync("DOGE", "BTC", null, exchanges, DateTime.UtcNow);

			// We don't get DOGE-USD directly from the API, so we will do a conversion later. The conversion flag in the API doesn't work either.
			var btcUsdTask = client2.History.MinuteAsync("BTC", "USD", null, exchanges, DateTime.UtcNow);
			
			await Task.WhenAll(dogeBtcTask, btcUsdTask, marketCapTask);

			if (dogeBtcTask.IsFaulted || btcUsdTask.IsFaulted || !dogeBtcTask.Result.IsSuccessfulResponse || !btcUsdTask.Result.IsSuccessfulResponse)
			{
				throw new InvalidOperationException("Could not get data from CryptoCompare.");
			}

			if (dogeBtcTask.Result.Data.Count != btcUsdTask.Result.Data.Count)
			{
				throw new InvalidOperationException($"There is a different amount of results for DOGE ({dogeBtcTask.Result.Data.Count}) and BTC ({btcUsdTask.Result.Data.Count}) returned by CryptoCompare.");
			}

			if (marketCapTask.IsFaulted)
			{
				throw new InvalidOperationException("Could not get data from CoinMarketCap.");
			}

			var itemCount = 60;
			var dogeBtcPrices = dogeBtcTask.Result.Data
				.OrderBy(p => p.Time.ToUnixTimeSeconds())
				.Skip(dogeBtcTask.Result.Data.Count - itemCount)
				.ToList();
			var btcUsdPrices = btcUsdTask.Result.Data
				.OrderBy(p => p.Time.ToUnixTimeSeconds())
				.Skip(btcUsdTask.Result.Data.Count - itemCount)
				.ToList();
			var marketInfo = marketCapTask.Result;
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

			marketInfo.PriceHistory1H = priceHistory;
			return marketInfo;
			
		}

		private Task<CoinMarketInfo> GetCoinMarketInfo()
		{
			return Task.Run(() =>
			{
				var currency = client.GetCurrencyById("dogecoin");

				return new CoinMarketInfo()
				{
					PriceUSD = double.Parse(currency.PriceUsd, usCulture),
					PriceBTC = double.Parse(currency.PriceBtc, usCulture),
					PercentChange1H = double.Parse(currency.PercentChange1h, usCulture),
					PercentChange24H = double.Parse(currency.PercentChange24h, usCulture),
					PercentChange7D = double.Parse(currency.PercentChange7d, usCulture),
					MarketCapUSD = double.Parse(currency.MarketCapUsd, usCulture),
					VolumeUSD = double.Parse(currency.Volume24hUsd, usCulture),
					Rank = int.Parse(currency.Rank)
				};
			});
		}
	}
}

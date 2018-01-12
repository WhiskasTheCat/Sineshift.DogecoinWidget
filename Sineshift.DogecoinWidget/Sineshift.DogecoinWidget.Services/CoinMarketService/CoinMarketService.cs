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
		ICoinmarketcapClient client;
		readonly CultureInfo usCulture = new CultureInfo("en-US");

		public CoinMarketService()
		{
			client = new CoinmarketcapClient();
		}

		public Task<CoinMarketInfo> GetCurrentMarketInfo()
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

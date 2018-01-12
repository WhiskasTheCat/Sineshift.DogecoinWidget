using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Services
{
	public class CoinMarketInfo
	{
		public double PriceUSD
		{
			get;
			internal set;
		}

		public double PriceBTC
		{
			get;
			internal set;
		}

		public double PercentChange1H
		{
			get;
			internal set;
		}

		public double PercentChange24H
		{
			get;
			internal set;
		}

		public double PercentChange7D
		{
			get;
			internal set;
		}

		public double MarketCapUSD
		{
			get;
			internal set;
		}

		public double VolumeUSD
		{
			get;
			internal set;
		}

		public int Rank
		{
			get;
			internal set;
		}
	}
}

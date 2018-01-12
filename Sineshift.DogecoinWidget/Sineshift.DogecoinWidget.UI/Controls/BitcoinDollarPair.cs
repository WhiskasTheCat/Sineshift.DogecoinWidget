using Sineshift.DogecoinWidget.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.UI
{
    public class BitcoinDollarPair
    {
		public double PriceBTC
		{
			get;
			set;
		}

		public double PriceUSD
		{
			get;
			set;
		}
		public override int GetHashCode()
		{
			return HashUtil.CombineHashes(PriceBTC.GetHashCode(), PriceUSD.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			var other = obj as BitcoinDollarPair;
			return PriceBTC.AlmostEquals(other.PriceBTC) && PriceUSD.AlmostEquals(other.PriceUSD);
		}
	}
}

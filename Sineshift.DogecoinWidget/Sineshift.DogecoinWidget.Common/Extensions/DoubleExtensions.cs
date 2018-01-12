using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public static class DoubleExtensions
	{
		public static bool AlmostEquals(this double a, double b)
		{
			return MathUtil.AreAlmostEqual(a, b);
		}

		public static double Limit(this double value, double min, double max)
		{
			return MathUtil.Limit(value, min, max);
		}
	}
}

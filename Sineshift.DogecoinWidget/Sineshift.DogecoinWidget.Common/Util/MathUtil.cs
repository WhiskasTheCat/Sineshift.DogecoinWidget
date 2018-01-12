using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Sineshift.DogecoinWidget.Common
{
	public static class MathUtil
	{
		const double multipleEpsilon = double.Epsilon * 5.0;
		const double minValueEpsilon = multipleEpsilon * double.MinValue;

		public static bool AreAlmostEqual(double a, double b)
		{
			if (a == b)
				return true;

			double diff = Math.Abs(a - b);

			if (a == 0 || b == 0 || diff < double.MinValue)
			{
				// a or b is zero or both are extremely close to it
				// relative error is less meaningful here
				return diff < minValueEpsilon;
			}
			else
			{
				double absA = Math.Abs(a);
				double absB = Math.Abs(b);

				// use relative error
				return diff / (absA + absB) < multipleEpsilon;
			}
		}

		public static double Limit(double value, double min, double max)
		{
			return value < min ? min :
				value > max ? max : value;
		}

		public static double Interpolate(double a, double b, double factor)
		{
			return a * (1.0 - factor) + b * factor;
		}
	}
}

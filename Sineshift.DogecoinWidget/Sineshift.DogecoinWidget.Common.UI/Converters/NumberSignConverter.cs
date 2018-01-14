using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sineshift.DogecoinWidget.Common.UI
{
	public class NumberSignConverter : IValueConverter
	{
		public object PositiveValue
		{
			get;
			set;
		}

		public object NegativeValue
		{
			get;
			set;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return NegativeValue;
			}

			double result;
			if (!double.TryParse(value.ToString(), out result))
			{
				return NegativeValue;
			}
			else
			{
				return result >= 0.0 ? PositiveValue : NegativeValue;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

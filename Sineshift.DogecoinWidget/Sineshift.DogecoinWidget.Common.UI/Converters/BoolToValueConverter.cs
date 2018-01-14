using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sineshift.DogecoinWidget.Common.UI
{
	public class BoolToValueConverter : IValueConverter
	{
		public object TrueValue
		{
			get;
			set;
		}

		public object FalseValue
		{
			get;
			set;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return FalseValue;
			}

			bool result;
			if (!bool.TryParse(value.ToString(), out result))
			{
				return FalseValue;
			}
			else
			{
				return result ? TrueValue : FalseValue;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

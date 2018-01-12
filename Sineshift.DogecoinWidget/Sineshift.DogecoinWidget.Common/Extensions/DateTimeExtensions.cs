using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public static class DateTimeExtensions
	{
		public static long ToEpochTime(this DateTime date)
		{
			return DateTimeUtil.GetEpochTime(date);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public static class Guard
	{
		public static void IsTrue(bool exp, string message)
		{
			if (!exp)
			{
				throw new InvalidOperationException(message);
			}
		}

		public static void IsOfType<T>(object obj, string message)
		{
			if (!(obj is T))
			{
				throw new InvalidOperationException(message);
			}
		}

		public static void IsNotNull(object obj, string message)
		{
			if (obj == null)
			{
				throw new InvalidOperationException(message);
			}
		}
	}
}

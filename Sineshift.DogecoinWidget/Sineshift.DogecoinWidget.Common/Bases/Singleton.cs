using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public abstract class Singleton<TImplementation> : Singleton<TImplementation, TImplementation>
	{
		protected Singleton()
		{

		}
	}

	public abstract class Singleton<TImplementation, TCurrent>
		where TImplementation : TCurrent
	{
		private static Lazy<TCurrent> current = new Lazy<TCurrent>(() => CreateInstance());

		protected Singleton()
		{

		}

		public static TCurrent Current
		{
			get => current.Value;
		}

		private static TCurrent CreateInstance()
		{
			return (TCurrent)ReflectionUtil.CreateInstance(typeof(TImplementation));
		}

		public static void SetCurrent(TCurrent newCurrent)
		{
			current = new Lazy<TCurrent>(() => newCurrent);
		}
	}
}

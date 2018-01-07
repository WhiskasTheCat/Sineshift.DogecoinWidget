using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public class ServiceLocator : Singleton<ServiceLocator, IServiceLocator>, IServiceLocator
	{
		private StandardKernel kernel;

		private ServiceLocator()
		{
			kernel = new StandardKernel();
			SetSingleton<IServiceLocator>(this);
		}

		public T Get<T>()
		{
			return kernel.Get<T>();
		}

		public IServiceLocator SetSingleton<T>()
		{
			kernel.Bind<T>().ToSelf().InSingletonScope();
			return this;
		}

		public IServiceLocator SetSingleton<T>(T instance)
		{
			kernel.Bind<T>().ToConstant(instance).InSingletonScope();
			return this;
		}
	}
}

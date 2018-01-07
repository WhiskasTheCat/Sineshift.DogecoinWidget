using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public interface IServiceLocator
	{
		T Get<T>();
		IServiceLocator SetSingleton<T>();
		IServiceLocator SetSingleton<T>(T instance);
	}
}

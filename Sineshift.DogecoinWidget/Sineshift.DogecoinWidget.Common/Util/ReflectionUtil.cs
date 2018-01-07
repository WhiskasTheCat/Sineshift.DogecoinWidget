using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public static class ReflectionUtil
	{
		static Dictionary<Type, Func<object>> factoryMethods;

		static ReflectionUtil()
		{
			factoryMethods = new Dictionary<Type, Func<object>>();
		}

		public static T CreateInstance<T>() where T : new()
		{
			return (T)CreateInstance(typeof(T));
		}

		public static object CreateInstance(Type type)
		{
			if (!factoryMethods.ContainsKey(type))
				factoryMethods[type] = CreateFactoryMethod(type);

			return factoryMethods[type]();
		}

		private static Func<object> CreateFactoryMethod<T>()
		{
			return CreateFactoryMethod(typeof(T));
		}

		private static Func<object> CreateFactoryMethod(Type type)
		{
			return Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
		}
	}
}

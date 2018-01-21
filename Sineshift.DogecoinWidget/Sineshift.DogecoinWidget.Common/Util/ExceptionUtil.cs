using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sineshift.DogecoinWidget.Common
{
	public static class ExceptionUtil
	{
		public static string GetExceptionText(Exception ex)
		{
			var exception = ex;
			var exceptionText = new StringBuilder();

			exceptionText.AppendLine("Exceptions:");

			while (exception != null)
			{
				exceptionText.AppendLine($"   {exception.GetType()}: {exception.Message}");
				exception = exception.InnerException;
			}

			exceptionText.AppendLine("StackTrace:");
			exceptionText.AppendLine(ex.StackTrace);

			return exceptionText.ToString();
		}

		public static void LogAndShowError(string message, Exception ex)
		{
			Logger.Current.Error(message, ex);
			MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public static void LogAndShowWarning(string message, Exception ex)
		{
			Logger.Current.Error(message, ex);
			MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		public static void IgnoreException(Action action)
		{
			try
			{
				action();
			}
			catch
			{

			}
		}

		public static T IgnoreException<T>(Func<T> fn)
		{
			try
			{
				return fn();
			}
			catch
			{
				return default(T);
			}
		}
	}
}

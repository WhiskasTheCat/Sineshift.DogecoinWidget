using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}

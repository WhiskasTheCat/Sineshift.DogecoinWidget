using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public interface ILogger
	{
		void Info(string message);
		void Debug(string message);
		void Warning(string message);
		void Error(string message);
		void Error(string message, Exception ex);
	}
}

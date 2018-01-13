using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public class Logger : Singleton<Logger, ILogger>, ILogger
	{
		private readonly static object locker = new object();

		private Logger()
		{
			LogPath = EnvironmentUtil.GetAppDataPath("Log.txt");

			// We delete the log if it gets too big
			try
			{
				var maxLogSize = 1024 * 1024 * 10; // 10 MB

				if (File.Exists(LogPath) && new FileInfo(LogPath).Length > maxLogSize)
				{
					File.Delete(LogPath);
				}
			}
			catch (Exception)
			{

			}
		}

		public string LogPath
		{
			get;
			private set;
		}

		public void Debug(string message)
		{
			WriteLog("Debug", message);
		}

		public void Error(string message)
		{
			WriteLog("Error", message);
		}

		public void Error(string message, Exception ex)
		{
			var exceptionText = ExceptionUtil.GetExceptionText(ex);
			WriteLog("Error", $"{message}: {exceptionText}");
		}

		public void Info(string message)
		{
			WriteLog("Info", message);
		}

		public void Warning(string message)
		{
			WriteLog("Warning", message);
		}

		private void WriteLog(string category, string message)
		{
			lock (locker)
			{
				var time = DateTime.Now.ToString();
				var log = $"[{time}] [{category}] {message} {Environment.NewLine}";

				System.Diagnostics.Debug.WriteLine(log);

				if (category != "Debug")
				{
					File.AppendAllText(LogPath, log);
				}
			}
		}
	}
}
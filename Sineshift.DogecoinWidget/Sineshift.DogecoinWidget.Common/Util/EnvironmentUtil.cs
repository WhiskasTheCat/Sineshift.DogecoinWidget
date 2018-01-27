using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public static class EnvironmentUtil
	{
		const string companyName = "Sineshift";
		const string appName = "DogecoinWidget";

		public static string GetAppDataPath(string filename)
		{
			var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			var companyPath = Path.Combine(appDataPath, companyName);
			if (!Directory.Exists(companyPath))
			{
				Directory.CreateDirectory(companyPath);
			}

			var appPath = Path.Combine(companyPath, appName);
			if (!Directory.Exists(appPath))
			{
				Directory.CreateDirectory(appPath);
			}

			return Path.Combine(appPath, filename);
		}

		public static string GetCurrentAppPath()
		{
			var startupAssembly = Assembly.GetEntryAssembly();
			return startupAssembly.Location;
		}

		public static bool IsWindows7()
		{
			var osVersion = Environment.OSVersion;
			return osVersion.Version.Major == 6 && osVersion.Version.Minor == 1;
		}
	}
}

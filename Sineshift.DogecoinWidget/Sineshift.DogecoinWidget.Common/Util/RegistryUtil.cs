using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sineshift.DogecoinWidget.Common
{
	public static class RegistryUtil
	{
		static readonly string startUpKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
		static readonly string appKey = "Sineshift.DogecoinWidget";

		public static bool IsStartupApp()
		{
			try
			{
				using (var registryKey = Registry.CurrentUser.OpenSubKey(startUpKey, false))
				{
					var registryValue = registryKey.GetValue(appKey);
					return registryValue != null;
				}
			}
			catch(Exception ex)
			{
				Logger.Current.Error("Could check if widget is a startup app.", ex);
				throw;
			}
		}

		public static void SetStartupApp(bool startup)
		{
			try
			{
				using (var registryKey = Registry.CurrentUser.OpenSubKey(startUpKey, true))
				{
					if (startup)
					{
						var currentPath = EnvironmentUtil.GetCurrentAppPath();
						registryKey.SetValue(appKey, currentPath);
					}
					else
					{
						registryKey.DeleteValue(appKey);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Current.Error("Could change the startup state of the widget.", ex);
				throw;
			}
		}
	}
}

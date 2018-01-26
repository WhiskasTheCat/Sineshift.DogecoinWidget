using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Sineshift.DogecoinWidget.Common
{
	public static class WindowUtil
	{
		public static void AttachToDesktop(Window window)
		{
			var windowHandle = new WindowInteropHelper(window).Handle;
			if (windowHandle == IntPtr.Zero)
			{
				throw new InvalidOperationException("Window handle is zero.");
			}
			var handle1 = FindWindow("Progman", null);
			var handle2 = FindWindowEx(handle1, IntPtr.Zero, "SHELLDLL_DefVIew", null);
			var handle3 = FindWindowEx(handle2 == IntPtr.Zero ? handle1 : handle2, IntPtr.Zero, "SysListView32", null);

			var desktopHandle = handle3 == IntPtr.Zero ?
				(handle2 == IntPtr.Zero ? handle1 : handle2) :
				handle3;

			if (desktopHandle == IntPtr.Zero)
			{
				throw new InvalidOperationException("Desktop handle is zero.");
			}

			// TO DO: Test this to use in Detach instead of IntPtr.Zero
			var originalParent = SetParent(windowHandle, desktopHandle);
		}

		public static void DetachFromDesktop(Window window)
		{
			var hWnd = new WindowInteropHelper(window).Handle;
			SetParent(hWnd, IntPtr.Zero);
		}

		#region Win32
		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

		private enum ABM : uint
		{
			New = 0x00000000,
			Remove = 0x00000001,
			QueryPos = 0x00000002,
			SetPos = 0x00000003,
			GetState = 0x00000004,
			GetTaskbarPos = 0x00000005,
			Activate = 0x00000006,
			GetAutoHideBar = 0x00000007,
			SetAutoHideBar = 0x00000008,
			WindowPosChanged = 0x00000009,
			SetState = 0x0000000A,
		}

		private enum ABE : uint
		{
			Left = 0,
			Top = 1,
			Right = 2,
			Bottom = 3
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}
		#endregion
	}
}

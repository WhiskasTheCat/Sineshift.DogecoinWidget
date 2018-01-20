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
			IntPtr hWnd = new WindowInteropHelper(window).Handle;
			IntPtr hWndProgMan = FindWindow("Progman", "Program Manager");
			SetParent(hWnd, hWndProgMan);
		}

		#region Win32
		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

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

		private static class ABS
		{
			public const int Autohide = 0x0000001;
			public const int AlwaysOnTop = 0x0000002;
		}

		private static class Shell32
		{
			[DllImport("shell32.dll", SetLastError = true)]
			public static extern IntPtr SHAppBarMessage(ABM dwMessage, [In] ref APPBARDATA pData);
		}

		private static class User32
		{
			[DllImport("user32.dll", SetLastError = true)]
			public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct APPBARDATA
		{
			public uint cbSize;
			public IntPtr hWnd;
			public uint uCallbackMessage;
			public ABE uEdge;
			public RECT rc;
			public int lParam;
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

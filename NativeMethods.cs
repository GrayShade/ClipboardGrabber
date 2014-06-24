using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;

namespace ClipboardGrabber
{
	static class NativeMethods
	{
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool DestroyIcon(IntPtr hIcon);

		internal enum ImageType
		{
			Bitmap = 0,
			Icon = 1,
			Cursor = 2,
			EnhMetafile = 3,
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private static extern IntPtr LoadImage(IntPtr hinst, int lpszName, ImageType uType, int cxDesired, int cyDesired, int fuLoad);
		
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetOpenClipboardWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int GetWindowTextInternal(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

		public static Icon GetApplicationIcon()
		{
			Icon icon = null;
			try
			{
				var handle = LoadImage(Process.GetCurrentProcess().MainModule.BaseAddress, 0x7f00, ImageType.Icon, 0, 0, 0x0040);
				icon = Icon.FromHandle(handle).Clone() as Icon;
				DestroyIcon(handle);
			}
			catch
			{
			}
			return icon;
		}

		public static string GetWindowText(IntPtr handle)
		{
			var sb = new StringBuilder(GetWindowTextLength(handle));
			GetWindowTextInternal(handle, sb, sb.Capacity);
			return sb.ToString();
		}
	}
}

using System;
using System.Windows.Forms;
using System.Net;

namespace ClipboardGrabber
{
	class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}

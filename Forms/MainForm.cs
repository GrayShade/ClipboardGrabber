using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace ClipboardGrabber
{
	public partial class MainForm : Form
	{
		private string baseUrl;
		[Import]
		private Server server { get; set; }
		[Import]
		private IConfigurationManager configurationManager { get; set; }

		public MainForm()
		{
			InitializeComponent();

			new CompositionContainer(new AssemblyCatalog(Assembly.GetExecutingAssembly())).ComposeParts(this);

			configurationManager.FavoriteIcon = NativeMethods.GetApplicationIcon() ?? Icon;
			Icon = configurationManager.FavoriteIcon;

			server.Setup();
			server.Request += new EventHandler<RequestEventArgs>(server_Request);
			server.Error += new EventHandler<ErrorEventArgs>(server_Error);
		}

		private void UpdateAddress(string path)
		{
			ClipboardProtect(() =>
			{
				txtAddress.Text = baseUrl + path;
				Clipboard.SetText(txtAddress.Text);
			});
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			foreach (MenuItem item in mnuLanguage.MenuItems)
				if (item.Tag as string == configurationManager.Language)
				{
					item.PerformClick();
					break;
				}

			btnUpdate.PerformClick();
			GetAddress();
			server.Start();
		}

		private void GetAddress()
		{
			MenuItem menuItem;
			foreach (var address in PortMapper.AddMapping(configurationManager.Port, configurationManager.Port, "TCP", "ClipboardGrabber"))
			{
				menuItem = new MenuItem(address, address_Click) { RadioCheck = true };
				mnuAddress.MenuItems.Add(menuItem);

				if (baseUrl == null)
					menuItem.PerformClick();
			}
			if (mnuAddress.MenuItems.Count > 0)
				mnuAddress.MenuItems[0].PerformClick();

			mnuAddress.MenuItems.Add(new MenuItem("-"));
			if (configurationManager.Address != null)
			{
				menuItem = new MenuItem(configurationManager.Address, address_Click) { RadioCheck = true };
				mnuAddress.MenuItems.Add(menuItem);
				menuItem.PerformClick();
			}

			menuItem = new MenuItem("Custom...", customIp_Click) { RadioCheck = true };
			mnuAddress.MenuItems.Add(menuItem);
		}

		private void SetAddress(string address)
		{
			baseUrl = "http://" + address;
			if (configurationManager.Port != 80)
				baseUrl += ':' + configurationManager.Port.ToString();
			baseUrl += '/';

			lstHistory_SelectedIndexChanged(lstHistory, null);
		}

		private void customIp_Click(object sender, EventArgs e)
		{
			var menuItem = sender as MenuItem;

			using (var customAddressForm = new CustomAddressForm())
			{
				if (customAddressForm.ShowDialog() == DialogResult.Cancel)
					return;

				if (!menuItem.Checked)
				{
					foreach (MenuItem item in mnuAddress.MenuItems)
						item.Checked = false;
					menuItem.Checked = true;

					configurationManager.Address = customAddressForm.Address;
					SetAddress(customAddressForm.Address);
				}
			}
		}

		private void address_Click(object sender, EventArgs e)
		{
			var menuItem = sender as MenuItem;
			if (menuItem.Checked)
				return;

			foreach (MenuItem item in mnuAddress.MenuItems)
				item.Checked = false;
			menuItem.Checked = true;

			SetAddress(menuItem.Text);
		}

		private void server_Request(object sender, RequestEventArgs e)
		{
			var request = e.Request;
			var msg = String.Format("Request from {0} - {1}: {2}{3}", request.RemoteEndPoint, request.UserAgent ?? String.Empty, request.Address, Environment.NewLine);
			txtLog.BeginInvoke(new Action(() => txtLog.AppendText(msg)));
		}

		private void server_Error(object sender, ErrorEventArgs e)
		{
			txtLog.BeginInvoke(new Action(() => txtLog.AppendText(e.Exception.ToString() + Environment.NewLine)));
		}

		[Import]
		private IHistoryEntryFormatter<Bitmap> bitmapHistoryEntryFormatter { get; set; }
		[Import]
		private IHistoryEntryFormatter<string> textHistoryEntryFormatter { get; set; }

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			ClipboardProtect(() =>
			{
				var dataObject = Clipboard.GetDataObject();
				IHistoryEntry entry;

				if (dataObject.GetDataPresent(DataFormats.Bitmap))
				{
					entry = bitmapHistoryEntryFormatter.Format(dataObject.GetData(DataFormats.Bitmap) as Bitmap);
				}
				else if (dataObject.GetDataPresent(DataFormats.Text))
				{
					entry = textHistoryEntryFormatter.Format(Clipboard.GetData(DataFormats.Text) as string);
				}
				else
				{
					txtLog.AppendText("Unrecognized clipboard format" + Environment.NewLine);
					return;
				}

				server.AddHistoryEntry(entry);
				lstHistory.SelectedIndex = lstHistory.Items.Add(entry);
			});
		}

		private void ClipboardProtect(Action action)
		{
			try
			{
				action();
			}
			catch (ExternalException exception)
			{
				txtLog.AppendText(String.Format("An error has occurred: {0}{1}", exception.Message, Environment.NewLine));
				var title = NativeMethods.GetWindowText(NativeMethods.GetOpenClipboardWindow());
				if (!String.IsNullOrEmpty(title))
					txtLog.AppendText(String.Format("The clipboard might be held open by a window named: {0}{1}", title, Environment.NewLine));
			}
		}

		private void ApplicationExceptionHandler(Exception e)
		{
		}

		private void exit_Click(object sender, EventArgs e)
		{
			Close();
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

            //PortMapper.RemoveMapping(configurationManager.Port, "TCP");
		}

		private void language_Click(object sender, EventArgs e)
		{
			var menuItem = sender as MenuItem;
			configurationManager.Language = menuItem.Tag as string;

			foreach (MenuItem item in mnuLanguage.MenuItems)
				item.Checked = item == menuItem;
		}

		private void lstHistory_SelectedIndexChanged(object sender, EventArgs e)
		{
			var entry = lstHistory.SelectedItem as IHistoryEntry;
			if (entry != null)
			{
				UpdateAddress(server.GetEntryPath(entry));

				var htmlEntry = entry as HtmlHistoryEntry;
				if (htmlEntry != null)
					mixedViewer.SetText(htmlEntry.FriendlyText);
				else
					mixedViewer.SetImage((entry as ImageHistoryEntry).Bitmap);
			}
		}
	}

	interface IHistoryEntryFormatter<T>
	{
		IHistoryEntry Format(T data);
	}

	[Export(typeof(IHistoryEntryFormatter<Bitmap>))]
	class ImageHistoryEntryFormatter : IHistoryEntryFormatter<Bitmap>
	{
		IHistoryEntry IHistoryEntryFormatter<Bitmap>.Format(Bitmap data)
		{
			return new ImageHistoryEntry(data);
		}
	}

	[Export(typeof(IHistoryEntryFormatter<string>))]
	class TextHistoryEntryFormatter : IHistoryEntryFormatter<string>
	{
		[Import]
		private IConfigurationManager configurationManager { get; set; }

		IHistoryEntry IHistoryEntryFormatter<string>.Format(string data)
		{
			var text = data.Replace("]]>", @"]]]]><![CDATA[>");
			text =
@"<html>
	<head>
		<script type=""text/javascript"" src=""http://alexgorbatchev.com/pub/sh/current/scripts/shCore.js""></script>
		<script type=""text/javascript"" src=""http://alexgorbatchev.com/pub/sh/current/scripts/shBrush" + configurationManager.Language + @".js""></script>
		<script type=""text/javascript"">SyntaxHighlighter.all();</script>
		<link type=""text/css"" rel=""stylesheet"" href=""http://alexgorbatchev.com/pub/sh/current/styles/shCore.css"" />
		<link type=""text/css"" rel=""stylesheet"" href=""http://alexgorbatchev.com/pub/sh/current/styles/shThemeDefault.css"" />
	</head>
	<body>
		<script type=""syntaxhighlighter"" class=""brush: " + configurationManager.Language.ToLower() + @";""><![CDATA[" + text + @"]]></script>
	</body>
</html>";
			return new HtmlHistoryEntry(data, text);
		}
	}
}

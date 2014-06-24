using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClipboardGrabber
{
	public class MixedViewer : Panel
	{
		private PictureBox pictureBox;
		private TextBox textBox;

		public MixedViewer()
		{
			pictureBox = new PictureBox()
			{
				Dock = DockStyle.Fill,
				SizeMode = PictureBoxSizeMode.Zoom
			};
			textBox = new TextBox()
			{
				BackColor = Color.White,
				Dock = DockStyle.Fill,
				Multiline = true,
				ReadOnly = true,
				ScrollBars = ScrollBars.Both
			};

			Controls.Add(pictureBox);
			Controls.Add(textBox);
		}

		public void SetImage(Image image)
		{
			pictureBox.Image = image;
			pictureBox.BringToFront();
			textBox.Text = String.Empty;
		}

		public void SetText(string text)
		{
			textBox.Text = text;
			textBox.BringToFront();
			pictureBox.Image = null;
		}
	}
}

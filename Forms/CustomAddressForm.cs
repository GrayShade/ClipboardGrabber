using System.Windows.Forms;

namespace ClipboardGrabber
{
	public partial class CustomAddressForm : Form
	{
		public string Address
		{
			get
			{
				return txtAddress.Text;
			}
		}

		public CustomAddressForm()
		{
			InitializeComponent();
		}
	}
}

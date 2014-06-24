namespace ClipboardGrabber
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Panel panel1;
			System.Windows.Forms.MainMenu mainMenu;
			System.Windows.Forms.MenuItem menuItem1;
			System.Windows.Forms.MenuItem menuItem3;
			System.Windows.Forms.MenuItem menuItem8;
			System.Windows.Forms.MenuItem menuItem7;
			System.Windows.Forms.MenuItem menuItem6;
			System.Windows.Forms.MenuItem menuItem9;
			System.Windows.Forms.MenuItem menuItem10;
			System.Windows.Forms.MenuItem menuItem11;
			System.Windows.Forms.MenuItem menuItem12;
			System.Windows.Forms.MenuItem menuItem13;
			System.Windows.Forms.MenuItem menuItem14;
			System.Windows.Forms.MenuItem menuItem15;
			System.Windows.Forms.MenuItem menuItem16;
			System.Windows.Forms.MenuItem menuItem17;
			System.Windows.Forms.MenuItem menuItem2;
			System.Windows.Forms.SplitContainer splitContainer1;
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.mnuLanguage = new System.Windows.Forms.MenuItem();
			this.mnuAddress = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.mixedViewer = new ClipboardGrabber.MixedViewer();
			this.lstHistory = new System.Windows.Forms.ListBox();
			panel1 = new System.Windows.Forms.Panel();
			mainMenu = new System.Windows.Forms.MainMenu(this.components);
			menuItem1 = new System.Windows.Forms.MenuItem();
			menuItem3 = new System.Windows.Forms.MenuItem();
			menuItem8 = new System.Windows.Forms.MenuItem();
			menuItem7 = new System.Windows.Forms.MenuItem();
			menuItem6 = new System.Windows.Forms.MenuItem();
			menuItem9 = new System.Windows.Forms.MenuItem();
			menuItem10 = new System.Windows.Forms.MenuItem();
			menuItem11 = new System.Windows.Forms.MenuItem();
			menuItem12 = new System.Windows.Forms.MenuItem();
			menuItem13 = new System.Windows.Forms.MenuItem();
			menuItem14 = new System.Windows.Forms.MenuItem();
			menuItem15 = new System.Windows.Forms.MenuItem();
			menuItem16 = new System.Windows.Forms.MenuItem();
			menuItem17 = new System.Windows.Forms.MenuItem();
			menuItem2 = new System.Windows.Forms.MenuItem();
			splitContainer1 = new System.Windows.Forms.SplitContainer();
			panel1.SuspendLayout();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			panel1.Controls.Add(this.txtAddress);
			panel1.Controls.Add(this.btnUpdate);
			panel1.Dock = System.Windows.Forms.DockStyle.Top;
			panel1.Location = new System.Drawing.Point(120, 0);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(469, 41);
			panel1.TabIndex = 0;
			// 
			// txtAddress
			// 
			this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAddress.Location = new System.Drawing.Point(12, 14);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.ReadOnly = true;
			this.txtAddress.Size = new System.Drawing.Size(364, 20);
			this.txtAddress.TabIndex = 2;
			// 
			// btnUpdate
			// 
			this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpdate.Location = new System.Drawing.Point(382, 12);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(75, 23);
			this.btnUpdate.TabIndex = 0;
			this.btnUpdate.Text = "&Update";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// mainMenu
			// 
			mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            menuItem1,
            menuItem3});
			// 
			// menuItem1
			// 
			menuItem1.Index = 0;
			menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExit});
			menuItem1.Text = "&File";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 0;
			this.mnuExit.Text = "E&xit";
			this.mnuExit.Click += new System.EventHandler(this.exit_Click);
			// 
			// menuItem3
			// 
			menuItem3.Index = 1;
			menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuLanguage,
            menuItem2,
            this.mnuAddress,
            this.menuItem4});
			menuItem3.Text = "&Options";
			// 
			// mnuLanguage
			// 
			this.mnuLanguage.Index = 0;
			this.mnuLanguage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            menuItem8,
            menuItem7,
            menuItem6,
            menuItem9,
            menuItem10,
            menuItem11,
            menuItem12,
            menuItem13,
            menuItem14,
            menuItem15,
            menuItem16,
            menuItem17});
			this.mnuLanguage.Text = "&Language";
			// 
			// menuItem8
			// 
			menuItem8.Index = 0;
			menuItem8.Tag = "Bash";
			menuItem8.Text = "Bash";
			menuItem8.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem7
			// 
			menuItem7.Index = 1;
			menuItem7.Tag = "CSharp";
			menuItem7.Text = "C#";
			menuItem7.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem6
			// 
			menuItem6.Index = 2;
			menuItem6.Tag = "Cpp";
			menuItem6.Text = "C++";
			menuItem6.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem9
			// 
			menuItem9.Index = 3;
			menuItem9.Tag = "Css";
			menuItem9.Text = "CSS";
			menuItem9.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem10
			// 
			menuItem10.Index = 4;
			menuItem10.Tag = "Diff";
			menuItem10.Text = "Diff";
			menuItem10.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem11
			// 
			menuItem11.Index = 5;
			menuItem11.Tag = "JScript";
			menuItem11.Text = "JavaScript";
			menuItem11.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem12
			// 
			menuItem12.Index = 6;
			menuItem12.Tag = "Java";
			menuItem12.Text = "Java";
			menuItem12.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem13
			// 
			menuItem13.Index = 7;
			menuItem13.Tag = "Php";
			menuItem13.Text = "PHP";
			menuItem13.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem14
			// 
			menuItem14.Index = 8;
			menuItem14.Tag = "Plain";
			menuItem14.Text = "Plain";
			menuItem14.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem15
			// 
			menuItem15.Index = 9;
			menuItem15.Tag = "Python";
			menuItem15.Text = "Python";
			menuItem15.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem16
			// 
			menuItem16.Index = 10;
			menuItem16.Tag = "Sql";
			menuItem16.Text = "SQL";
			menuItem16.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem17
			// 
			menuItem17.Index = 11;
			menuItem17.Tag = "Xml";
			menuItem17.Text = "XML";
			menuItem17.Click += new System.EventHandler(this.language_Click);
			// 
			// menuItem2
			// 
			menuItem2.Index = 1;
			menuItem2.Text = "-";
			// 
			// mnuAddress
			// 
			this.mnuAddress.Index = 2;
			this.mnuAddress.Text = "&IP address";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "&Port...";
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			splitContainer1.Location = new System.Drawing.Point(120, 41);
			splitContainer1.Name = "splitContainer1";
			splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(this.txtLog);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(this.mixedViewer);
			splitContainer1.Size = new System.Drawing.Size(469, 448);
			splitContainer1.SplitterDistance = 174;
			splitContainer1.TabIndex = 5;
			// 
			// txtLog
			// 
			this.txtLog.BackColor = System.Drawing.Color.White;
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Location = new System.Drawing.Point(0, 0);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLog.Size = new System.Drawing.Size(469, 174);
			this.txtLog.TabIndex = 4;
			this.txtLog.WordWrap = false;
			// 
			// mixedViewer
			// 
			this.mixedViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mixedViewer.Location = new System.Drawing.Point(0, 0);
			this.mixedViewer.Name = "mixedViewer";
			this.mixedViewer.Size = new System.Drawing.Size(469, 270);
			this.mixedViewer.TabIndex = 1;
			// 
			// lstHistory
			// 
			this.lstHistory.Dock = System.Windows.Forms.DockStyle.Left;
			this.lstHistory.FormattingEnabled = true;
			this.lstHistory.IntegralHeight = false;
			this.lstHistory.Location = new System.Drawing.Point(0, 0);
			this.lstHistory.Name = "lstHistory";
			this.lstHistory.Size = new System.Drawing.Size(120, 489);
			this.lstHistory.TabIndex = 3;
			this.lstHistory.SelectedIndexChanged += new System.EventHandler(this.lstHistory_SelectedIndexChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(589, 489);
			this.Controls.Add(splitContainer1);
			this.Controls.Add(panel1);
			this.Controls.Add(this.lstHistory);
			this.Menu = mainMenu;
			this.MinimumSize = new System.Drawing.Size(350, 300);
			this.Name = "MainForm";
			this.Text = "Clipboard Grabber";
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel1.PerformLayout();
			splitContainer1.Panel2.ResumeLayout(false);
			splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.TextBox txtAddress;
		private MixedViewer mixedViewer;
		private System.Windows.Forms.MenuItem mnuAddress;
		private System.Windows.Forms.TextBox txtLog;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuLanguage;
		private System.Windows.Forms.ListBox lstHistory;

	}
}


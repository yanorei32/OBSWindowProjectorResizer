using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

partial class MainForm : Form {
	void initializeComponent() {
		const int
			imgW = 320,
			imgH = 84,
			margin = 13,
			padding = 6;

		int curW = 0, curH = 0;

		Assembly execAsm = Assembly.GetExecutingAssembly();

		/*\
		|*| UI Init
		\*/
		this.logo			= new PictureBox();
		this.resolution		= new ComboBox();
		this.apply			= new Button();
		this.refresh		= new Button();
		this.saveShortcut	= new Button();
		this.currentStatus	= new Label();

		this.SuspendLayout();
		curH = curW = margin;

		/*\
		|*| Logo column
		\*/
		this.logo.Location			= new Point(curH, curW);
		this.logo.Size				= new Size(imgW, imgH);
		// this.logo.BackgroundImage	= new Bitmap(
		// 	execAsm.GetManifestResourceStream("logo")
		// );
        //
		curH += this.logo.Size.Height;
		curH += padding;

		/*\
		|*| Refresh button column
		\*/
		this.refresh.Text			= "Refresh (&R)";
		this.refresh.Size			= new Size(75, 23);
		this.refresh.Location		= new Point(curW, curH);
		this.refresh.Click			+= new EventHandler(refreshClick);
		this.refresh.UseMnemonic	= true;

		curW = margin;
		curH += this.refresh.Size.Height;
		curH += padding;

		/*\
		|*| CurrentStatus column
		\*/
		this.currentStatus.Text		= "CurrentStatus";
		this.currentStatus.AutoSize	= false;
		this.currentStatus.Location	= new Point(curW, curH);
		this.currentStatus.Size		= new Size(imgW, 22);
		this.currentStatus.Font		= new Font("Consolas", 16F);

		curH += this.currentStatus.Size.Height;
		curH += padding;

		/*\
		|*| Resolution
		\*/
		this.resolution.AutoSize	= false;
		this.resolution.Location	= new Point(curW, curH);
		this.resolution.Size		= new Size(imgW, 40);
		this.resolution.Font		= new Font("Consolas", 16F);

		this.resolution.Items.AddRange(new object[] {
			"854x480",
			"1280x720",
			"1600x900",
			"1920x1080",
			"2560x1440"
		});
		this.resolution.SelectedItem	= "1920x1080";

		curH += 40;
		// curH += padding;

		/*\
		|*| Apply
		\*/
		this.apply.Text			= "Apply (&A)";
		this.apply.Size			= new Size(100, 23);
		this.apply.Location		= new Point(curW, curH);
		this.apply.Click		+= new EventHandler(applyClick);
		this.apply.UseMnemonic	= true;

		curW += this.apply.Size.Width;
		
		this.saveShortcut.Text			= "Save Shortcut (&S)";
		this.saveShortcut.Size			= new Size(100, 23);
		this.saveShortcut.Location		= new Point(curW, curH);
		this.saveShortcut.Click			+= new EventHandler(saveShortcutClick);
		this.saveShortcut.UseMnemonic	= true;

		curH += this.saveShortcut.Size.Height;


		/*\
		|*| Form
		\*/
		this.Text				= Program.APPLICATION_NAME;
		this.ClientSize			= new Size(imgW + (margin ), curH);
		this.MinimumSize		= this.Size;
		this.MaximumSize		= this.Size;
		this.FormBorderStyle	= FormBorderStyle.FixedSingle;
		// this.Icon				= new Icon(execAsm.GetManifestResourceStream("icon"));

		this.Controls.Add(this.logo);
		this.Controls.Add(this.resolution);
		this.Controls.Add(this.apply);
		this.Controls.Add(this.saveShortcut);
		this.Controls.Add(this.refresh);
		this.Controls.Add(this.currentStatus);

		this.ResumeLayout(false);
	}
}


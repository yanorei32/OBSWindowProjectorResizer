using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

partial class MainForm : Form {
	// UI Elements
	PictureBox	logo;

	Button		apply,
				refresh;

	ComboBox	resolution;

	Label		currentStatus;


	VRViewWindow vvw;

	void updateUI() {
		this.apply.Enabled = false;

		if (vvw.HWnd == IntPtr.Zero) {
			this.currentStatus.Text = "VRView Not found";
			return;
		}

		var size = vvw.GetResolution();

		if (size.IsEmpty) {
			this.currentStatus.Text = "VRView Not available";
			return;
		}

		this.currentStatus.Text = string.Format(
			"Status: {0}x{1}",
			size.Width,
			size.Height
		);

		this.apply.Enabled = true;
	}

	void applyClick(object sender, EventArgs e) {
		var regex = new Regex(@"^\d+x\d+$");

		if (!regex.Match(resolution.Text).Success) {
			MessageBox.Show(
				string.Format(
					"{0} is invalid",
					resolution.Text
				),
				this.Text,
				MessageBoxButtons.OK,
				MessageBoxIcon.Asterisk
			);
			return;
		}

		var splitted = resolution.Text.Split(new Char[] {'x'});
		var size = new Size(int.Parse(splitted[0]), int.Parse(splitted[1]));

		if (size.IsEmpty) {
			MessageBox.Show(
				"Resolution is not valid (empty)",
				this.Text,
				MessageBoxButtons.OK,
				MessageBoxIcon.Asterisk
			);
			return;
		}

		vvw.SetResolution(this.Handle, size);
		updateUI();
	}

	void refreshClick(object sender, EventArgs e) {
		vvw.ReloadWindow();
		updateUI();
	}

	public MainForm() {
		initializeComponent();

		vvw = new VRViewWindow();
		vvw.ReloadWindow();
		updateUI();
	}
}


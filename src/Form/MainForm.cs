using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

partial class MainForm : Form {
	// UI Elements
	PictureBox	logo;

	Button		apply,
				refresh,
				saveShortcut;

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

		this.saveShortcut.Enabled = this.apply.Enabled = true;
	}

	bool getSize(out Size s) {
		try {
			s = Util.Str2Size(resolution.Text);

		} catch {
			MessageBox.Show(
				string.Format("{0} is not valid", resolution.Text),
				Program.APPLICATION_NAME,
				MessageBoxButtons.OK,
				MessageBoxIcon.Hand
			);

			s = new Size(0,0);
			return false;
		}

		if (s.IsEmpty) {
			MessageBox.Show(
				"Resolution is not valid (empty)",
				Program.APPLICATION_NAME,
				MessageBoxButtons.OK,
				MessageBoxIcon.Asterisk
			);

			return false;
		}

		return true;
	}

	void applyClick(object sender, EventArgs e) {
		Size s;
		if (!getSize(out s)) return;

		vvw.SetResolution(this.Handle, s);
		updateUI();
	}

	static string getExeDir() {
		return Path.GetDirectoryName(
			Application.ExecutablePath
		);
	}

	void saveShortcutClick(object sender, EventArgs e) {
		Size s;
		if (!getSize(out s)) return;

		var s_str = Util.Size2Str(s);

		var t = Type.GetTypeFromCLSID(
			new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")
		);

		dynamic shell = Activator.CreateInstance(t);

		var shortcut = shell.CreateShortcut(
			Path.Combine(
				getExeDir(),
				string.Format(
					"{1} - {0}.lnk",
					s_str,
					Path.GetFileNameWithoutExtension(Application.ExecutablePath)
				)
			)
		);

		shortcut.IconLocation = Application.ExecutablePath + ",1";
		shortcut.TargetPath = Application.ExecutablePath;
		shortcut.Arguments = "\"" + s_str + "\"";
		shortcut.Save();

		Marshal.FinalReleaseComObject(shortcut);
		Marshal.FinalReleaseComObject(shell);
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


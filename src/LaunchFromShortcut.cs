using System;
using System.Drawing;
using System.Windows.Forms;

static class LaunchFromShortcut {
	public static void Run(string arg) {
		Size s;

		try {
			s = Util.Str2Size(arg);

		} catch {
			MessageBox.Show(
				string.Format("Illegal argument: {0}", arg),
				Program.APPLICATION_NAME,
				MessageBoxButtons.OK,
				MessageBoxIcon.Hand
			);

			return;
		}

		var wpw = new WindowProjectorWindow();

		if (wpw.HWnd == IntPtr.Zero) {
			MessageBox.Show(
				"VRView window not found",
				Program.APPLICATION_NAME,
				MessageBoxButtons.OK,
				MessageBoxIcon.Hand
			);

			return;
		}

		if (wpw.GetResolution().IsEmpty) {
			MessageBox.Show(
				"WindowProjector window is not available",
				Program.APPLICATION_NAME,
				MessageBoxButtons.OK,
				MessageBoxIcon.Hand
			);

			return;
		}

		if (s.IsEmpty) {
			MessageBox.Show(
				"Resolution is not valid (empty)",
				Program.APPLICATION_NAME,
				MessageBoxButtons.OK,
				MessageBoxIcon.Asterisk
			);

			return;
		}

		wpw.SetResolution((IntPtr)0, s);
	}
}


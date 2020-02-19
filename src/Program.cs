using System;
using System.Windows.Forms;

class Program {
	public const string APPLICATION_NAME = "SteamVR VRView Resizer";

	static void Main(string[] Args) {
		switch (Args.Length) {
			case 0:
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
				break;
			
			case 1:
				LaunchFromShortcut.Run(Args[0]);
				break;

			default:
				MessageBox.Show(
					string.Format("Illegal argument count: {0}", Args.Length),
					Program.APPLICATION_NAME,
					MessageBoxButtons.OK,
					MessageBoxIcon.Hand
				);
				break;
		}
	}
}


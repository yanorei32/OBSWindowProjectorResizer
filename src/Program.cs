using System;
using System.Drawing;
using System.Windows.Forms;

class Program {
	static void Main() {
		var vvw = new VRViewWindow();
		vvw.SetResolution((IntPtr)(0), new Size(640, 480));
		Console.WriteLine(vvw.GetResolution());
	}
}


using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Text;

class VRViewWindow {
	// Clinet RECT - OBS Gamecapture's value
	Size CLIENT_MARGIN = new Size(
		958 - 958,
		488 - 455
	);

	IntPtr hWnd;

	public IntPtr HWnd {
		get { return this.hWnd; }
	}

	public bool IsVisible {
		get {
			return this.hWnd == IntPtr.Zero ?
				false : IsWindowVisible(this.hWnd);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	struct RECT {
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;
	}

	[StructLayout(LayoutKind.Sequential)]
	struct WINDOWINFO {
		public int cbSize;
		public RECT rcWindow;
		public RECT rcClient;
		public int dwStyle;
		public int dwExStyle;
		public int dwWindowStatus;
		public uint cxWindowBorders;
		public uint cyWindowBorders;
		public short atomWindowType;
		public short wCreatorVersion;
	}

	const int SW_RESTORE = 9;
	delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

	[DllImport("user32.dll", SetLastError=true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool EnumWindows(
		EnumWindowsDelegate lpEnumFunc,
		IntPtr lparam
	);

	[DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
	static extern int GetWindowText(
		IntPtr hWnd,
		StringBuilder lpString,
		int nMaxCount
	);

	[DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)]
	static extern int GetWindowTextLength(
		IntPtr hWnd
	);

	[DllImport("user32.dll", SetLastError=true)]
	static extern int GetWindowThreadProcessId(
		IntPtr hWnd,
		out int lpdwProcessId
	);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool IsWindowVisible(
		IntPtr hWnd
	);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool ShowWindow(
		IntPtr hWnd,
		int nCmdShow
	);

	[DllImport("user32.dll", SetLastError=true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool GetWindowRect(
		IntPtr hWnd,
		out RECT rect
	);

	[DllImport("user32.dll", SetLastError=true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool GetClientRect(
		IntPtr hWnd,
		out RECT rect
	);

	[DllImport("user32.dll", SetLastError=true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool SetWindowPos(
		IntPtr hWnd,
		IntPtr hWndInsertAfter,
		int x,
		int y,
		int cx,
		int cy,
		uint flags
	);

	[DllImport("user32.dll", SetLastError=true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool AdjustWindowRectEx(
		ref RECT lpRect,
		int dwStyle,
		bool bMenu,
		int dwExStyle
	);

	[DllImport("user32.dll", SetLastError=true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool GetWindowInfo(
		IntPtr hWnd,
		out WINDOWINFO pwi
	);

	string DebugRectToString(RECT rect) {
		return string.Format(
			"Left: {0}, Top: {1}, Right: {2}, Bottom: {3}",
			rect.Left,
			rect.Top,
			rect.Right,
			rect.Bottom
		);
	}

	public Size GetResolution() {
		RECT clientRect;

		if (!GetClientRect(hWnd, out clientRect))
			return new Size(0, 0);

		return new Size(
			clientRect.Right	- CLIENT_MARGIN.Width,
			clientRect.Bottom	- CLIENT_MARGIN.Height
		);
	}

	public bool SetResolution(IntPtr hWndInsertAfter, Size resolution) {
		if (!IsVisible)
			return true;

		ShowWindow(hWnd, SW_RESTORE);

		RECT newClientRect;

		RECT windowRect, clientRect;
		if (!GetWindowRect(hWnd, out windowRect))
			return true;

		if (!GetClientRect(hWnd, out clientRect))
			return true;

		WINDOWINFO wi;
		if (!GetWindowInfo(hWnd, out wi))
			return true;

		newClientRect.Top		= windowRect.Top;
		newClientRect.Left		= windowRect.Left;
		newClientRect.Right		= resolution.Width	+ CLIENT_MARGIN.Width;
		newClientRect.Bottom	= resolution.Height	+ CLIENT_MARGIN.Height;
		
		AdjustWindowRectEx(
			ref newClientRect,
			wi.dwStyle,
			false,
			wi.dwExStyle
		);

		newClientRect.Right		+= windowRect.Left	- newClientRect.Left;
		newClientRect.Bottom	+= windowRect.Top	- newClientRect.Top;
		newClientRect.Top		= windowRect.Top;
		newClientRect.Left		= windowRect.Left;

		SetWindowPos(
			hWnd,
			hWndInsertAfter,
			newClientRect.Left,
			newClientRect.Top,
			newClientRect.Right,
			newClientRect.Bottom,
			0
		);

		return false;
	}

	bool EnumWindowCallback(IntPtr hWnd, IntPtr lparam) {
		var textLen = GetWindowTextLength(hWnd);

		if (textLen == 0)
			return true;

		var titleSB = new StringBuilder(textLen + 1);
		GetWindowText(hWnd, titleSB, titleSB.Capacity);

		var titleStr = titleSB.ToString();

		Console.WriteLine(titleStr);
		if (titleStr != "VR View")
			return true;

		int pid;
		GetWindowThreadProcessId(hWnd, out pid);

		if (Process.GetProcessById(pid).ProcessName != "vrmonitor")
			return true;

		this.hWnd = hWnd;
		
		return false;
	}

	public void ReloadWindow() {
		this.hWnd = IntPtr.Zero;
		EnumWindows(new EnumWindowsDelegate(EnumWindowCallback), IntPtr.Zero);
	}

	public VRViewWindow() {
		ReloadWindow();
	}
}


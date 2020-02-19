using System;
using System.Drawing;
using System.Text.RegularExpressions;

static class Util {
	public static string Size2Str(Size size) {
		return string.Format(
			"{0}x{1}",
			size.Width,
			size.Height
		);
	}

	public static Size Str2Size(string str) {
		var regex = new Regex(@"^\d+x\d+$");

		if (!regex.Match(str).Success) {
			throw new FormatException("invalid format");
		}

		var splitted = str.Split(new Char[] {'x'});

		return new Size(
			int.Parse(splitted[0]),
			int.Parse(splitted[1])
		);
	}
}


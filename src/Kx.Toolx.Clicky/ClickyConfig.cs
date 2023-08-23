using System.Drawing;

namespace Kx.Toolx.Clicky;

public class ClickyConfig
{
	public readonly static string KEY = "Clicky";

	public string Hwnd { get; set; }
	public List<Point> Points { get; set; }
}

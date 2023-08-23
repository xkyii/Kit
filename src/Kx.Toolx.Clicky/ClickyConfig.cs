using System.Drawing;
using System.Text;

namespace Kx.Toolx.Clicky;

public class ClickyConfig
{
	public readonly static string KEY = "Clicky";

	/// <summary>
	/// 发送消息的目标窗口句柄
	/// </summary>
	public string Hwnd { get; set; } = string.Empty;

	/// <summary>
	/// 随机点列表
	/// </summary>
	public List<Point> Points { get; set; } = new List<Point>();

	/// <summary>
	/// 定时器时间间隔 (秒)
	/// </summary>
	public double TimerPeriod { get; set; } = 5;

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.AppendLine($"\nKEY: {KEY}");
		sb.AppendLine($"窗口句柄(Hwnd): {Hwnd}");
		sb.AppendLine($"时间间隔(TimerPeriod): {TimerPeriod} (秒)");
		sb.Append($"随机点列表(Points): {{ ");
		foreach ( var p in Points )
		{
			sb.Append($"[{p.X}, {p.Y}], ");
		}
		sb.Append($"}}");

		return sb.ToString();
	}
}

namespace Kx.Codex.Console.Text;

public class CommonModel
{
	public static readonly CommonModel Empty = new CommonModel();

	public string Author { get; set; }

	public string Version { get; set; }
}

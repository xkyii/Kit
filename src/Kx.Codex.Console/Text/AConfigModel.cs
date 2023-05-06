using System.Collections.Generic;

namespace Kx.Codex.Console.Text;

public abstract class AConfigModel
{
	public Dictionary<string, Dictionary<string, string>> Configs { get; set; }
}

using System.Collections.Generic;

namespace Kx.Codex.Console.Text;

public abstract class AConfigModel
{
	/// <summary>
	/// 模型列表
	/// </summary>
	public Dictionary<string, Dictionary<string, string>> Models { get; set; }

	/// <summary>
	/// 通用配置
	/// </summary>
	public CommonModel Common { get; set; }
}

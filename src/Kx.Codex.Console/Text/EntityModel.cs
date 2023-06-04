using System.Collections.Generic;

namespace Kx.Codex.Console.Text;

/// <summary>
/// Entity 模型
/// </summary>
public class EntityModel : AConfigModel
{
	/// <summary>
	/// 数据库Table的原始信息
	/// </summary>
	public Db.Table Table { get; set; }

    /// <summary>
    /// 数据库Table的原始字段列表信息
    /// </summary>
    public List<Db.Column> Columns { get; set; }
}

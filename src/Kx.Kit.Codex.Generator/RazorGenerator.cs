using RazorEngineCore;

namespace Kx.Kit.Codex.Generator;

public class RazorGenerator
{
    private readonly RazorEngine _engine = new();

    public dynamic Gen(dynamic model, string templateContent)
    {
        // 编译模板
        var template = _engine.Compile(templateContent);
        // 渲染
        var result = template.Run(model);
        return result;
    }

    public void Gen(dynamic model, string templateFile, string targetFile)
    {
        // 读取模板内容
        var templateContent = File.ReadAllText(templateFile);
        // 生成
        var result = Gen(model, templateContent);
        File.WriteAllText(targetFile, result);
    }
}

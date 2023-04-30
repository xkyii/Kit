using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.Razor;

namespace Kx.Codex.Console;

public class CodexTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(
            new TemplateDefinition("Entity")
                .WithRazorEngine()
                .WithVirtualFilePath(
                    "/Text/Entity.cshtml",
                    isInlineLocalized: true
                )
        );
    }
}

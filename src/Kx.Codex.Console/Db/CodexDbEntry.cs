using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kx.Codex.Console.Db;

public static class CodexDbEntry
{
    public static void AddCodexDb(this IServiceCollection services)
    {
        services.AddDbContext<CodexDbContext>((sp, options) =>
        {
            options.UseMySQL("server=localhost;port=3306;database=yf_data;user=yfty_admin;password=Yfty!23456");
        });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kx.Toolx.Codex.Source.MySql;

public static class MySqlDbEntry
{
    public static void AddCodexMySql(this IServiceCollection services)
    {
        services.AddDbContext<CodexDbContext>((sp, options) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            //string url = configuration["DataSource:Url"]!;
            string url = "server=localhost;port=3306;database=tyr;user=tyr_admin;password=Tyr!23456";
            options.UseMySQL(url);
        });
    }
}

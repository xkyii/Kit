using Microsoft.EntityFrameworkCore;

namespace Kx.Codex.Db;


/// <summary>
/// 注入使用DbContextFactory时需要
/// </summary>
internal class CodexDbContextFactory : IDbContextFactory<CodexDbContext>
{
    public CodexDbContext CreateDbContext()
    {
        //System.Console.WriteLine($"MinterDbContextFactory.CreateDbContext: {Pathx.RuntimeDirectory()}");

        var builder = new DbContextOptionsBuilder<CodexDbContext>();
        builder.UseMySQL("server=localhost;port=3306;database=yf_data;user=yfty_admin;password=Yfty!23456");

        return new CodexDbContext(builder.Options);
    }
}

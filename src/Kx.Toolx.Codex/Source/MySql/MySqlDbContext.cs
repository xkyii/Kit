using Microsoft.EntityFrameworkCore;

namespace Kx.Toolx.Codex.Source.MySql;


public class MySqlDbContext : DbContext
{
    /// <summary>
    /// 原始表信息
    /// </summary>
    public DbSet<Table> Tables { get; set; }

    /// <summary>
    /// 原始字段信息
    /// </summary>
    public DbSet<Column> Columns { get; set; }


    /// <summary>
    /// 迁移需要
    /// </summary>
    public MySqlDbContext()
    {
    }

    /// <summary>
    /// 为注入提供配置入口
    /// </summary>
    /// <param name="options"></param>
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.DefaultTypeMapping<Table>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table>(builder =>
        {
            builder.ToSqlQuery("select * from information_schema.tables where TABLE_SCHEMA=(select database())")
                .HasKey(c => new { c.TableCatalog, c.TableSchema, c.TableName, c.TableType });
        });

        modelBuilder.Entity<Column>(builder =>
        {
            builder.ToSqlQuery("select * from information_schema.columns where TABLE_SCHEMA=(select database())")
                .HasKey(c => new { c.TableCatalog, c.TableSchema, c.TableName, c.ColumnName });
        });

        base.OnModelCreating(modelBuilder);
    }
}

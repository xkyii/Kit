using System.Linq;
using System.Threading.Tasks;
using Kx.Codex.Db;
using Kx.Codex.Db.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Kx.Codex;

public class HelloWorldService : ITransientDependency
{
    public ILogger<HelloWorldService> Logger { get; set; }

    private readonly CodexDbContext _dbContext;

    public HelloWorldService(CodexDbContext dbContext)
    {
        Logger = NullLogger<HelloWorldService>.Instance;
        _dbContext = dbContext;
    }

    public Task SayHelloAsync()
    {
        Logger.LogInformation("Hello World!");

        //string sql = $"SELECT table_name FROM information_schema.tables WHERE table_schema = DATABASE();";
        //var tables = _dbContext.Database.SqlQuery<string>("").ToList();

        //string sql = $"SELECT [table_name] FROM [information_schema].[tables] WHERE [table_schema] = DATABASE();";
        //var ids = _dbContext.Database.SqlQuery<int>($"SELECT [table_name] FROM information_schema.tables WHERE table_schema = DATABASE();").ToList();

        //var tables = _dbContext.Tables.ToList();

        using var connection = _dbContext.Database.GetDbConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "select * from information_schema.tables where TABLE_SCHEMA=(select database());";
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var TableCatalog = reader.GetValueOrDefault<string>("TABLE_CATALOG");
                Logger.LogInformation($"TableCatalog: {TableCatalog}");
                var TableSchema = reader.GetValueOrDefault<string>("TABLE_SCHEMA");
                Logger.LogInformation($"TableSchema: {TableSchema}");
                var TableName = reader.GetValueOrDefault<string>("TABLE_NAME");
                Logger.LogInformation($"TableName: {TableName}");
                var TableType = reader.GetValueOrDefault<string>("TABLE_TYPE");
                Logger.LogInformation($"TableType: {TableType}");
            }
        }

        // ef8 就可以用了
        //var tables = _dbContext.Database
        //    .SqlQuery<string>($"select TABLE_NAME as Name,TABLE_COMMENT as Description from information_schema.tables where TABLE_SCHEMA=(select database());")
        //    .ToList();

        return Task.CompletedTask;
    }
}

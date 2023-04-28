using System.Linq;
using System.Threading.Tasks;
using Kx.Codex.Db;
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

        var types = _dbContext.Model.GetEntityTypes();

        return Task.CompletedTask;
    }
}

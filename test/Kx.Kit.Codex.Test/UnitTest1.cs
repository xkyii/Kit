using Kx.Toolx.Codex.Source.MySql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;


namespace Kx.Toolx.Codex.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var services = new ServiceCollection();

            services.AddDbContext<MySqlDbContext>((sp, options) =>
            {
                string url = "server=localhost;port=3306;database=codex;user=xkyii;password=xk!23456";
                options.UseMySQL(url);
            });

            var sp = services.BuildServiceProvider();
            var db = sp.GetService<MySqlDbContext>();

            Assert.True(db != null);
            Assert.True(db.Tables != null);
            Assert.Equal(1, db.Tables.Count());

            var table = db.Tables.First();
            Assert.True(table != null);
            Assert.Equal("t_demo", table.TableName);
        }
    }
}

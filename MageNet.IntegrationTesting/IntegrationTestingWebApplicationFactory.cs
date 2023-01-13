using System;
using System.Linq;
using System.Threading.Tasks;
using MageNet.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MageNet.IntegrationTesting;

public class IntegrationTestingWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private const string ConnString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;

    public IntegrationTestingWebApplicationFactory()
    {
        _connection = new SqliteConnection(ConnString);
    }

    public T UseDbContext<T>(Func<MageNetDbContext, T> func)
    {
        using (var scope = Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<MageNetDbContext>();
            return func(db);
        }
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<MageNetDbContext>));

            services.Remove(descriptor);


            services.AddDbContext<MageNetDbContext>(options =>
            {
                _connection.Open();
                options.UseSqlite(_connection);
            });
            
            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MageNetDbContext>();
                db.Database.EnsureCreated();
            }
        });
    }

    public override ValueTask DisposeAsync()
    {
        _connection.Close();
        return base.DisposeAsync();
    }
}
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MageNet.IntegrationTesting.Exceptions;
using MageNet.Persistence;
using MageNetServices.Authentication.DTO;
using MageNetServices.Interfaces.Authentication;
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

    public void ConfigureJwtToken(HttpClient httpClient)
    {
        using (var scope = Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var loginHandler = scopedServices.GetRequiredService<ILoginHandler>();

            var authResultTask = loginHandler.TryLogInBackendUser(new LoginData()
            {
                UserName = "Admin",
                Password = "admin"
            });

            // running method synchronously since constructor cannot be async 
            // and this method should be available in factory constructor

            var authResult = authResultTask.Result;

            if (authResult.IsAuthSuccessful)
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", $"{authResult.Token}");
            }

            else
            {
                throw new JwtTokenGenerationException("JWT Token for integration testing cannot be generated!");
            }
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
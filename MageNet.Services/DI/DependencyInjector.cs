using MageNet.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MageNetServices.DI;

public static class DependencyInjector
{
    public static IServiceCollection InjectDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MageNetDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }
}
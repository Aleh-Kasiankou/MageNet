using MageNet.Persistence;
using MageNetServices.AttributeRepository;
using MageNetServices.AttributeValidator;
using MageNetServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MageNetServices.DI;

public static class DependencyHandler
{
    public static IServiceCollection RegisterDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MageNetDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }

    public static IServiceCollection RegisterRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IAttributeRepository, AttributeRepository.AttributeRepository>();
        return services;
    }
    
    public static IServiceCollection RegisterValidationServices(this IServiceCollection services)
    {
        services.AddScoped<IAttributeValidator, AttributeValidator.AttributeValidator>();
        return services;
    }
}
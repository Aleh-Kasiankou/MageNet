using MageNet.Persistence;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository;
using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.AttributeRepository.TypedDataRepositories;
using MageNetServices.Authentication;
using MageNetServices.Authentication.DTO;
using MageNetServices.Interfaces;
using MageNetServices.Interfaces.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MageNetServices.DI;

public static class DependencyHandler
{
    public static IServiceCollection RegisterDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MageNetDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }
    
    public static IServiceCollection RegisterAttributeServices(this IServiceCollection services)
    {
        services.AddScoped<IAttributeRepository, AttributeRepository.AttributeRepository>();
        services.AddScoped<IAttributeTypeFactory, AttributeTypeFactory>();
        services.AddScoped<IAttributeDataRepository<PriceAttributeData>, PriceAttributeDataRepo>();
        services.AddScoped<IAttributeDataRepository<TextAttributeData>, TextAttributeDataRepo>();
        services.AddScoped<IAttributeDataRepository<SelectableAttributeData>, SelectableAttributeDataRepo>();
        return services;
    }

    public static IServiceCollection RegisterDataTransferObjects(this IServiceCollection services)
    {
        services.AddScoped<IPostAttributeWithData, PostAttributeWithData>();
        services.AddScoped<IPostSelectableOption, PostSelectableOption>();
        return services;
    }


    public static IServiceCollection RegisterValidationServices(this IServiceCollection services)
    {
        services.AddScoped<IAttributeValidator, AttributeValidator.AttributeValidator>();
        return services;
    }

    public static IServiceCollection RegisterAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSecurityKey>(configuration.GetSection("JWTSecurityKey"));
        services.AddScoped<ILoginHandler, JwtLoginHandler>();
        services.AddScoped<IDataHasher, DataHasher>();
        services.AddScoped<ILoginData, LoginData>();

        
        return services;
    }

}
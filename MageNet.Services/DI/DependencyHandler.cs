using MageNet.Persistence;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO;
using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.AttributeRepository.DTO.TypedDataRepositories;
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

    public static IServiceCollection RegisterAttributeServices(this IServiceCollection services)
    {
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
}
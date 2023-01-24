using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.Extensions;
using MageNetServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Attribute = MageNetServices.AttributeRepository.DTO.Attributes.Attribute;

namespace MageNetServices.AttributeRepository;

public class AttributeRepository : IAttributeRepository
{
    private readonly MageNetDbContext _dbContext;
    private readonly IAttributeTypeFactory _attributeTypeFactory;
    private readonly IAttributeValidator _attributeValidator;


    public AttributeRepository(MageNetDbContext dbContext, IAttributeTypeFactory attributeTypeFactory,
        IAttributeValidator attributeValidator)
    {
        _dbContext = dbContext;
        _attributeTypeFactory = attributeTypeFactory;
        _attributeValidator = attributeValidator;
    }

    public IEnumerable<IAttributeWithData> GetAttributes()
    {
        var attributes = _dbContext.Attributes.ToArray().Select(attr =>
            attr.MapToIAttributeWithNoValidation(_attributeTypeFactory));

        // map to IAttribute


        return attributes.Select(attribute => attribute.JoinWithSavedData())
            .ToArray();
    }

    public IAttributeWithData GetAttributeById(Guid guid)
    {
        var attribute = _getAttributeById(guid).MapToIAttributeWithNoValidation(_attributeTypeFactory);
        return attribute.JoinWithSavedData();
    }

    public Guid CreateNewAttribute(IPostAttributeWithData postAttributeWithData)
    {
        return new Attribute(_attributeValidator, _attributeTypeFactory)
        {
            AttributeType = _attributeTypeFactory.CreateAttributeType(postAttributeWithData.AttributeType)
        }.CreateNewDbEntry(postAttributeWithData);
    }

    public void UpdateAttribute(IPutAttributeWithData attributeWithData)
    {
        var savedAttribute = _getAttributeById(attributeWithData.AttributeId)
            .MapToIAttributeWithValidation(_attributeTypeFactory, _attributeValidator);
        
        savedAttribute.Update(attributeWithData);
    }

    public void DeleteAttributeById(Guid guid)
    {
        var attribute = _getAttributeById(guid);
        _dbContext.Attributes.Remove((AttributeEntity)attribute);
        _dbContext.SaveChanges();
    }

    private IAttributeEntity _getAttributeById(Guid guid)
    {
        var attribute = _dbContext.Attributes.AsNoTracking()
            .SingleOrDefault(attr => attr.AttributeId == guid);

        if (attribute == null)
        {
            throw new AttributeNotFoundException();
        }

        return attribute;
    }
}
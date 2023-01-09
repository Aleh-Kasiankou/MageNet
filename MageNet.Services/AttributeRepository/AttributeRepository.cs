using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.Extensions;
using MageNetServices.Interfaces;
using Attribute = MageNetServices.AttributeRepository.DTO.Attributes.Attribute;

namespace MageNetServices.AttributeRepository;

public class AttributeRepository : IAttributeRepository
{
    private readonly MageNetDbContext _dbContext;
    private readonly IAttributeTypeFactory _attributeTypeFactory;
    private readonly IAttributeValidator _attributeValidator;


    public AttributeRepository(MageNetDbContext dbContext, IAttributeTypeFactory attributeTypeFactory, IAttributeValidator attributeValidator)
    {
        _dbContext = dbContext;
        _attributeTypeFactory = attributeTypeFactory;
        _attributeValidator = attributeValidator;
    }

    public IEnumerable<IAttributeWithData> GetAttributes()
    {
        
        var attributes = _dbContext.Attributes.ToArray().Select(attr => 
            attr.MapToIAttribute(_attributeTypeFactory));

        // map to IAttribute


        return attributes.Select(attribute => attribute.JoinWithSavedData())
            .ToArray();
    }

    public IAttributeWithData GetAttributeById(Guid guid)
    {
        var attribute = _getAttributeById(guid).MapToIAttribute(_attributeTypeFactory);
        return attribute.JoinWithSavedData();
    }

    public Guid CreateNewAttribute(IPostAttributeWithData postAttributeWithData)
    {
        return new Attribute(_attributeValidator)
        {
            AttributeType = _attributeTypeFactory.CreateAttributeType(postAttributeWithData.AttributeType)
        }.SaveToDb(postAttributeWithData);
    }

    public IAttributeWithData UpdateAttribute(IAttributeWithData attributeWithData)
    {
        var attributeTypeBearer = _attributeTypeFactory.CreateAttributeType(attributeWithData.AttributeType);
        attributeTypeBearer.UpdateAttributeData(attributeWithData);
        return GetAttributeById(attributeWithData.AttributeId);
    }

    public void DeleteAttributeById(Guid guid)
    {
        var attribute = _getAttributeById(guid);
        _dbContext.Attributes.Remove((AttributeEntity)attribute);
        _dbContext.SaveChanges();
    }

    private IAttributeEntity _getAttributeById(Guid guid)
    {
        var attribute = _dbContext.Attributes
            .SingleOrDefault(attr => attr.AttributeId == guid);

        if (attribute == null)
        {
            throw new AttributeNotFoundException();
        }

        return attribute;
    }
}
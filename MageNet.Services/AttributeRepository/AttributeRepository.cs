using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO;
using MageNetServices.Extensions;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository;

public class AttributeRepository : IAttributeRepository
{
    private readonly MageNetDbContext _dbContext;
    private readonly IAttributeTypeFactory _attributeTypeFactory;


    public AttributeRepository(MageNetDbContext dbContext, IAttributeTypeFactory attributeTypeFactory)
    {
        _dbContext = dbContext;
        _attributeTypeFactory = attributeTypeFactory;
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
        var attributeWithData = new AttributeWithData
        {
            AttributeId = Guid.Empty,
            AttributeName = postAttributeWithData.AttributeName,
            AttributeType = postAttributeWithData.AttributeType,
            DefaultLiteralValue = postAttributeWithData.DefaultLiteralValue,
            EntityId = postAttributeWithData.EntityId,
            IsMultipleSelect = postAttributeWithData.IsMultipleSelect,
            SelectableOptions = postAttributeWithData.SelectableOptions?
                .Select(x => new SelectableAttributeValue
                {
                    IsDefaultValue = x.IsDefaultValue,
                    Value = x.Value
                })
        };

        
        var attributeTypeBearer = _attributeTypeFactory.CreateAttributeType(attributeWithData.AttributeType);
        var (attribute, attributeData) = attributeTypeBearer.DecoupleAttributeWithData(attributeWithData);
        _dbContext.Attributes.Add(attribute as AttributeEntity ?? throw new InvalidOperationException());
        _dbContext.SaveChanges();

        return attribute.AttributeId;
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
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO;

public class PriceTypeBearer : IAttributeTypeBearer
{
    private readonly IAttributeDataRepository<PriceAttributeData> _dataRepository;
    private readonly IAttributeTypeFactory _attributeTypeFactory;


    public PriceTypeBearer(IAttributeDataRepository<PriceAttributeData> dataRepository,
        IAttributeTypeFactory attributeTypeFactory)
    {
        _dataRepository = dataRepository;
        _attributeTypeFactory = attributeTypeFactory;
        AttributeType = AttributeType.Price;
    }

    public AttributeType AttributeType { get; }

    public void CreateData(IAttributeData attributeData)
    {
        _dataRepository.CreateAttributeData(attributeData);
    }

    public IAttributeWithData JoinWithData(IAttribute attribute)
    {
        var attributeData = _dataRepository.GetAttributeData(attribute.AttributeId);

        return new AttributeWithData()
        {
            AttributeId = attribute.AttributeId,
            EntityId = attribute.EntityId,
            AttributeName = attribute.AttributeName,
            AttributeType = attribute.AttributeType.AttributeType,
            DefaultLiteralValue = attributeData.DefaultValue.ToString(),
            SelectableOptions = null,
            IsMultipleSelect = null
        };
    }

    public void UpdateAttributeData(IAttributeWithData attributeData)
    {
        throw new NotImplementedException();
    }

    public (IAttributeEntity, IAttributeData) DecoupleAttributeWithData(IAttributeWithData attributeWithData)
    {
        var attribute = new AttributeEntity
        {
            AttributeId = attributeWithData.AttributeId,
            AttributeName = attributeWithData.AttributeName,
            AttributeType = attributeWithData.AttributeType,
            EntityId = attributeWithData.EntityId,
        };

        var attributeData = new PriceAttributeData
        {
            AttributeId = attributeWithData.AttributeId,
            DefaultValue = decimal.Parse(s: attributeWithData.DefaultLiteralValue
                                            ?? throw new DefaultLiteralValueNotProvidedException(
                                                "Price attribute must have a decimal value"))
        };

        return (attribute, attributeData);
    }
}
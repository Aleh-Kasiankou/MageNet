using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO.TypeBearers;

public class PriceTypeBearer : IAttributeTypeBearer
{
    private readonly IAttributeDataRepository<PriceAttributeData> _dataRepository;


    public PriceTypeBearer(IAttributeDataRepository<PriceAttributeData> dataRepository)
    {
        _dataRepository = dataRepository;
        AttributeType = AttributeType.Price;
    }

    public AttributeType AttributeType { get; }

    public Guid CreateNewDbEntry(IAttributeWithData attributeWithData)
    {
        var (attributeEntity, attributeData) =
            DecoupleAttributeWithData(attributeWithData);

        var attributeId = _dataRepository.CreateAttribute(attributeEntity);
        attributeData.AttributeId = attributeId;
        _dataRepository.CreateAttributeData(attributeData);
        _dataRepository.SaveChanges();

        return attributeId;
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

    public void UpdateAttributeData(IAttributeWithData attributeWithData, bool typeIsChanged)
    {
        var (attributeEntity, attributeData) = DecoupleAttributeWithData(attributeWithData);
        _dataRepository.UpdateAttribute(attributeEntity);

        if (typeIsChanged)
        {
            _dataRepository.CreateAttributeData(attributeData);
        }

        else
        {
            _dataRepository.UpdateAttributeData(attributeData);
        }

        _dataRepository.SaveChanges();
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

    public IAttributeWithData RemoveIrrelevantProperties(IAttributeWithData attributeWithData)
    {
        attributeWithData.SelectableOptions = null;
        attributeWithData.IsMultipleSelect = null;

        return attributeWithData;
    }
}
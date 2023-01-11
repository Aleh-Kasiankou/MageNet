using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.TypeBearers;

public class SelectableTypeBearer : IAttributeTypeBearer
{
    private readonly IAttributeDataRepository<SelectableAttributeData> _dataRepository;


    public SelectableTypeBearer(IAttributeDataRepository<SelectableAttributeData> dataRepository)
    {
        _dataRepository = dataRepository;
        AttributeType = AttributeType.Selectable;
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
            DefaultLiteralValue = null,
            SelectableOptions = attributeData.Values,
            IsMultipleSelect = attributeData.IsMultipleSelect
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
        var attribute = new AttributeEntity()
        {
            AttributeId = attributeWithData.AttributeId,
            AttributeName = attributeWithData.AttributeName,
            AttributeType = attributeWithData.AttributeType,
            EntityId = attributeWithData.EntityId,
        };

        var attributeData = new SelectableAttributeData()
        {
            AttributeId = attributeWithData.AttributeId,
            Values = attributeWithData.SelectableOptions ?? throw new SelectableOptionsNotProvidedException(
                "Null selectable options are not allowed. Please send the empty list of options if you plan to add them later."),
            IsMultipleSelect = attributeWithData.IsMultipleSelect ??
                               throw new IsMultipleSelectValueNotProvidedException(
                                   "IsMultipleSelect value must be provided for the selectable attributes.")
        };

        return (attribute, attributeData);
    }

    public void RemoveDbData(Guid attributeId)
    {
        _dataRepository.DeleteAttributeData(attributeId);
    }

    public IAttributeWithData RemoveIrrelevantProperties(IAttributeWithData attributeWithData)
    {
        attributeWithData.DefaultLiteralValue = null;
        return attributeWithData;
    }
}
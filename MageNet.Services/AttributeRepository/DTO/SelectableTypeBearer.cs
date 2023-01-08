using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO;

public class SelectableTypeBearer : IAttributeTypeBearer
{
    private readonly IAttributeDataRepository<SelectableAttributeData> _dataRepository;
    private readonly IAttributeTypeFactory _attributeTypeFactory;


    public SelectableTypeBearer(IAttributeDataRepository<SelectableAttributeData> dataRepository,
        IAttributeTypeFactory attributeTypeFactory)
    {
        _dataRepository = dataRepository;
        _attributeTypeFactory = attributeTypeFactory;
        AttributeType = AttributeType.Selectable;
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
            DefaultLiteralValue = null,
            SelectableOptions = attributeData.Values,
            IsMultipleSelect = attributeData.IsMultipleSelect
        };
    }

    public void UpdateAttributeData(IAttributeWithData attributeData)
    {
        throw new NotImplementedException();
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
}

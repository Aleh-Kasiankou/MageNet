using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO;
using MageNetServices.Interfaces;
using Attribute = MageNetServices.AttributeRepository.DTO.Attribute;

namespace MageNetServices.Extensions;

public static class AttributeMapper
{
    public static IAttribute MapToIAttribute(this IAttributeEntity attributeEntity, IAttributeTypeFactory typeFactory)
    {
        return new Attribute
        {
            AttributeId = attributeEntity.AttributeId,
            AttributeName = attributeEntity.AttributeName,
            AttributeType = typeFactory.CreateAttributeType(attributeEntity.AttributeType),
            EntityId = attributeEntity.EntityId
        };
    }

    public static IAttributeWithData MapToAttributeWithData(this IPostAttributeWithData postAttributeWithData)
    {
        return new AttributeWithData()
        {
            AttributeName = postAttributeWithData.AttributeName,
            AttributeType = postAttributeWithData.AttributeType,
            DefaultLiteralValue = postAttributeWithData.DefaultLiteralValue,
            EntityId = postAttributeWithData.EntityId,
            IsMultipleSelect = postAttributeWithData.IsMultipleSelect,
            SelectableOptions = postAttributeWithData.SelectableOptions?.Select(x => new SelectableAttributeValue()
            {
                IsDefaultValue = x.IsDefaultValue,
                Value = x.Value
            })
        };
    }

}
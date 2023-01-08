using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
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
}
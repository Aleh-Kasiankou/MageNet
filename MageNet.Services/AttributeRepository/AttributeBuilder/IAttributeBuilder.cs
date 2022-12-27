using MageNetServices.AttributeRepository.DTO.Attributes;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository.AttributeBuilder;

public interface IAttributeBuilder
{
    AttributeWithData GetAttributeWithData(Attribute attribute);

    Guid CreateAttributeWithData(AttributeWithData attributeWithData);

    AttributeWithData UpdateAttributeWithData(AttributeWithData attributeWithData);
}
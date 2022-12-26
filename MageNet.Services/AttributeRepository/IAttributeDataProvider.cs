using MageNetServices.AttributeRepository.DTO;
using MageNetServices.AttributeRepository.DTO.Attributes;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository;

public interface IAttributeDataProvider
{
    AttributeWithData GetAttributeWithData(Attribute attribute);

    Guid CreateAttributeWithData(AttributeWithData attributeWithData);

    AttributeWithData UpdateAttributeWithData(AttributeWithData attributeWithData);
}
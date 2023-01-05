using MageNetServices.AttributeRepository.DTO.Attributes;

namespace MageNetServices.Interfaces;

public interface IAttributeRepository
{
     IEnumerable<AttributeWithData> GetAttributes();

     AttributeWithData GetAttributeById(Guid guid);

     Guid CreateNewAttribute(PostAttributeWithData attributeWithData);

     AttributeWithData UpdateAttribute(AttributeWithData attributeWithData);

     void DeleteAttributeById(Guid guid);
}
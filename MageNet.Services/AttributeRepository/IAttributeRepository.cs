using MageNetServices.AttributeRepository.DTO;
using MageNetServices.AttributeRepository.DTO.Attributes;

namespace MageNetServices.AttributeRepository;

public interface IAttributeRepository
{
     IEnumerable<AttributeWithData> GetAttributes();

     AttributeWithData GetAttributeById(Guid guid);

     Guid CreateNewAttribute(PostAttributeWithData attributeWithData);

     AttributeWithData UpdateAttribute(AttributeWithData attributeWithData);

     void DeleteAttributeById(Guid guid);
}
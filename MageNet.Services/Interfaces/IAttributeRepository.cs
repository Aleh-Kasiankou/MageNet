namespace MageNetServices.Interfaces;

public interface IAttributeRepository
{
     IEnumerable<IAttributeWithData> GetAttributes();

     IAttributeWithData GetAttributeById(Guid guid);

     Guid CreateNewAttribute(IPostAttributeWithData attributeWithData);

     IAttributeWithData UpdateAttribute(IAttributeWithData attributeWithData);

     void DeleteAttributeById(Guid guid);
}
namespace MageNetServices.Interfaces;

public interface IAttributeRepository
{
     IEnumerable<IAttributeWithData> GetAttributes();

     IAttributeWithData GetAttributeById(Guid guid);

     Guid CreateNewAttribute(IPostAttributeWithData attributeWithData);

     void UpdateAttribute(IPutAttributeWithData attributeWithData);

     void DeleteAttributeById(Guid guid);
}
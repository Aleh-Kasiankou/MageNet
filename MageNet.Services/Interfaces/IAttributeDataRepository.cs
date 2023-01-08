using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNetServices.Interfaces;

public interface IAttributeDataRepository<T> where T : IAttributeData
{
    T GetAttributeData(Guid attributeId);

    Guid CreateAttribute(IAttributeEntity attributeEntity);
    void CreateAttributeData(IAttributeData attributeData);

    void UpdateAttributeData(IAttributeData attributeData);

    void SaveChanges();
}
namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface IAttributeDataRepository<T> where T : IAttributeData
{
    T GetAttributeData(Guid attributeId);

    void CreateAttributeData(IAttributeData attributeData);

    void UpdateAttributeData(IAttributeData attributeData);
}
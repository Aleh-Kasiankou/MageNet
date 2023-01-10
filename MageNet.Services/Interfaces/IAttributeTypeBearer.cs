using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNetServices.Interfaces;

public interface IAttributeTypeBearer
{
    AttributeType AttributeType { get; }
    
    Guid CreateNewDbEntry(IAttributeWithData postAttributeWithData);

    IAttributeWithData JoinWithData(IAttribute attribute);
    void UpdateAttributeData(IAttributeWithData attributeData, bool typeIsChanged);
    (IAttributeEntity, IAttributeData) DecoupleAttributeWithData(IAttributeWithData attributeWithData);

    IAttributeWithData RemoveIrrelevantProperties(IAttributeWithData attributeWithData);
}
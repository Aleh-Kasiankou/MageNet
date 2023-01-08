using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNetServices.Interfaces;

public interface IAttributeTypeBearer
{
    AttributeType AttributeType { get; }
    
    void CreateData(IAttributeData attributeData);

    IAttributeWithData JoinWithData(IAttribute attribute);

    void UpdateAttributeData(IAttributeWithData attributeData);
    (IAttributeEntity, IAttributeData) DecoupleAttributeWithData(IAttributeWithData attributeWithData);
}
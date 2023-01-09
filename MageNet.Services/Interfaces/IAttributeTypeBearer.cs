using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNetServices.Interfaces;

public interface IAttributeTypeBearer
{
    AttributeType AttributeType { get; }
    
    Guid SaveToDb(IAttributeWithData postAttributeWithData);

    IAttributeWithData JoinWithData(IAttribute attribute);

    void UpdateAttributeData(IAttributeWithData attributeData);
    (IAttributeEntity, IAttributeData) DecoupleAttributeWithData(IAttributeWithData attributeWithData);
}
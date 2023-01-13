using MageNet.Persistence.Models.AbstractModels.ModelEnums;

namespace MageNetServices.Interfaces;

public interface IAttributeTypeFactory
{
    public IAttributeTypeBearer CreateAttributeType(AttributeType attributeType);
    
    
}
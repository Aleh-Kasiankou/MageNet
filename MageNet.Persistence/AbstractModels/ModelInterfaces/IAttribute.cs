using MageNet.Persistence.AbstractModels.ModelEnums;

namespace MageNet.Persistence.AbstractModels.ModelInterfaces;

public interface IAttribute
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    
}
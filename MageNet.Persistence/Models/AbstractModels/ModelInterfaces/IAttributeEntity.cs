using MageNet.Persistence.Models.AbstractModels.ModelEnums;

namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface IAttributeEntity
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }

    public Guid EntityId { get; set; }

    AttributeType AttributeType { get; set; }

}
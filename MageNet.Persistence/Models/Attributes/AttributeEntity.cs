using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models.Attributes;

public class AttributeEntity : IAttributeEntity
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    public Guid EntityId { get; set; }
    public virtual Entity Entity { get; set; }
    public AttributeType AttributeType { get; set; }
}
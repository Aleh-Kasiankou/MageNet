using MageNet.Persistence.AbstractModels.ModelEnums;
using MageNet.Persistence.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models.Attributes;

public class Attribute : IAttribute
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    public AttributeType AttributeType { get; set; }
    public Guid EntityId { get; set; }
}
using System.Text.Json.Serialization;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models.Attributes;

public class Attribute : IAttribute
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    public AttributeType AttributeType { get; set; }
    [JsonIgnore]
    public virtual Entity Entity { get; set; }
    public Guid EntityId { get; set; }
    
    
    
}
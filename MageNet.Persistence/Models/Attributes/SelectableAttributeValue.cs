using System.Text.Json.Serialization;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models.Attributes;

public class SelectableAttributeValue : ISelectableAttributeValue
{
    public Guid SelectableAttributeValueId { get; set; }
    
    public Guid AttributeId { get; set; }
    [JsonIgnore]
    public virtual SelectableAttribute Attribute { get; set; }
    
    public string Value { get; set; } 
    public bool IsDefaultValue { get; set; }
}
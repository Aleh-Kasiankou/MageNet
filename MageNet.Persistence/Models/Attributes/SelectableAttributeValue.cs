using System.Text.Json.Serialization;

namespace MageNet.Persistence.Models.Attributes;

public class SelectableAttributeOption
{
    public Guid OptionId { get; set; }
    
    public Guid AttributeDataId { get; set; }
    [JsonIgnore]
    public virtual SelectableAttributeData Attribute { get; set; }
    
    public string Value { get; set; } 
    public bool IsDefaultValue { get; set; }
}
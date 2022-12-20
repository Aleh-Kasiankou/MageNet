namespace MageNet.Persistence.Models.Attributes;

public class SelectableAttributeValue
{
    public Guid SelectableAttributeValueId { get; set; }
    
    public Guid AttributeId { get; set; }
    public virtual SelectableAttribute Attribute { get; set; }
    
    public string Value { get; set; } 
    public bool IsDefaultValue { get; set; }
}
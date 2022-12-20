namespace MageNet.Models.Attributes.Selectable;

public class UpdateSelectableValueRequest
{
    public Guid SelectableAttributeValueId { get; set; }
    public string Value { get; set; } 
    public bool IsDefaultValue { get; set; }
}
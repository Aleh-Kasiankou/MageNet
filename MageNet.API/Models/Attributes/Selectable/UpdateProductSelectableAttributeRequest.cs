namespace MageNet.Models.Attributes.Selectable;

public class UpdateProductSelectableAttributeRequest
{
    public Guid AttributeId { get; set; }
    public string Name { get; set; }
    public bool IsMultipleSelect { get; set; }
    public Guid EntityId { get; set; }
    public IEnumerable<UpdateSelectableValueRequest> SelectableValues { get; set; }
}
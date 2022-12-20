namespace MageNet.Models.Attributes.Selectable;

public class CreateProductSelectableAttribute
{
    public string Name { get; set; }
    public bool IsMultipleSelect { get; set; }
    public Guid EntityId { get; set; }
    public IEnumerable<CreateSelectableValueRequest> SelectableValues { get; set; }
}
using MageNet.Persistence.AbstractModels;

namespace MageNet.Persistence.Models.Attributes;

public class SelectableAttribute
{
    public Guid AttributeId { get; set; }
    public bool IsMultipleSelect { get; set; }
    public IEnumerable<SelectableAttributeValue> Values { get; set; }
}
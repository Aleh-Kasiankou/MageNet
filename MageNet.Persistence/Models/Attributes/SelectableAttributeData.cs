using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models.Attributes;

public class SelectableAttributeData : IAttributeData, ISelectableAttributeData
{
    public Guid SelectableAttributeId { get; set; }
    public Guid AttributeId { get; set; }
    public virtual AttributeEntity Attribute { get; set; }

    public bool IsMultipleSelect { get; set; }
    public IEnumerable<SelectableAttributeValue> Values { get; set; } = new List<SelectableAttributeValue>();
}
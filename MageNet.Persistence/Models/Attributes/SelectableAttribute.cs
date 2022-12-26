namespace MageNet.Persistence.Models.Attributes;

public class SelectableAttribute : IAttributeData
{
    public Guid SelectableAttributeId { get; set; }
    public Guid AttributeId { get; set; }
    public virtual Attribute Attribute { get; set; }

    public bool IsMultipleSelect { get; set; }
    public IEnumerable<SelectableAttributeValue> Values { get; set; } = new List<SelectableAttributeValue>();
}
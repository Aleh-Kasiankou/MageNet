using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNet.Persistence.Models;

public class AttributeSet
{
    public Guid AttributeSetId { get; set; }
    public string Name { get; set; }
    public Guid EntityId { get; set; }
    public virtual IEnumerable<Attribute> Attributes { get; set; }
}
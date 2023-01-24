using MageNet.Persistence.Models.Attributes;

namespace MageNet.Persistence.Models;

public class AttributeSet
{
    public Guid AttributeSetId { get; set; }
    public string Name { get; set; }
    public Guid EntityId { get; set; }
    public virtual IEnumerable<AttributeEntity> Attributes { get; set; }
}
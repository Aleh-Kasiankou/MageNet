using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNet.Models;

public class CreateAttributeSetRequest
{
    public string Name { get; set; }
    public Guid EntityId { get; set; }
    public virtual IEnumerable<Attribute> Attributes { get; set; }
}
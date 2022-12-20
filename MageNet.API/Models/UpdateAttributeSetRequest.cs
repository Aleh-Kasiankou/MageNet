using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNet.Models;

public class UpdateAttributeSetRequest
{
    public Guid AttributeSetId { get; set; }
    public string Name { get; set; }
    public virtual IEnumerable<Attribute> Attributes { get; set; }
}
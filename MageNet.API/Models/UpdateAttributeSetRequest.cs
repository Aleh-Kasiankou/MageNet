using MageNet.Persistence.AbstractModels.ModelInterfaces;

namespace MageNet.Models;

public class UpdateAttributeSetRequest
{
    public Guid AttributeSetId { get; set; }
    public string Name { get; set; }
    public virtual IEnumerable<IAttribute<IEntity>> Attributes { get; set; }
}
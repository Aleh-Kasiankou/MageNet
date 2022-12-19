using MageNet.Persistence.AbstractModels.ModelInterfaces;

namespace MageNet.Models;

public class CreateAttributeSetRequest
{
    public string Name { get; set; }
    public Guid EntityId { get; set; }
    public virtual IEnumerable<IAttribute<IEntity>> Attributes { get; set; }
}
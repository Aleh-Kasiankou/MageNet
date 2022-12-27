using MageNet.Persistence.Models.AbstractModels.ModelEnums;

namespace MageNet.Persistence.Models;

public class Entity
{
    public Guid EntityId { get; set; }
    public string Name { get; set; }
    public EntityType EntityType { get; set; }
}
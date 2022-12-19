using MageNet.Persistence.AbstractModels.ModelEnums;

namespace MageNet.Persistence.Models;

public class Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public EntityType EntityType { get; set; }
}
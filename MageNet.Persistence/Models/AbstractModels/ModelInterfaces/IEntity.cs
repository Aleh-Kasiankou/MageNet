namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface IEntity
{
    public Guid EntityId { get; set; }
    public Entity Entity { get; set; }
    public DateTime CreatedDate { get; set; }
}
using MageNet.Persistence.AbstractModels.ModelEnums;

namespace MageNet.Persistence.AbstractModels.ModelInterfaces;

public interface IAttribute<T> where T : IEntity
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }

    public AttributeType AttributeType { get; set; }
    
    public Guid EntityId { get; set; }
    public T Entity { get; set; }
}
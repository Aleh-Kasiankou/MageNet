namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface IAttribute
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    
}
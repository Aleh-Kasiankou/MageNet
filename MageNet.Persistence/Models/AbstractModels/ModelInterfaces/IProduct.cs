using MageNet.Persistence.Models.AbstractModels.ModelEnums;

namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface IProduct : IEntity
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public ProductType ProductType { get; set; }
    
    public Guid AttributeSetId { get; set; }
    public AttributeSet AttributeSet { get; set; }
    
}
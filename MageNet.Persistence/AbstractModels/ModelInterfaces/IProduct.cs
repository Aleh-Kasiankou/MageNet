using MageNet.Persistence.AbstractModels.ModelEnums;
using MageNet.Persistence.Models;

namespace MageNet.Persistence.AbstractModels.ModelInterfaces;

public interface IProduct : IEntity
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public ProductType ProductType { get; set; }
    
    public Guid AttributeSetId { get; set; }
    public AttributeSet AttributeSet { get; set; }
    
}
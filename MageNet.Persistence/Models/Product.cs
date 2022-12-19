using MageNet.Persistence.AbstractModels.ModelEnums;
using MageNet.Persistence.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models;

public class Product : IProduct
{
    public Guid EntityId { get; set; }
    public virtual Entity Entity { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public ProductType ProductType { get; set; }
    
    public Guid AttributeSetId { get; set; }
    public virtual AttributeSet AttributeSet { get; set; }
}
using MageNet.Persistence.AbstractModels.ModelEnums;
using MageNet.Persistence.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models;

public class ProductAttribute : IAttribute<Product>
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    
    public AttributeType AttributeType { get; set; }

    public virtual Guid EntityId { get; set; }
    public Product Entity { get; set; }
}
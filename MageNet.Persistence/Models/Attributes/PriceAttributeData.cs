using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models.Attributes;

public class PriceAttributeData : IAttributeData, IPriceAttributeData
{
    public Guid PriceAttributeId { get; set; }
    public Guid AttributeId { get; set; }
    public virtual AttributeEntity Attribute { get; set; }
    public decimal DefaultValue { get; set; }
}
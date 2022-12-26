namespace MageNet.Persistence.Models.Attributes;

public class PriceAttribute : IAttributeData
{
    public Guid PriceAttributeId { get; set; }
    public Guid AttributeId { get; set; }
    public virtual Attribute Attribute { get; set; }
    public decimal DefaultValue { get; set; }
}
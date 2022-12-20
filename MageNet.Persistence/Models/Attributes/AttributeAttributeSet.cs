namespace MageNet.Persistence.Models.Attributes;

public class AttributeAttributeSet
{
    public Guid AttributeAttributeSetId { get; set; }
    
    public Guid AttributeId { get; set; }
    public virtual Attribute Attribute { get; set; }

    public Guid AttributeSetId { get; set; }
    public virtual AttributeSet AttributeSet { get; set; }
    
}
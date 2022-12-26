namespace MageNet.Persistence.Models.Attributes;

public class TextAttribute : IAttributeData
{
    public Guid TextAttributeId { get; set; }
    public Guid AttributeId { get; set; }
    public virtual Attribute Attribute { get; set; }

    public string DefaultValue { get; set; } = "";
}
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models.Attributes;

public class TextAttribute : IAttributeData, ITextAttributeData
{
    public Guid TextAttributeId { get; set; }
    public Guid AttributeId { get; set; }
    public virtual Attribute Attribute { get; set; }

    public string DefaultValue { get; set; } = "";
}
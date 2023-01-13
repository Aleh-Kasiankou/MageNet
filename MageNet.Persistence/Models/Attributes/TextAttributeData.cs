using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models.Attributes;

public class TextAttributeData : IAttributeData, ITextAttributeData
{
    public Guid TextAttributeId { get; set; }
    public Guid AttributeId { get; set; }
    public virtual AttributeEntity Attribute { get; set; }

    public string DefaultValue { get; set; } = "";
}
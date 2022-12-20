using MageNet.Persistence.AbstractModels;

namespace MageNet.Persistence.Models.Attributes;

public class TextAttribute
{
    public Guid AttributeId { get; set; }
    public string DefaultValue { get; set; } = "";
}
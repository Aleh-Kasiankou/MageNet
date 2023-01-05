using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface IAttributeData
{
    public Guid AttributeId { get; set; }
    public Attribute Attribute { get; set; }
}
using MageNet.Persistence.Models.Attributes;
namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface IAttributeData
{
    public Guid AttributeId { get; set; }
    public AttributeEntity Attribute { get; set; }
}
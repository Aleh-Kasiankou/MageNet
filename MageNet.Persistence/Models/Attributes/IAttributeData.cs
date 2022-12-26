namespace MageNet.Persistence.Models.Attributes;

public interface IAttributeData
{
    public Guid AttributeId { get; set; }
    public Attribute Attribute { get; set; }
}
using MageNet.Persistence.AbstractModels.ModelEnums;

namespace MageNet.Models;

public class UpdateProductAttributeRequest
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    public AttributeType AttributeType { get; set; }
}
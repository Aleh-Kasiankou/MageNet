using MageNet.Persistence.AbstractModels.ModelEnums;

namespace MageNet.Models;

public class CreateProductAttributeRequest
{
    public string AttributeName { get; set; }
    
    public AttributeType AttributeType { get; set; }
    
}
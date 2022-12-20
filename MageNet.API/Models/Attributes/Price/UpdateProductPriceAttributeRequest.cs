namespace MageNet.Models.Attributes.Price;

public class UpdateProductPriceAttributeRequest
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    public Decimal DefaultValue { get; set; }
    
}
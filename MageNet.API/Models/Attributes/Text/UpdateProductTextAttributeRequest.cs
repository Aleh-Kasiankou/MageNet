namespace MageNet.Models.Attributes.Text;

public class UpdateProductTextAttributeRequest
{
    public Guid Id { get; set; }
    public string AttributeName { get; set; }
    public string DefaultValue { get; set; }
}
using MageNet.Persistence.AbstractModels;

namespace MageNet.Persistence.Models.Attributes;

public class PriceAttribute
{
    public Guid AttributeId { get; set; }
    public decimal DefaultValue { get; set; }
}
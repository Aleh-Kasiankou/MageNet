using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;

namespace MageNetServices.Interfaces;

public interface IAttributeWithData
{
    public Guid AttributeId { get; set; }
    public Guid EntityId { get; set; }
    public string AttributeName { get; set; }
    public AttributeType AttributeType { get; set; }
    
    public string? DefaultLiteralValue { get; set; }
    public IEnumerable<SelectableAttributeValue>? SelectableOptions { get; set; }
    public bool? IsMultipleSelect { get; set; }
}
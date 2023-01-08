using MageNet.Persistence.Models.AbstractModels.ModelEnums;

namespace MageNetServices.Interfaces;

public interface IPostAttributeWithData
{
    public Guid EntityId { get; set; }
    public string AttributeName { get; set; }
    public AttributeType AttributeType { get; set; }
    public string DefaultLiteralValue { get; set; }
    public IEnumerable<IPostSelectableOption> SelectableOptions { get; set; }
    public bool? IsMultipleSelect { get; set; }
}
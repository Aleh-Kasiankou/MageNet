using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO.Attributes;

public class PutAttributeWithData : IPutAttributeWithData
{
    public Guid AttributeId { get; set; }
    public Guid? EntityId { get; set; }
    public string? AttributeName { get; set; }
    public AttributeType? AttributeType { get; set; }
    public string? DefaultLiteralValue { get; set; }
    public IEnumerable<PutSelectableOption>? SelectableOptions { get; set; }
    public bool? IsMultipleSelect { get; set; }
}
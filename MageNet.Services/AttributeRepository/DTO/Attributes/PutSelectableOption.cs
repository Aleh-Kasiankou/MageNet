using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO.Attributes;

public class PutSelectableOption : IPutSelectableOption
{
    public Guid? OptionId { get; set; }
    public string? Value { get; set; }
    public bool? IsDefaultValue { get; set; }
    public bool IsToDelete { get; set; }
}
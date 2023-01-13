using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO.Attributes;

public class PostSelectableOption: IPostSelectableOption
{
    public string Value { get; set; } 
    public bool IsDefaultValue { get; set; }
}
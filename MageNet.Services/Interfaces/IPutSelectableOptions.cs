namespace MageNetServices.Interfaces;

public interface IPutSelectableOption
{
    public Guid OptionId { get; set; }
    public string? Value { get; set; } 
    public bool? IsDefaultValue { get; set; }
}
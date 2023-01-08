namespace MageNetServices.Interfaces;

public interface IPostSelectableOption
{
    public string Value { get; set; } 
    public bool IsDefaultValue { get; set; }
}
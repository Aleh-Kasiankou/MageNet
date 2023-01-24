namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface ITextAttributeData
{
    public Guid TextAttributeId { get; set; }
    public string DefaultValue { get; set; } 
}
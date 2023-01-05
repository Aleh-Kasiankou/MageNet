namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface IPriceAttributeData
{
    public Guid PriceAttributeId { get; set; }
    public decimal DefaultValue { get; set; }
}
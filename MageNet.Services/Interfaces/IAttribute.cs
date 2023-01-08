namespace MageNetServices.Interfaces;

public interface IAttribute
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }

    public Guid EntityId { get; set; }

    IAttributeTypeBearer AttributeType { get; set; }
    IAttributeWithData JoinWithSavedData();
    
}
using System.Text.Json.Serialization;
using MageNet.Persistence.Models;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO;

public class Attribute : IAttribute
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    public IAttributeTypeBearer AttributeType { get; set; }
    
    [JsonIgnore]
    public virtual Entity Entity { get; set; }
    public Guid EntityId { get; set; }
    
    public IAttributeWithData JoinWithSavedData()
    {
        return AttributeType.JoinWithData(this);
    }
    
}
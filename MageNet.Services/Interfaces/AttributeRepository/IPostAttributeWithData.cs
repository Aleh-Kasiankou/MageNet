using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNetServices.AttributeRepository.DTO.Attributes;

namespace MageNetServices.Interfaces;

public interface IPostAttributeWithData
{
    public Guid EntityId { get; set; }
    public string AttributeName { get; set; }
    public AttributeType AttributeType { get; set; }
    public string DefaultLiteralValue { get; set; }
    public IEnumerable<PostSelectableOption>? SelectableOptions { get; set; }
    
    // need tight coupling because deserialization of interfaces is not supported
    
    public bool? IsMultipleSelect { get; set; }
}
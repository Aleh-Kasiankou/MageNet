using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository.DTO.Attributes;

public class PostAttributeWithData
{
    public Guid? EntityId { get; set; }
    public string? AttributeName { get; set; }
    public AttributeType? AttributeType { get; set; }
    public string? DefaultLiteralValue { get; set; }
    public IEnumerable<PostSelectableOptions>? SelectableOptions { get; set; }

    public bool? IsMultipleSelect { get; set; }

    public PostAttributeWithData()
    {
        
    }
    
}
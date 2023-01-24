using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO.Attributes;

public class PostAttributeWithData : IPostAttributeWithData
{
    public Guid EntityId { get; set; }
    public string AttributeName { get; set; }
    public AttributeType AttributeType { get; set; }
    public string? DefaultLiteralValue { get; set; }
    public IEnumerable<PostSelectableOption>? SelectableOptions { get; set; }

    public bool? IsMultipleSelect { get; set; }
}
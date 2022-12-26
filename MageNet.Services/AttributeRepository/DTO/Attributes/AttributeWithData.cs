using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository.DTO.Attributes;

public class AttributeWithData
{
    public Guid AttributeId { get; set; }
    public Guid? EntityId { get; set; }
    public string? AttributeName { get; set; }
    public AttributeType? AttributeType { get; set; }
    public string? DefaultLiteralValue { get; set; }
    public IEnumerable<SelectableAttributeValue>? SelectableOptions { get; set; }

    public bool? IsMultipleSelect { get; set; }

    public AttributeWithData()
    {
        
    }
    
    public AttributeWithData(Attribute attribute, PriceAttribute priceAttributeData)
    {
        AttributeId = attribute.AttributeId;
        EntityId = attribute.EntityId;
        AttributeName = attribute.AttributeName;
        AttributeType = attribute.AttributeType;
        DefaultLiteralValue = priceAttributeData.DefaultValue.ToString();
        SelectableOptions = null;
        IsMultipleSelect = null;
    }

    public AttributeWithData(Attribute attribute, TextAttribute textAttributeData)
    {
        AttributeId = attribute.AttributeId;
        EntityId = attribute.EntityId;
        AttributeName = attribute.AttributeName;
        AttributeType = attribute.AttributeType;
        DefaultLiteralValue = textAttributeData.DefaultValue;
        SelectableOptions = null;
        IsMultipleSelect = null;
    }

    public AttributeWithData(Attribute attribute, SelectableAttribute selectableAttributeData)
    {
        AttributeId = attribute.AttributeId;
        EntityId = attribute.EntityId;
        AttributeName = attribute.AttributeName;
        AttributeType = attribute.AttributeType;
        DefaultLiteralValue = null;
        SelectableOptions = selectableAttributeData.Values;
        IsMultipleSelect = selectableAttributeData.IsMultipleSelect;
    }
}
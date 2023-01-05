using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.Exceptions;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeAssembler;

public class AttributeAssembler : IAttributeAssembler
{
    private static readonly Dictionary<AttributeType, Type> TypeCompatibilityTable = new()
    {
        { AttributeType.Price, typeof(PriceAttribute) },
        { AttributeType.Text, typeof(TextAttribute) },
        { AttributeType.Selectable, typeof(SelectableAttribute) },
    };

    public IAttributeWithData JoinAttributeWithData(IAttribute attribute, IAttributeData attributeData)
    {
        CheckDataTypesCompatibleWithAttribute(attribute, attributeData);

        IAttributeWithData attributeWithData = MapCommonAttributeInfo(attribute);

        switch (attribute.AttributeType)
        {
            case AttributeType.Price:
                attributeWithData = JoinPriceAttributeWithData(attributeWithData, attributeData);
                break;
            case AttributeType.Text:
                attributeWithData = JoinTextAttributeWithData(attributeWithData, attributeData);
                break;
            case AttributeType.Selectable:
                attributeWithData = JoinSelectableAttributeWithData(attributeWithData, attributeData);
                break;

            default:
                throw new NotSupportedException(
                    $"Attribute assembly failed. Attribute type '{attribute.AttributeType.ToString()}' is not supported");
        }

        return attributeWithData;
    }

    private void CheckDataTypesCompatibleWithAttribute(IAttribute attribute, IAttributeData attributeData)
    {
        if (!(attributeData.GetType() == TypeCompatibilityTable[attribute.AttributeType]))
        {
            throw new AttributeAssemblyDataTypeMismatchException();
        }
    }

    private IAttributeWithData MapCommonAttributeInfo(IAttribute attribute)
    {
        return new AttributeWithData
        {
            AttributeId = attribute.AttributeId,
            EntityId = attribute.EntityId,
            AttributeName = attribute.AttributeName,
            AttributeType = attribute.AttributeType,
        };
    }

    private IAttributeWithData JoinPriceAttributeWithData(IAttributeWithData attribute, IAttributeData attributeData)
    {
        var priceAttributeData = attributeData as IPriceAttributeData ?? throw new InvalidOperationException();


        attribute.DefaultLiteralValue = priceAttributeData.DefaultValue.ToString();
        attribute.SelectableOptions = null;
        attribute.IsMultipleSelect = null;

        return attribute;
    }

    private IAttributeWithData JoinTextAttributeWithData(IAttributeWithData attribute, IAttributeData attributeData)
    {
        var textAttributeData = attributeData as ITextAttributeData ?? throw new InvalidOperationException();


        attribute.DefaultLiteralValue = textAttributeData.DefaultValue;
        attribute.SelectableOptions = null;
        attribute.IsMultipleSelect = null;

        return attribute;
    }

    private IAttributeWithData JoinSelectableAttributeWithData(IAttributeWithData attribute,
        IAttributeData attributeData)
    {
        var selectableAttributeData =
            attributeData as ISelectableAttributeData ?? throw new InvalidOperationException();

        attribute.DefaultLiteralValue = null;
        attribute.SelectableOptions = selectableAttributeData.Values;
        attribute.IsMultipleSelect = selectableAttributeData.IsMultipleSelect;

        return attribute;
    }
}
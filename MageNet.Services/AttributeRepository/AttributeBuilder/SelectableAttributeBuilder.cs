using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using Microsoft.EntityFrameworkCore;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository.AttributeBuilder;

public partial class AttributeBuilder
{
    private SelectableAttribute GetSelectableAttributeData(Attribute attribute)
    {
        var selectableAttributeData = _dbContext.SelectableAttributes.Include(x => x.Values)
            .SingleOrDefault(x => x.AttributeId == attribute.AttributeId);

        if (selectableAttributeData != null)
        {
            return selectableAttributeData;
        }

        throw new ArgumentException(
            $"The wrong attribute id was provided. There is no selectable attribute with id '{attribute.AttributeId}'");
    }

    private SelectableAttribute CreateSelectableAttributeData(AttributeWithData attributeWithData, Attribute attribute)
    {
        var newValuesSet = new List<SelectableAttributeValue>();

        if (attributeWithData.SelectableOptions != null)
        {
            foreach (var value in attributeWithData.SelectableOptions)
            {
                value.AttributeId = attribute.AttributeId;
                newValuesSet.Add(value);
            }
        }

        return new SelectableAttribute
        {
            Attribute = attribute,
            IsMultipleSelect = (bool)attributeWithData.IsMultipleSelect,
            Values = newValuesSet
        };
    }

    private SelectableAttribute UpdateSelectableAttributeData(AttributeWithData attributeWithData, Attribute attribute,
        out bool isAttributeTypeChanged)
    {
        var selectableAttribute =
            _dbContext.SelectableAttributes.SingleOrDefault(x => x.AttributeId == attributeWithData.AttributeId);

        if (selectableAttribute != null)
        {
            isAttributeTypeChanged = false;
            selectableAttribute.IsMultipleSelect = (bool)attributeWithData.IsMultipleSelect;
            selectableAttribute.Values = attributeWithData.SelectableOptions;
        }

        else
        {
            isAttributeTypeChanged = true;
            selectableAttribute = CreateSelectableAttributeData(attributeWithData, attribute);
        }

        return selectableAttribute;
    }
}
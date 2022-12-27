using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository.AttributeBuilder;

public partial class AttributeBuilder
{
    private TextAttribute GetTextAttributeData(Attribute attribute)
    {
        var textAttributeData = _dbContext.TextAttributes
            .FirstOrDefault(x => x.AttributeId == attribute.AttributeId);

        if (textAttributeData != null)
        {
            return textAttributeData;
        }

        else
        {
            throw new ArgumentException(
                $"The wrong attribute id was provided. There is no text attribute with id '{attribute.AttributeId}'"
            );
        }
    }
    
    private TextAttribute CreateTextAttributeData(AttributeWithData attributeWithData, Attribute attribute)
    {
        return new TextAttribute
        {
            Attribute = attribute,
            DefaultValue = attributeWithData.DefaultLiteralValue
        };
    }
    
    private TextAttribute UpdateTextAttributeData(AttributeWithData attributeWithData, Attribute attribute,
        out bool isAttributeTypeChanged)
    {
        var textAttribute =
            _dbContext.TextAttributes.SingleOrDefault(x => x.AttributeId == attributeWithData.AttributeId);
        if (textAttribute != null)
        {
            isAttributeTypeChanged = false;
            textAttribute.DefaultValue = attributeWithData.DefaultLiteralValue;
        }

        else
        {
            isAttributeTypeChanged = true;
            textAttribute = CreateTextAttributeData(attributeWithData, attribute);
        }

        return textAttribute;
    }
    
    
}
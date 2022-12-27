using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository.AttributeBuilder;

public partial class AttributeBuilder
{
    private PriceAttribute GetPriceAttributeData(Attribute attribute)
    {
        var priceAttributeData = _dbContext.PriceAttributes
            .SingleOrDefault(x => x.AttributeId == attribute.AttributeId);

        if (priceAttributeData != null)
        {
            return priceAttributeData;
        }

        else
        {
            throw new ArgumentException(
                $"The wrong attribute id was provided. There is no price attribute with id '{attribute.AttributeId}'");
        }
    }
    
    private PriceAttribute CreatePriceAttributeData(AttributeWithData attributeWithData, Attribute attribute)
    {
        return new PriceAttribute
        {
            Attribute = attribute,
            DefaultValue = decimal.Parse(attributeWithData.DefaultLiteralValue)
        };
    }
    
    
    private PriceAttribute UpdatePriceAttributeData(AttributeWithData attributeWithData, Attribute attribute,
        out bool isAttributeTypeChanged)
    {
        var priceAttribute =
            _dbContext.PriceAttributes.SingleOrDefault(x => x.AttributeId == attributeWithData.AttributeId);
        if (priceAttribute != null)
        {
            isAttributeTypeChanged = false;
            priceAttribute.DefaultValue = Decimal.Parse(attributeWithData.DefaultLiteralValue);
        }

        else
        {
            isAttributeTypeChanged = true;
            priceAttribute = CreatePriceAttributeData(attributeWithData, attribute);
        }

        return priceAttribute;
    }
    
}
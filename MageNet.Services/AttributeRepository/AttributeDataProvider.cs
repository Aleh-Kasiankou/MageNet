using MageNet.Persistence;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using Microsoft.EntityFrameworkCore;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository;

public class AttributeDataProvider : IAttributeDataProvider
{
    private readonly MageNetDbContext _dbContext;

    public AttributeDataProvider(MageNetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public AttributeWithData GetAttributeWithData(Attribute attribute)
    {
        AttributeWithData attributeWithData;

        switch (attribute.AttributeType)
        {
            case AttributeType.Price:
                attributeWithData = new AttributeWithData(attribute, GetPriceAttributeData(attribute));
                break;
            case AttributeType.Text:
                attributeWithData = new AttributeWithData(attribute, GetTextAttributeData(attribute));
                break;
            case AttributeType.Selectable:
                attributeWithData = new AttributeWithData(attribute, GetSelectableAttributeData(attribute));
                break;

            default:
                throw new NotSupportedException(
                    $"The attribute type '{attribute.AttributeType.ToString()}' is not supported");
        }

        return attributeWithData;
    }

    public Guid CreateAttributeWithData(AttributeWithData attributeWithData)
    {
        var newAttribute = new Attribute
        {
            AttributeType = (AttributeType)attributeWithData.AttributeType,
            AttributeName = attributeWithData.AttributeName,
            EntityId = (Guid)attributeWithData.EntityId
        };

        using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
        {
            _dbContext.Attributes.Add(newAttribute);

            switch (attributeWithData.AttributeType)
            {
                case AttributeType.Price:
                    _dbContext.PriceAttributes.Add(CreatePriceAttributeData(attributeWithData, newAttribute));
                    break;
                case AttributeType.Text:
                    _dbContext.TextAttributes.Add(CreateTextAttributeData(attributeWithData, newAttribute));
                    break;

                case AttributeType.Selectable:
                    _dbContext.SelectableAttributes.Add(CreateSelectableAttributeData(attributeWithData, newAttribute));
                    break;

                default:
                    dbContextTransaction.Rollback();
                    throw new ArgumentException(
                        $"Unable to save attribute. Unknown attribute type: '{attributeWithData.AttributeType.ToString()}'");
            }

            _dbContext.SaveChanges();

            dbContextTransaction.Commit();
        }

        return newAttribute.AttributeId;
    }

    public AttributeWithData UpdateAttributeWithData(AttributeWithData attributeWithData)
    {
        var attribute = _dbContext.Attributes.SingleOrDefault(x => x.AttributeId == attributeWithData.AttributeId);

        if (attribute != null)
        {
            attribute.AttributeType = (AttributeType)attributeWithData.AttributeType;
            attribute.AttributeName = attributeWithData.AttributeName;

            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Attributes.Update(attribute);

                bool isAttributeTypeChanged;

                switch (attributeWithData.AttributeType)
                {
                    case AttributeType.Price:
                        var priceAttributeData = UpdatePriceAttributeData(attributeWithData, attribute,
                            out isAttributeTypeChanged);

                        if (isAttributeTypeChanged)
                        {
                            _dbContext.PriceAttributes.Add(priceAttributeData);
                        }

                        else
                        {
                            _dbContext.PriceAttributes.Update(priceAttributeData);
                        }

                        break;

                    case AttributeType.Text:

                        var textAttribute = UpdateTextAttributeData(attributeWithData, attribute,
                            out isAttributeTypeChanged);

                        if (isAttributeTypeChanged)
                        {
                            _dbContext.TextAttributes.Add(textAttribute);
                        }

                        else
                        {
                            _dbContext.TextAttributes.Update(textAttribute);
                        }

                        break;

                    case AttributeType.Selectable:

                        var selectableAttribute = UpdateSelectableAttributeData(attributeWithData, attribute,
                            out isAttributeTypeChanged);

                        if (isAttributeTypeChanged)
                        {
                            _dbContext.SelectableAttributes.Add(selectableAttribute);
                        }

                        else
                        {
                            _dbContext.SelectableAttributes.Update(selectableAttribute);
                        }

                        break;

                    default:
                        dbContextTransaction.Rollback();
                        throw new ArgumentException(
                            $"Unable to save attribute. Unknown attribute type: '{attributeWithData.AttributeType.ToString()}'");
                }

                _dbContext.SaveChanges();

                dbContextTransaction.Commit();

                return attributeWithData;
            }
        }

        else
        {
            throw new ArgumentException(
                $"The wrong attribute id was provided. There is no attribute with id '{attributeWithData.AttributeId}'");
        }
    }


    private PriceAttribute GetPriceAttributeData(Attribute attribute)
    {
        var priceAttributeData = _dbContext.PriceAttributes
            .FirstOrDefault(x => x.AttributeId == attribute.AttributeId);

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

    private SelectableAttribute GetSelectableAttributeData(Attribute attribute)
    {
        var selectableAttributeData = _dbContext.SelectableAttributes.Include(x => x.Values)
            .FirstOrDefault(x => x.AttributeId == attribute.AttributeId);

        if (selectableAttributeData != null)
        {
            return selectableAttributeData;
        }

        else
        {
            throw new ArgumentException(
                $"The wrong attribute id was provided. There is no selectable attribute with id '{attribute.AttributeId}'");
        }
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
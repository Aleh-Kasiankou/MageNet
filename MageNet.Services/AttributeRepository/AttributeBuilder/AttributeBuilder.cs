using MageNet.Persistence;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNetServices.AttributeRepository.DTO.Attributes;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNetServices.AttributeRepository.AttributeBuilder;

public partial class AttributeBuilder : IAttributeBuilder
{
    private readonly MageNetDbContext _dbContext;

    public AttributeBuilder(MageNetDbContext dbContext)
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
}
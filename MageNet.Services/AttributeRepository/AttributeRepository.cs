using System.ComponentModel.DataAnnotations;
using MageNet.Persistence;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.AttributeBuilder;
using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.AttributeValidator;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository;

public class AttributeRepository : IAttributeRepository
{
    private readonly MageNetDbContext _dbContext;
    private readonly IAttributeBuilder _attributeBuilder;
    private readonly IAttributeValidator _attributeValidator;

    public AttributeRepository(MageNetDbContext dbContext,
        IAttributeValidator attributeValidator)
    {
        _dbContext = dbContext;
        _attributeBuilder = new AttributeBuilder.AttributeBuilder(dbContext);
        _attributeValidator = attributeValidator;
    }

    public IEnumerable<AttributeWithData> GetAttributes()
    {
        var attributesWithoutDetails = _dbContext.Attributes.ToArray();
        return attributesWithoutDetails.Select(x => _attributeBuilder.GetAttributeWithData(x));
    }

    public AttributeWithData GetAttributeById(Guid guid)
    {
        var attributeWithoutDetails = _dbContext.Attributes.SingleOrDefault(x => x.AttributeId == guid);
        if (attributeWithoutDetails != null)
        {
            return _attributeBuilder.GetAttributeWithData(attributeWithoutDetails);
        }
        else
        {
            throw new ArgumentException($"Attribute with id '{guid}' does not exist");
        }
    }

    public Guid CreateNewAttribute(PostAttributeWithData postAttributeWithData)
    {
        var attributeWithData = new AttributeWithData()
        {
            AttributeName = postAttributeWithData.AttributeName,
            AttributeType = postAttributeWithData.AttributeType,
            DefaultLiteralValue = postAttributeWithData.DefaultLiteralValue,
            EntityId = postAttributeWithData.EntityId,
            IsMultipleSelect = postAttributeWithData.IsMultipleSelect,
            SelectableOptions = postAttributeWithData.SelectableOptions?
                .Select(x => new SelectableAttributeValue
                {
                    IsDefaultValue = x.IsDefaultValue,
                    Value = x.Value
                })
        };

        (bool isValid, IEnumerable<ValidationException> exceptions) =
            _attributeValidator.CheckAttributeValidity(attributeWithData);

        if (isValid)
        {
            return _attributeBuilder.CreateAttributeWithData(attributeWithData);
        }

        else
        {
            var msg = "Validation Error. The following issues have been discovered:\n";
            var exceptionMsg = string.Join("\n", exceptions.Select(x => x.Message));
            throw new ValidationException(msg + exceptionMsg);
        }
    }

    public AttributeWithData UpdateAttribute(AttributeWithData updatedAttributeWithData)
    {
        updatedAttributeWithData = FillMissingProperties(updatedAttributeWithData);

        (bool isValid, IEnumerable<ValidationException> exceptions) =
            _attributeValidator.CheckAttributeValidity(updatedAttributeWithData);

        if (isValid)
        {
            return _attributeBuilder.UpdateAttributeWithData(updatedAttributeWithData);
        }

        else
        {
            var msg = "Validation Error. The following issues have been discovered:\n";
            var exceptionMsg = string.Join(",", exceptions);
            throw new ValidationException(msg + exceptionMsg);
        }
    }

    private AttributeWithData FillMissingProperties(AttributeWithData updatedAttributeWithData)
    {
        var trackedAttributeWithData = GetAttributeById(updatedAttributeWithData.AttributeId);
        if (updatedAttributeWithData.AttributeType != null)
        {
            trackedAttributeWithData.AttributeType = updatedAttributeWithData.AttributeType;
        }

        if (updatedAttributeWithData.AttributeName != null)
        {
            trackedAttributeWithData.AttributeName = updatedAttributeWithData.AttributeName;
        }

        if (updatedAttributeWithData.SelectableOptions != null &&
            trackedAttributeWithData.AttributeType == AttributeType.Selectable)
        {
            trackedAttributeWithData.SelectableOptions = updatedAttributeWithData.SelectableOptions;
        }

        if (updatedAttributeWithData.DefaultLiteralValue != null &&
            trackedAttributeWithData.AttributeType != AttributeType.Selectable)
        {
            trackedAttributeWithData.DefaultLiteralValue = updatedAttributeWithData.DefaultLiteralValue;
        }

        if (updatedAttributeWithData.IsMultipleSelect != null &&
            trackedAttributeWithData.AttributeType == AttributeType.Selectable)
        {
            trackedAttributeWithData.IsMultipleSelect = updatedAttributeWithData.IsMultipleSelect;
        }

        return trackedAttributeWithData;
    }


    public void DeleteAttributeById(Guid guid)
    {
        using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
        {
            var attributeToRemove = _dbContext.Attributes.SingleOrDefault(x => x.AttributeId == guid);
            if (attributeToRemove == null)
            {
                throw new ArgumentException(
                    $"Unable to delete the attribute. The attribute with id '{guid}' does not exist");
            }

            _dbContext.Attributes.Remove(attributeToRemove);
            switch (attributeToRemove.AttributeType)
            {
                case AttributeType.Price:
                    var priceAttributeDataToRemove =
                        _dbContext.PriceAttributes.SingleOrDefault(x => x.AttributeId == guid);
                    if (priceAttributeDataToRemove != null)
                    {
                        _dbContext.PriceAttributes.Remove(priceAttributeDataToRemove);
                    }

                    break;

                case AttributeType.Text:
                    var textAttributeDataToRemove =
                        _dbContext.TextAttributes.SingleOrDefault(x => x.AttributeId == guid);
                    if (textAttributeDataToRemove != null)
                    {
                        _dbContext.TextAttributes.Remove(textAttributeDataToRemove);
                    }

                    break;

                case AttributeType.Selectable:
                    var selectableAttributeDataToRemove =
                        _dbContext.SelectableAttributes.SingleOrDefault(x => x.AttributeId == guid);
                    if (selectableAttributeDataToRemove != null)
                    {
                        _dbContext.SelectableAttributes.Remove(selectableAttributeDataToRemove);
                    }

                    break;

                default:
                    dbContextTransaction.Rollback();
                    throw new ArgumentException(
                        $"Unable to delete attribute data. Unknown attribute type: '{attributeToRemove.AttributeType.ToString()}'");
            }

            _dbContext.SaveChanges();

            dbContextTransaction.Commit();
        }
    }
}
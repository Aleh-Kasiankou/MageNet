using System.Text.Json.Serialization;
using MageNet.Persistence.Models;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNetServices.Exceptions;
using MageNetServices.Extensions;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO.Attributes;

public class Attribute : IAttribute
{
    private readonly IAttributeTypeFactory _attributeTypeFactory;
    private readonly IAttributeValidator _attributeValidator;

    private Attribute(IAttributeTypeFactory attributeTypeFactory)
    {
        _attributeTypeFactory = attributeTypeFactory;
        // empty constructor for conversion from attribute entity saved in the database
        // attribute is assumed to be valid so validator is not required
    }

    public Attribute(IAttributeValidator attributeValidator, IAttributeTypeFactory attributeTypeFactory)
    {
        _attributeValidator = attributeValidator;
        _attributeTypeFactory = attributeTypeFactory;
    }

    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; }
    public IAttributeTypeBearer AttributeType { get; set; }

    [JsonIgnore] public virtual Entity Entity { get; set; }
    public Guid EntityId { get; set; }

    public IAttributeWithData JoinWithSavedData()
    {
        return AttributeType.JoinWithData(this);
    }

    public Guid CreateNewDbEntry(IPostAttributeWithData postAttributeWithData)
    {
        var attributeWithData = postAttributeWithData.MapToAttributeWithData();
        var validationResult = _attributeValidator.CheckAttributeValidity(attributeWithData);

        if (validationResult.isValid)
        {
            return AttributeType.CreateNewDbEntry(attributeWithData);
        }

        var validationErrorMsg = string.Join(",", validationResult.validationErrors.Select(e => e.Message));

        throw new AggregateAttributeValidationException(validationErrorMsg);
    }

    public void Update(IPutAttributeWithData putAttributeWithData)
    {
        var savedAttributeWithData = JoinWithSavedData();

        if (putAttributeWithData.AttributeType != null &&
            putAttributeWithData.AttributeType != AttributeType.AttributeType)
        {
            UpdateAttributeDataWithTypeChange(savedAttributeWithData, putAttributeWithData);
        }

        else
        {
            UpdateAttributeData(savedAttributeWithData, putAttributeWithData);
        }
    }

    private void UpdateAttributeDataWithTypeChange(IAttributeWithData savedAttributeWithData,
        IPutAttributeWithData putAttributeWithData)
    {
        if (putAttributeWithData.AttributeType != null)
        {
            AttributeType.RemoveDbData(AttributeId);
            savedAttributeWithData.AttributeType = (AttributeType)putAttributeWithData.AttributeType;
            AttributeType = _attributeTypeFactory.CreateAttributeType(savedAttributeWithData.AttributeType);
            // need change attribute type property


            var updatedAttribute = AttributeType.RemoveIrrelevantProperties(savedAttributeWithData);
            UpdateAttributeData(updatedAttribute, putAttributeWithData, true);
            return;
        }

        throw new InvalidOperationException("Attribute Type cannot be changed because new attribute type is null");
    }

    private void UpdateAttributeData(IAttributeWithData savedAttributeWithData,
        IPutAttributeWithData putAttributeWithData, bool typeIsChanged = false)
    {
        var updatedAttribute = putAttributeWithData.MapToAttributeWithData(savedAttributeWithData);
        
        var validationResult = _attributeValidator.CheckAttributeValidity(updatedAttribute);

       if (validationResult.isValid)
           
           
        {
            AttributeType.UpdateAttributeData(updatedAttribute, typeIsChanged);

           return;
        }

        var validationErrorMsg = string.Join(",", validationResult.validationErrors.Select(e => e.Message));

        throw new AggregateAttributeValidationException(validationErrorMsg);
    }

    public static Attribute CreateAttributeWithoutValidation(Guid attributeId, IAttributeTypeBearer attributeType,
        string attributeName, Guid entityId, IAttributeTypeFactory attributeTypeFactory)
    {
        return new Attribute(attributeTypeFactory)
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = attributeType,
            EntityId = entityId
        };
    }
}
using System.Text.Json.Serialization;
using MageNet.Persistence.Models;
using MageNetServices.Exceptions;
using MageNetServices.Extensions;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO.Attributes;

public class Attribute : IAttribute
{
    private readonly IAttributeValidator _attributeValidator;

    private Attribute()
    {
        // empty constructor for conversion from attribute entity saved in the database
        // attribute is assumed to be valid so validator is not required
    }
    
    public Attribute(IAttributeValidator attributeValidator)
    {
        _attributeValidator = attributeValidator;
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

    public Guid SaveToDb(IPostAttributeWithData postAttributeWithData)
    {
        var attributeWithData = postAttributeWithData.MapToAttributeWithData();
        var validationResult = _attributeValidator.CheckAttributeValidity(attributeWithData);

        if (validationResult.Item1)
        {
            return AttributeType.SaveToDb(attributeWithData);

        }

        var validationErrorMsg = string.Join(",", validationResult.Item2.Select(e => e.Message));

        throw new AggregateAttributeValidationException(validationErrorMsg);
    }

    public static Attribute CreateAttributeWithoutValidation(Guid attributeId, IAttributeTypeBearer attributeType, string attributeName, Guid entityId)
    {
        return new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = attributeType,
            EntityId = entityId
        };
    }
}
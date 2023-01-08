using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNetServices.AttributeRepository.DTO;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeValidator;

public class AttributeValidator : IAttributeValidator
{
    private List<ValidationException> Exceptions { get; set; } = new();

    public (bool, IEnumerable<ValidationException>) CheckAttributeValidity(IAttributeWithData attributeWithData)
    {
        var isValid = false;

        var testcases = typeof(AttributeValidator).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.ReturnType == typeof(IEnumerable<ValidationException>))
            .ToList();

        foreach (var testcase in testcases)
        {
            var exceptions = testcase.Invoke(this, new object[] { attributeWithData });
            if (exceptions != null)
            {
                Exceptions.AddRange(exceptions as List<ValidationException> ??
                                    throw new InvalidOperationException(
                                        $"Validation Internal Error: '{testcase.Name} testcase'"));
            }
        }

        if (!Exceptions.Any())
        {
            isValid = true;
        }

        return (isValid, Exceptions);
    }

    private IEnumerable<ValidationException> CheckRequiredFields(AttributeWithData attributeWithData)
    {
        var exceptions = new List<ValidationException>();

        if (attributeWithData.AttributeName == null || string.IsNullOrWhiteSpace(attributeWithData.AttributeName))
        {
            exceptions.Add(new ValidationException("Attribute Name cannot be null or empty"));
        }

        if (attributeWithData.SelectableOptions == null && attributeWithData.AttributeType == AttributeType.Selectable)
        {
            exceptions.Add(
                new ValidationException(
                    "Options of a selectable attribute cannot be null. Send empty list of options instead"));
        }

        if (attributeWithData.DefaultLiteralValue == null &&
            attributeWithData.AttributeType != AttributeType.Selectable)
        {
            exceptions.Add(new ValidationException("All non-selectable attributes must have a default value"));
        }

        if (attributeWithData.IsMultipleSelect == null && attributeWithData.AttributeType == AttributeType.Selectable)
        {
            exceptions.Add(
                new ValidationException("All selectable attributes must be either single-select or multiselect"));
        }

        return exceptions;
    }

    private IEnumerable<ValidationException> CheckSelectableAttributeOverPosting(AttributeWithData attributeWithData)
    {
        var exceptions = new List<ValidationException>();

        if (attributeWithData.AttributeType == AttributeType.Selectable)
        {
            if (attributeWithData.DefaultLiteralValue != null)
            {
                exceptions.Add(
                    new ValidationException("Selectable attributes cannot have a default literal value"));
            }
        }

        return exceptions;
    }

    private IEnumerable<ValidationException> CheckNonSelectableAttributeOverPosting(AttributeWithData attributeWithData)
    {
        var exceptions = new List<ValidationException>();

        if (attributeWithData.AttributeType != AttributeType.Selectable)
        {
            if (attributeWithData.SelectableOptions != null)
            {
                exceptions.Add(
                    new ValidationException("Non-selectable attributes cannot have options"));
            }

            if (attributeWithData.IsMultipleSelect != null)
            {
                exceptions.Add(
                    new ValidationException("Non-selectable attributes cannot be multiselect or single-select"));
            }
        }


        return exceptions;
    }
}
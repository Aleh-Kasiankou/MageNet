using System.ComponentModel.DataAnnotations;

namespace MageNetServices.Interfaces;

public interface IAttributeValidator
{
    public (bool isValid, IEnumerable<ValidationException> validationErrors) CheckAttributeValidity(
        IAttributeWithData attributeWithData);
}
using System.ComponentModel.DataAnnotations;

namespace MageNetServices.Interfaces;

public interface IAttributeValidator
{
    public (bool, IEnumerable<ValidationException>) CheckAttributeValidity(IAttributeWithData attributeWithData);
}
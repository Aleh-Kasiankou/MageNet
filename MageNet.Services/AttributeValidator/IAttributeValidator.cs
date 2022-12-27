using System.ComponentModel.DataAnnotations;
using MageNetServices.AttributeRepository.DTO;
using MageNetServices.AttributeRepository.DTO.Attributes;

namespace MageNetServices.AttributeValidator;

public interface IAttributeValidator
{
    public (bool, IEnumerable<ValidationException>) CheckAttributeValidity(AttributeWithData attributeWithData);
}
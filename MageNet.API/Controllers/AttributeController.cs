using MageNetServices.AttributeRepository;
using MageNetServices.AttributeRepository.DTO;
using MageNetServices.AttributeRepository.DTO.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace MageNet.Controllers;

[ApiController]
[Route("attribute")]
public class AttributeController : ControllerBase
{
    private readonly IAttributeRepository _attributeRepository;

    public AttributeController(IAttributeRepository attributeRepository)
    {
        _attributeRepository = attributeRepository;
    }

    [HttpGet]
    public IEnumerable<AttributeWithData> GetProductAttributes()
    {
        return _attributeRepository.GetAttributes();
    }

    [HttpGet("{guid:guid}")]
    public AttributeWithData GetProductAttributeById(Guid guid)
    {
        return _attributeRepository.GetAttributeById(guid);
    }

    [HttpPost]
    public IActionResult PostAttribute([FromBody] PostAttributeWithData attributeWithData)
    {
        return Ok(_attributeRepository.CreateNewAttribute(attributeWithData));
    }

    

    [HttpPut]
    public IActionResult PutProductAttribute([FromBody] AttributeWithData updatedAttributeWithData)
    {
        return Ok(_attributeRepository.UpdateAttribute(updatedAttributeWithData));
    }

    

    [HttpDelete("{guid:guid}")]
    public IActionResult DeleteProductAttribute([FromRoute] Guid guid)
    {
        _attributeRepository.DeleteAttributeById(guid);
        return NoContent();
    }
}
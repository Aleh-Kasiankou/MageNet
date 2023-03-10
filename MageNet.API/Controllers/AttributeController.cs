using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.Interfaces;
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
    public IActionResult GetProductAttributes()
    {
        try
        {
            return Ok(_attributeRepository.GetAttributes());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{guid:guid}")]
    public IActionResult GetProductAttributeById(Guid guid)
    {
        try
        {
            return Ok(_attributeRepository.GetAttributeById(guid));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public IActionResult PostAttribute([FromBody] PostAttributeWithData attributeWithData)
    {
        try
        {
            return Ok(_attributeRepository.CreateNewAttribute(attributeWithData));
        }

        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpPut]
    public IActionResult PutProductAttribute([FromBody] PutAttributeWithData putAttributeWithData)
    {
        try
        {
            _attributeRepository.UpdateAttribute(putAttributeWithData);
            return Ok();
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpDelete("{guid:guid}")]
    public IActionResult DeleteProductAttribute([FromRoute] Guid guid)
    {
        try
        {
            _attributeRepository.DeleteAttributeById(guid);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}
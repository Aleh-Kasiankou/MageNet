using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MageNet.Controllers;

[ApiController]
[Route("attribute")]
[Authorize]
public class AttributeController : ControllerBase
{
    private readonly IAttributeRepository _attributeRepository;

    public AttributeController(IAttributeRepository attributeRepository)
    {
        _attributeRepository = attributeRepository;
    }

    [HttpGet]
    [Authorize]
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
    [Authorize]
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
    [Authorize]
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
    [Authorize]
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
    [Authorize]
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
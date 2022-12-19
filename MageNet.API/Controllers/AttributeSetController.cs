using MageNet.Models;
using MageNet.Persistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace MageNet.Controllers;

[ApiController]
[Route("AttributeSet")]
public class AttributeSetController : ControllerBase
{
    [HttpGet]
    public IEnumerable<AttributeSet> GetAttributeSets()
    {
        return new AttributeSet[10];
    }
    
    [HttpPost]
    public IActionResult PostAttributeSet([FromBody]CreateAttributeSetRequest createRequest)
    {
        return Ok(Guid.NewGuid());
    }
    
    [HttpPut]
    public IActionResult PutAttributeSet([FromBody] UpdateAttributeSetRequest updateRequest)
    {
        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteAttributeSet([FromRoute] Guid id)
    {
        return NoContent();
    }
}
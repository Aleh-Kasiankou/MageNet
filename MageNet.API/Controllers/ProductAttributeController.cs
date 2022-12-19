using MageNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace MageNet.Controllers;
[ApiController]
[Route("product_attribute")]
public class ProductAttributeController : ControllerBase
{
    [HttpGet]
    public IEnumerable<ProducesAttribute> GetProductAttributes()
    {
        return new ProducesAttribute[10];
    }
    
    [HttpPost]
    public IActionResult PostProductAttribute([FromBody]CreateProductAttributeRequest createRequest)
    {
        return Ok(Guid.NewGuid());
    }
    
    [HttpPut]
    public IActionResult PutProductAttribute([FromBody] UpdateProductAttributeRequest updateRequest)
    {
        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProductAttribute([FromRoute] Guid id)
    {
        return NoContent();
    }
}
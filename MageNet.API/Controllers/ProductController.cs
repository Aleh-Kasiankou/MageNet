using MageNet.Models;
using MageNet.Persistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace MageNet.Controllers;

[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Product> GetProducts()
    {
        return new Product[10];
    }
    
    [HttpPost]
    public IActionResult PostProduct([FromBody]CreateProductRequest createRequest)
    {
        return Ok(Guid.NewGuid());
    }
    
    [HttpPut]
    public IActionResult PutProduct([FromBody] UpdateProductRequest updateRequest)
    {
        return Ok();
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProduct([FromRoute] Guid id)
    {
        return NoContent();
    }
}
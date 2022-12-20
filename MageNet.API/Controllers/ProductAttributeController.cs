using MageNet.Models.Attributes.Price;
using MageNet.Models.Attributes.Selectable;
using MageNet.Models.Attributes.Text;
using Microsoft.AspNetCore.Mvc;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNet.Controllers;
[ApiController]
[Route("product_attribute")]
public class ProductAttributeController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Attribute> GetProductAttributes()
    {
        return new Attribute[10];
    }

    private const string PriceRoute = "price";
    private const string TextRoute = "text";
    private const string SelectableRoute = "selectable";
    
    [HttpPost(PriceRoute)]
    public IActionResult PostProductPriceAttribute([FromBody]CreateProductPriceAttributeRequest createRequest)
    {
        return Ok(Guid.NewGuid());
    }
    
    [HttpPost(TextRoute)]
    public IActionResult PostProductTextAttribute([FromBody]CreateProductTextAttributeRequest createRequest)
    {
        return Ok(Guid.NewGuid());
    }
    
    [HttpPost(SelectableRoute)]
    public IActionResult PostProductSelectableAttribute([FromBody]CreateProductSelectableAttribute createRequest)
    {
        return Ok(Guid.NewGuid());
    }
    
    [HttpPut(PriceRoute)]
    public IActionResult PutProductAttribute([FromBody] UpdateProductPriceAttributeRequest updateRequest)
    {
        return Ok();
    }
    
    [HttpPut(TextRoute)]
    public IActionResult PutProductAttribute([FromBody] UpdateProductTextAttributeRequest updateRequest)
    {
        return Ok();
    }
    
    [HttpPut(SelectableRoute)]
    public IActionResult PutProductAttribute([FromBody] UpdateProductSelectableAttributeRequest updateRequest)
    {
        return Ok();
    }
    
  
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProductAttribute([FromRoute] Guid id)
    {
        return NoContent();
    }
}
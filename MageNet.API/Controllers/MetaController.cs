using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using Microsoft.AspNetCore.Mvc;

namespace MageNet.Controllers;

[ApiController]
[Route("meta")]
public class MetaController : ControllerBase
{
    [HttpGet("attribute-type")]
    public IEnumerable<string> AttributeTypes()
    {
        var values = Enum.GetValues(typeof(AttributeType));
        List<string> textValues = new();
        
        foreach (var value in values)
        {
            if (value != null)
            {
                textValues.Add(value.ToString());
            }
        }

        return textValues;
    }
    
    
}
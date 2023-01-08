using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;

namespace MageNetServices.AttributeRepository.DTO;

public class PriceAttributeDataRepo : IAttributeDataRepository<PriceAttributeData>
{
    private readonly MageNetDbContext _dbContext;

    public PriceAttributeDataRepo(MageNetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public PriceAttributeData GetAttributeData(Guid attributeId)
    {
        var priceAttributeData = _dbContext.PriceAttributes.SingleOrDefault(x => x.AttributeId == attributeId);
        return priceAttributeData ?? throw new AttributeNotFoundException();
    }

    public void CreateAttributeData(IAttributeData attributeData)
    {
        _dbContext.PriceAttributes.Add(attributeData as PriceAttributeData
                                       ?? throw new InvalidAttributeDataException(
                                           $"Attribute Data cannot be safely casted to Price Attribute Data"));
    }

    public void UpdateAttributeData(IAttributeData attributeData)
    {
        // if attribute type didn't change, it is enough to map changed props. 
        throw new NotImplementedException();

        // if attribute type changed, it is necessary to remove the existing data,
        // update attribute type, and use new DataRepo To Create Data.
    }
}
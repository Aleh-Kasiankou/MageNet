using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;

namespace MageNetServices.AttributeRepository.TypedDataRepositories;

public class PriceAttributeDataRepo : AttributeDataRepo<PriceAttributeData>
{
    private readonly MageNetDbContext _dbContext;

    public PriceAttributeDataRepo(MageNetDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override PriceAttributeData GetAttributeData(Guid attributeId)
    {
        var priceAttributeData = _dbContext.PriceAttributes.SingleOrDefault(x => x.AttributeId == attributeId);
        return priceAttributeData ?? throw new AttributeNotFoundException();
    }

    public override void CreateAttributeData(IAttributeData attributeData)
    {
        _dbContext.PriceAttributes.Add(attributeData as PriceAttributeData
                                       ?? throw new InvalidAttributeDataException(
                                           $"Attribute Data cannot be safely casted to Price Attribute Data"));
    }

    public override void UpdateAttributeData(IAttributeData attributeData)
    {
        _dbContext.PriceAttributes.Update(attributeData as PriceAttributeData ??
                                          throw new InvalidOperationException(
                                              $"Attribute Data cannot be safely casted to Price Attribute Data"));
    }

    public override void DeleteAttributeData(Guid attributeId)
    {
        _dbContext.PriceAttributes
            .Remove(_dbContext.PriceAttributes
                .Single(x => x.AttributeId == attributeId));
    }
}
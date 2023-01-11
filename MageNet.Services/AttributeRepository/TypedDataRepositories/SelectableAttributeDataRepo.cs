using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using Microsoft.EntityFrameworkCore;

namespace MageNetServices.AttributeRepository.TypedDataRepositories;

public class SelectableAttributeDataRepo : AttributeDataRepo<SelectableAttributeData>
{
    private readonly MageNetDbContext _dbContext;

    public SelectableAttributeDataRepo(MageNetDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override SelectableAttributeData GetAttributeData(Guid attributeId)
    {
        var selectableAttributeData = _dbContext.SelectableAttributes
            .Include(x => x.Values)
            .SingleOrDefault(x => x.AttributeId == attributeId);

        return selectableAttributeData ?? throw new AttributeNotFoundException();
    }


    public override void CreateAttributeData(IAttributeData attributeData)
    {
        _dbContext.SelectableAttributes.Add(attributeData as SelectableAttributeData
                                            ?? throw new InvalidAttributeDataException(
                                                $"Attribute Data cannot be safely casted to Selectable Attribute Data"));
    }

    public override void UpdateAttributeData(IAttributeData attributeData)
    {
        _dbContext.SelectableAttributes.Update(attributeData as SelectableAttributeData ??
                                          throw new InvalidOperationException(
                                              $"Attribute Data cannot be safely casted to Selectable Attribute Data"));
    }

    public override void DeleteAttributeData(Guid attributeId)
    {
        _dbContext.SelectableAttributes
            .Remove(_dbContext.SelectableAttributes
                .Single(x => x.AttributeId == attributeId));
    }
}
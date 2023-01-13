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
        var selectableAttributeData = _dbContext.SelectableAttributes.AsNoTracking()
            .Include(x => x.Values).AsNoTracking()
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
        var selectableAttributeData = attributeData as SelectableAttributeData ??
                                      throw new InvalidOperationException(
                                          $"Attribute Data cannot be safely casted to Selectable Attribute Data");

        
        var savedSelectableAttributeValues = _dbContext.SelectableAttributeValues.AsNoTracking().Where(x =>
            x.AttributeId == selectableAttributeData.SelectableAttributeId).ToList();
        
        foreach (var option in savedSelectableAttributeValues)
        {
            if (selectableAttributeData.Values.All(x =>
                    x.SelectableAttributeValueId != option.SelectableAttributeValueId))
            {
                _dbContext.SelectableAttributeValues.Remove(option);
            }
        }
        // child entities are not tracked due to the mappings, so it is necessary to remove them manually

        _dbContext.SelectableAttributes.Update(selectableAttributeData);
    }

    public override void DeleteAttributeData(Guid attributeId)
    {
        _dbContext.SelectableAttributes
            .Remove(_dbContext.SelectableAttributes
                .Single(x => x.AttributeId == attributeId));
    }
}
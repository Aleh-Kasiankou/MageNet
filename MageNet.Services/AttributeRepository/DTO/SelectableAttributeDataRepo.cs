using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using Microsoft.EntityFrameworkCore;

namespace MageNetServices.AttributeRepository.DTO;

public class SelectableAttributeDataRepo : IAttributeDataRepository<SelectableAttributeData>
{
    private readonly MageNetDbContext _dbContext;

    public SelectableAttributeDataRepo(MageNetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public SelectableAttributeData GetAttributeData(Guid attributeId)
    {
        var selectableAttributeData = _dbContext.SelectableAttributes
            .Include(x => x.Values)
            .SingleOrDefault(x => x.AttributeId == attributeId);

        return selectableAttributeData ?? throw new AttributeNotFoundException();
    }

    public void CreateAttributeData(IAttributeData attributeData)
    {
        _dbContext.SelectableAttributes.Add(attributeData as SelectableAttributeData
                                            ?? throw new InvalidAttributeDataException(
                                                $"Attribute Data cannot be safely casted to Selectable Attribute Data"));

        _dbContext.SaveChanges();
    }

    public void UpdateAttributeData(IAttributeData attributeData)
    {
        throw new NotImplementedException();
    }
}
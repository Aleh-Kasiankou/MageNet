using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO;

public abstract class AttributeDataRepo<T> : IAttributeDataRepository<T> where T : IAttributeData
{
    private readonly MageNetDbContext _dbContext;
    public AttributeDataRepo(MageNetDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public abstract T GetAttributeData(Guid attributeId);

    public virtual Guid CreateAttribute(IAttributeEntity attributeEntity)
    {
        _dbContext.Attributes.Add(attributeEntity as AttributeEntity ??
                                  throw new InvalidAttributeDataException(
                                      "Provided attribute doesn't meet required attribute signature"));

        return attributeEntity.AttributeId;
    }

    public abstract void CreateAttributeData(IAttributeData attributeData);

    public abstract void UpdateAttributeData(IAttributeData attributeData);
    
    public virtual void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}
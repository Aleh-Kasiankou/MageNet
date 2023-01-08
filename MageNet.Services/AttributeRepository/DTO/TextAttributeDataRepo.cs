using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;

namespace MageNetServices.AttributeRepository.DTO;

public class TextAttributeDataRepo : IAttributeDataRepository<TextAttributeData>
{
    private readonly MageNetDbContext _dbContext;

    public TextAttributeDataRepo(MageNetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public TextAttributeData GetAttributeData(Guid attributeId)
    {
        var textAttributeData = _dbContext.TextAttributes.SingleOrDefault(x => x.AttributeId == attributeId);

        return textAttributeData ?? throw new AttributeNotFoundException();
    }

    public void CreateAttributeData(IAttributeData attributeData)
    {
        _dbContext.TextAttributes.Add(attributeData as TextAttributeData
                                       ?? throw new InvalidAttributeDataException(
                                           $"Attribute Data cannot be safely casted to Text Attribute Data"));
    }

    public void UpdateAttributeData(IAttributeData attributeData)
    {
        throw new NotImplementedException();
    }
}
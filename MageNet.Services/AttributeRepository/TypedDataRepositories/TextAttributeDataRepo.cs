using MageNet.Persistence;
using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;

namespace MageNetServices.AttributeRepository.TypedDataRepositories;

public class TextAttributeDataRepo : AttributeDataRepo<TextAttributeData>
{
    private readonly MageNetDbContext _dbContext;

    public TextAttributeDataRepo(MageNetDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override TextAttributeData GetAttributeData(Guid attributeId)
    {
        var textAttributeData = _dbContext.TextAttributes.SingleOrDefault(x => x.AttributeId == attributeId);

        return textAttributeData ?? throw new AttributeNotFoundException();
    }


    public override void CreateAttributeData(IAttributeData attributeData)
    {
        _dbContext.TextAttributes.Add(attributeData as TextAttributeData
                                      ?? throw new InvalidAttributeDataException(
                                          $"Attribute Data cannot be safely casted to Text Attribute Data"));
    }

    public override void UpdateAttributeData(IAttributeData attributeData)
    {
        _dbContext.TextAttributes.Update(attributeData as TextAttributeData ??
                                         throw new InvalidOperationException(
                                             $"Attribute Data cannot be safely casted to Text Attribute Data"));
    }

    public override void DeleteAttributeData(Guid attributeId)
    {
        _dbContext.TextAttributes.Remove(_dbContext.TextAttributes.Single(x => x.AttributeId == attributeId));
    }
}
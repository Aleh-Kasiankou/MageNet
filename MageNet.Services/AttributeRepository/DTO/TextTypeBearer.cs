using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.Extensions;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO;

public class TextTypeBearer: IAttributeTypeBearer
{
    private readonly IAttributeDataRepository<TextAttributeData> _dataRepository;
    private readonly IAttributeTypeFactory _attributeTypeFactory;


    public TextTypeBearer(IAttributeDataRepository<TextAttributeData> dataRepository,
        IAttributeTypeFactory attributeTypeFactory)
    {
        _dataRepository = dataRepository;
        _attributeTypeFactory = attributeTypeFactory;
        AttributeType = AttributeType.Text;
    }

    public AttributeType AttributeType { get; }

    public Guid SaveToDb(IPostAttributeWithData postAttributeWithData)
    {
        var (attributeEntity, attributeData) =
            DecoupleAttributeWithData(postAttributeWithData.MapToAttributeWithData());

        var attributeId = _dataRepository.CreateAttribute(attributeEntity);
        attributeData.AttributeId = attributeId;

        
        _dataRepository.CreateAttributeData(attributeData);
        _dataRepository.SaveChanges();

        return attributeId;
    }

    public IAttributeWithData JoinWithData(IAttribute attribute)
    {
        var attributeData = _dataRepository.GetAttributeData(attribute.AttributeId);

        return new AttributeWithData()
        {
            AttributeId = attribute.AttributeId,
            EntityId = attribute.EntityId,
            AttributeName = attribute.AttributeName,
            AttributeType = attribute.AttributeType.AttributeType,
            DefaultLiteralValue = attributeData.DefaultValue,
            SelectableOptions = null,
            IsMultipleSelect = null
        };
    }

    public void UpdateAttributeData(IAttributeWithData attributeData)
    {
        throw new NotImplementedException();
    }

    public (IAttributeEntity, IAttributeData) DecoupleAttributeWithData(IAttributeWithData attributeWithData)
    {
        var attribute = new AttributeEntity()
        {
            AttributeId = attributeWithData.AttributeId,
            AttributeName = attributeWithData.AttributeName,
            AttributeType = attributeWithData.AttributeType,
            EntityId = attributeWithData.EntityId,
        };

        var attributeData = new TextAttributeData
        {
            AttributeId = attributeWithData.AttributeId,
            DefaultValue = attributeWithData.DefaultLiteralValue ?? ""
        };

        return (attribute, attributeData);
    }
}

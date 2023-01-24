using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.TypeBearers;

public class TextTypeBearer : IAttributeTypeBearer
{
    private readonly IAttributeDataRepository<TextAttributeData> _dataRepository;


    public TextTypeBearer(IAttributeDataRepository<TextAttributeData> dataRepository)
    {
        _dataRepository = dataRepository;
        AttributeType = AttributeType.Text;
    }

    public AttributeType AttributeType { get; }

    public Guid CreateNewDbEntry(IAttributeWithData attributeWithData)
    {
        var (attributeEntity, attributeData) =
            DecoupleAttributeWithData(attributeWithData);

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

    public void UpdateAttributeData(IAttributeWithData attributeWithData, bool typeIsChanged)
    {
        var (attributeEntity, attributeData) = DecoupleAttributeWithData(attributeWithData);
        _dataRepository.UpdateAttribute(attributeEntity);

        if (typeIsChanged)
        {
            _dataRepository.CreateAttributeData(attributeData);
        }

        else
        {
            _dataRepository.UpdateAttributeData(attributeData);
        }

        _dataRepository.SaveChanges();
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

    public void RemoveDbData(Guid attributeId)
    {
        _dataRepository.DeleteAttributeData(attributeId);
    }

    public IAttributeWithData RemoveIrrelevantProperties(IAttributeWithData attributeWithData)
    {
        attributeWithData.SelectableOptions = null;
        attributeWithData.IsMultipleSelect = null;
        return attributeWithData;
    }
}
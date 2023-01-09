using MageNet.Persistence.Exceptions;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.Interfaces;

namespace MageNetServices.AttributeRepository.DTO;

public class AttributeTypeFactory : IAttributeTypeFactory
{

    private readonly IAttributeDataRepository<PriceAttributeData> _priceDataRepository;
    private readonly IAttributeDataRepository<TextAttributeData> _textDataRepository;
    private readonly IAttributeDataRepository<SelectableAttributeData> _selectableDataRepository;

    public AttributeTypeFactory(IAttributeDataRepository<PriceAttributeData> priceDataRepository, IAttributeDataRepository<TextAttributeData> textDataRepository, IAttributeDataRepository<SelectableAttributeData> selectableDataRepository)
    {
        _priceDataRepository = priceDataRepository;
        _textDataRepository = textDataRepository;
        _selectableDataRepository = selectableDataRepository;
    }
    
    public IAttributeTypeBearer CreateAttributeType(AttributeType attributeType)
    {
        IAttributeTypeBearer attributeTypeBearer;

        switch (attributeType)
        {
            case AttributeType.Price:
                attributeTypeBearer = CreatePriceAttributeBearer();
                break;
            case AttributeType.Text:
                attributeTypeBearer = CreateTextAttributeBearer();
                break;
            case AttributeType.Selectable:
                attributeTypeBearer = CreateSelectableAttributeBearer();
                break;
            default:
                throw new AttributeTypeNotSupportedException(
                    "This attribute type is not yet supported. Please contact our support team to get the release date.");
        }

        return attributeTypeBearer;
    }

    private IAttributeTypeBearer CreatePriceAttributeBearer()
    {
        return new PriceTypeBearer(_priceDataRepository, this);
    }

    private IAttributeTypeBearer CreateTextAttributeBearer()
    {
        return new TextTypeBearer(_textDataRepository, this);
    }

    private IAttributeTypeBearer CreateSelectableAttributeBearer()
    {
        return new SelectableTypeBearer(_selectableDataRepository, this);
    }
}
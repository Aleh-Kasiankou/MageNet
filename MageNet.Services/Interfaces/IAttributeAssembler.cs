using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

namespace MageNetServices.Interfaces;

public interface IAttributeAssembler
{

    IAttributeWithData JoinAttributeWithData(IAttribute attribute, IAttributeData attributeData);
}
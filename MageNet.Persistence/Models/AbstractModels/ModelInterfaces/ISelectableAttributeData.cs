using MageNet.Persistence.Models.Attributes;

namespace MageNet.Persistence.Models.AbstractModels.ModelInterfaces;

public interface ISelectableAttributeData
{
    public Guid SelectableAttributeId { get; set; }

    public bool IsMultipleSelect { get; set; }
    public IEnumerable<SelectableAttributeOption> Values { get; set; }
}
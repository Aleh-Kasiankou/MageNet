﻿using MageNet.Persistence.AbstractModels.ModelInterfaces;

namespace MageNet.Persistence.Models;

public class AttributeSet
{
    public Guid AttributeSetId { get; set; }
    public string Name { get; set; }
    public Guid EntityId { get; set; }
    public Entity Entity { get; set; }
    public virtual IEnumerable<IAttribute<IEntity>> Attributes { get; set; }
}
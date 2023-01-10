using MageNet.Persistence.Models.AbstractModels.ModelInterfaces;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using MageNetServices.Interfaces;
using Attribute = MageNetServices.AttributeRepository.DTO.Attributes.Attribute;

namespace MageNetServices.Extensions;

public static class AttributeMapper
{
    public static IAttribute MapToIAttributeWithNoValidation(this IAttributeEntity attributeEntity,
        IAttributeTypeFactory typeFactory)
    {
        return Attribute.CreateAttributeWithoutValidation(
            attributeId: attributeEntity.AttributeId,
            attributeType: typeFactory.CreateAttributeType(attributeEntity.AttributeType),
            attributeName: attributeEntity.AttributeName,
            entityId: attributeEntity.EntityId, typeFactory);
    }

    public static IAttribute MapToIAttributeWithValidation(this IAttributeEntity attributeEntity,
        IAttributeTypeFactory typeFactory, IAttributeValidator attributeValidator)
    {
        return new Attribute(attributeValidator, typeFactory)
        {
            AttributeId = attributeEntity.AttributeId,
            AttributeType = typeFactory.CreateAttributeType(attributeEntity.AttributeType),
            AttributeName = attributeEntity.AttributeName,
            EntityId = attributeEntity.EntityId
        };
    }

    public static IAttributeWithData MapToAttributeWithData(this IPostAttributeWithData postAttributeWithData)
    {
        return new AttributeWithData()
        {
            AttributeName = postAttributeWithData.AttributeName,
            AttributeType = postAttributeWithData.AttributeType,
            DefaultLiteralValue = postAttributeWithData.DefaultLiteralValue,
            EntityId = postAttributeWithData.EntityId,
            IsMultipleSelect = postAttributeWithData.IsMultipleSelect,
            SelectableOptions = postAttributeWithData.SelectableOptions?.Select(x => new SelectableAttributeValue()
            {
                IsDefaultValue = x.IsDefaultValue,
                Value = x.Value
            }).ToArray()
        };
    }

    public static IAttributeWithData MapToAttributeWithData(this IPutAttributeWithData putAttributeWithData,
        IAttributeWithData savedAttributeWithData)
    {
        if (putAttributeWithData.AttributeName != null)
        {
            savedAttributeWithData.AttributeName = putAttributeWithData.AttributeName;
        }

        if (putAttributeWithData.EntityId != null)
        {
            savedAttributeWithData.EntityId = (Guid)putAttributeWithData.EntityId;
        }

        if (putAttributeWithData.DefaultLiteralValue != null)
        {
            savedAttributeWithData.DefaultLiteralValue = putAttributeWithData.DefaultLiteralValue;
        }

        if (putAttributeWithData.SelectableOptions != null)
        {
            // TODO Add new options if needed
            
            // find saved options
            var savedOptions = savedAttributeWithData.SelectableOptions;

            if (savedOptions != null)
            {
                // delete those options with to delete flag 
                var optionsToDeleteIds = putAttributeWithData
                    .SelectableOptions
                    .Where(x => x.IsToDelete);

                savedOptions =
                    savedOptions.Where(opt => optionsToDeleteIds
                        .All(x => x.OptionId != opt.SelectableAttributeValueId)).ToList();

                // map options that are not to be deleted

                var updateDataForSavedOptions = putAttributeWithData
                    .SelectableOptions
                    .Where(x => !x.IsToDelete).ToArray();

                foreach (var option in savedOptions)
                {
                    var optionUpdateData = updateDataForSavedOptions
                        .SingleOrDefault(x =>
                            x.OptionId == option.SelectableAttributeValueId);

                    if (optionUpdateData is { Value: { } }) option.Value = optionUpdateData.Value;
                    if (optionUpdateData is { IsDefaultValue: { } })
                        option.IsDefaultValue = (bool)optionUpdateData.IsDefaultValue;
                }

                savedAttributeWithData.SelectableOptions = savedOptions;
            }
        }

        if (putAttributeWithData.IsMultipleSelect != null)
        {
            savedAttributeWithData.IsMultipleSelect = putAttributeWithData.IsMultipleSelect;
        }

        return savedAttributeWithData;
    }
}
using System;
using System.Collections.Generic;
using MageNet.Persistence.Models;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeAssembler;
using MageNetServices.Exceptions;
using MageNetServices.Interfaces;
using Xunit;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNet.UnitTesting;

public class AttributeAssemblerTest
{
    [Fact]
    public void TestPriceAttributeAndPriceDataAssembled_OK()
    {
        // Arrange
        var assembler = new AttributeAssembler();

        var attributeId = Guid.NewGuid();
        var attributeName = "UnitTest";
        var entityId = Guid.NewGuid();
        var defaultDecimalValue = 26.345M;

        var priceAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = AttributeType.Price,
            EntityId = entityId,
            Entity = new Entity()
        };

        var priceAttributeData = new PriceAttribute()
        {
            AttributeId = attributeId,
            Attribute = priceAttribute,
            DefaultValue = defaultDecimalValue,
            PriceAttributeId = Guid.NewGuid()
        };

        // Act

        var attributeWithData = assembler.JoinAttributeWithData(priceAttribute, priceAttributeData);

        // Assert

        Assert.NotNull(attributeWithData);
        Assert.IsAssignableFrom<IAttributeWithData>(attributeWithData);
        Assert.Equal(attributeId, attributeWithData.AttributeId);
        Assert.Equal(attributeName, attributeWithData.AttributeName);
        Assert.Equal(AttributeType.Price, attributeWithData.AttributeType);
        Assert.Equal(entityId, attributeWithData.EntityId);
        Assert.Equal(defaultDecimalValue.ToString(), attributeWithData.DefaultLiteralValue);
        Assert.Null(attributeWithData.SelectableOptions);
        Assert.Null(attributeWithData.IsMultipleSelect);
    }

    [Fact]
    public void TestTextAttributeAndTextDataAssembled_OK()
    {
        // Arrange
        var assembler = new AttributeAssembler();

        var attributeId = Guid.NewGuid();
        var attributeName = "UnitTest";
        var entityId = Guid.NewGuid();
        var defaultTextValue = "TestValue";

        var textAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = AttributeType.Text,
            EntityId = entityId,
            Entity = new Entity()
        };

        var textAttributeData = new TextAttribute()
        {
            AttributeId = attributeId,
            Attribute = textAttribute,
            DefaultValue = defaultTextValue,
            TextAttributeId = Guid.NewGuid()
        };

        // Act

        var attributeWithData = assembler.JoinAttributeWithData(textAttribute, textAttributeData);

        // Assert

        Assert.NotNull(attributeWithData);
        Assert.IsAssignableFrom<IAttributeWithData>(attributeWithData);
        Assert.Equal(attributeId, attributeWithData.AttributeId);
        Assert.Equal(attributeName, attributeWithData.AttributeName);
        Assert.Equal(AttributeType.Text, attributeWithData.AttributeType);
        Assert.Equal(entityId, attributeWithData.EntityId);
        Assert.Equal(defaultTextValue, attributeWithData.DefaultLiteralValue);
        Assert.Null(attributeWithData.SelectableOptions);
        Assert.Null(attributeWithData.IsMultipleSelect);
    }

    [Fact]
    public void TestSelectableAttributeAndSelectableDataAssembled_OK()
    {
        // Arrange
        var assembler = new AttributeAssembler();

        var attributeId = Guid.NewGuid();
        var attributeName = "UnitTest";
        var entityId = Guid.NewGuid();
        var isMultipleSelect = true;


        var selectableAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = AttributeType.Selectable,
            EntityId = entityId,
            Entity = new Entity()
        };
        
        var optionOne = new SelectableAttributeValue()
        {
            AttributeId = attributeId,
            IsDefaultValue = true
        };

        var optionTwo = new SelectableAttributeValue()
        {
            AttributeId = attributeId,
            IsDefaultValue = false
        };

        var selectableAttributeData = new SelectableAttribute()
        {
            AttributeId = attributeId,
            Attribute = selectableAttribute,
            SelectableAttributeId = Guid.NewGuid(),
            IsMultipleSelect = isMultipleSelect,
            Values = new List<SelectableAttributeValue>(){ optionOne, optionTwo }
        };


        foreach (var option in selectableAttributeData.Values)
        {
            option.SelectableAttributeValueId = selectableAttributeData.SelectableAttributeId;
        }
         

        // Act

        var attributeWithData = assembler.JoinAttributeWithData(selectableAttribute, selectableAttributeData);

        // Assert

        Assert.NotNull(attributeWithData);
        Assert.IsAssignableFrom<IAttributeWithData>(attributeWithData);
        Assert.Equal(attributeId, attributeWithData.AttributeId);
        Assert.Equal(attributeName, attributeWithData.AttributeName);
        Assert.Equal(AttributeType.Selectable, attributeWithData.AttributeType);
        Assert.Equal(entityId, attributeWithData.EntityId);
        Assert.Null(attributeWithData.DefaultLiteralValue);
        Assert.NotNull(attributeWithData.SelectableOptions);
        Assert.NotNull(attributeWithData.IsMultipleSelect);
        Assert.Contains(optionOne, selectableAttributeData.Values);
        Assert.Contains(optionTwo, selectableAttributeData.Values);
    }


    [Fact]
    public void TestPriceAttributeAndTextDataAssembled_FAIL()
    {
        // Arrange
        var assembler = new AttributeAssembler();

        var attributeId = Guid.NewGuid();
        var attributeName = "UnitTest";
        var entityId = Guid.NewGuid();
        var defaultDecimalValue = (decimal)26.345;

        var priceAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = AttributeType.Price,
            EntityId = entityId,
            Entity = new Entity()
        };

        var textAttributeData = new TextAttribute()
        {
            AttributeId = attributeId,
            Attribute = priceAttribute,
            DefaultValue = defaultDecimalValue.ToString(),
            TextAttributeId = Guid.NewGuid()
        };

        // Act
        // Assert

        Assert.Throws<AttributeAssemblyDataTypeMismatchException>(() => assembler.JoinAttributeWithData(priceAttribute, textAttributeData));
    }

    [Fact]
    public void TestPriceAttributeAndSelectableDataAssembled_FAIL()
    {
        // Arrange

        var assembler = new AttributeAssembler();
        var attributeId = Guid.NewGuid();
        var entityId = Guid.NewGuid();
        var priceAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = "UnitTest",
            AttributeType = AttributeType.Price,
            EntityId = entityId,
            Entity = new Entity()
        };

        var selectableAttributeData = new SelectableAttribute()
        {
            AttributeId = attributeId,
            Attribute = priceAttribute,
            SelectableAttributeId = Guid.NewGuid(),
            IsMultipleSelect = true,
            Values = new List<SelectableAttributeValue>()
        };


        // Act
        // Assert

        Assert.Throws<AttributeAssemblyDataTypeMismatchException>(() =>
            assembler.JoinAttributeWithData(priceAttribute, selectableAttributeData));
    }

    [Fact]
    public void TestTextAttributeAndPriceDataAssembled_FAIL()
    {
        // Arrange

        var assembler = new AttributeAssembler();

        var attributeId = Guid.NewGuid();
        var attributeName = "UnitTest";
        var entityId = Guid.NewGuid();

        var textAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = AttributeType.Text,
            EntityId = entityId,
            Entity = new Entity()
        };

        var priceAttributeData = new PriceAttribute()
        {
            AttributeId = attributeId,
            Attribute = textAttribute,
            DefaultValue = 2.67M,
            PriceAttributeId = Guid.NewGuid()
        };

        // Act
        // Assert

        Assert.Throws<AttributeAssemblyDataTypeMismatchException>(() => assembler.JoinAttributeWithData(textAttribute, priceAttributeData));
    }

    [Fact]
    public void TestTextAttributeAndSelectableDataAssembled_FAIL()
    {
        // Arrange

        var assembler = new AttributeAssembler();

        var attributeId = Guid.NewGuid();
        var attributeName = "UnitTest";
        var entityId = Guid.NewGuid();

        var textAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = AttributeType.Text,
            EntityId = entityId,
            Entity = new Entity()
        };

        var selectableAttributeData = new SelectableAttribute()
        {
            AttributeId = attributeId,
            Attribute = textAttribute,
            SelectableAttributeId = Guid.NewGuid(),
            IsMultipleSelect = true,
            Values = new List<SelectableAttributeValue>()
        };

        // Act
        // Assert

        Assert.Throws<AttributeAssemblyDataTypeMismatchException>(() => assembler.JoinAttributeWithData(textAttribute, selectableAttributeData));
    }

    [Fact]
    public void TestSelectableAttributeAndPriceDataAssembled_FAIL()
    {
        // Arrange

        var assembler = new AttributeAssembler();

        var attributeId = Guid.NewGuid();
        var attributeName = "UnitTest";
        var entityId = Guid.NewGuid();


        var selectableAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = AttributeType.Selectable,
            EntityId = entityId,
            Entity = new Entity()
        };

        var priceAttributeData = new PriceAttribute()
        {
            AttributeId = attributeId,
            Attribute = selectableAttribute,
            DefaultValue = 2.96M,
            PriceAttributeId = Guid.NewGuid()
        };

        // Act
        // Assert

        Assert.Throws<AttributeAssemblyDataTypeMismatchException>(() =>
            assembler.JoinAttributeWithData(selectableAttribute, priceAttributeData));
    }

    [Fact]
    public void TestSelectableAttributeAndTextDataAssembled_FAIL()
    {
        // Arrange

        var assembler = new AttributeAssembler();

        var attributeId = Guid.NewGuid();
        var attributeName = "UnitTest";
        var entityId = Guid.NewGuid();


        var selectableAttribute = new Attribute
        {
            AttributeId = attributeId,
            AttributeName = attributeName,
            AttributeType = AttributeType.Selectable,
            EntityId = entityId,
            Entity = new Entity()
        };

        var textAttributeData = new TextAttribute()
        {
            AttributeId = attributeId,
            Attribute = selectableAttribute,
            DefaultValue = "defaultTextValue",
            TextAttributeId = Guid.NewGuid()
        };

        // Act
        // Assert

        Assert.Throws<AttributeAssemblyDataTypeMismatchException>(() => assembler.JoinAttributeWithData(selectableAttribute, textAttributeData));
    }
}
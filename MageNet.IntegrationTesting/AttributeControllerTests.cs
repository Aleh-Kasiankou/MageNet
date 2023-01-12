using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using MageNetServices.AttributeRepository.DTO.Attributes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace MageNet.IntegrationTesting;

public class AttributeControllerTests : IClassFixture<IntegrationTestingWebApplicationFactory<Program>>
{
    private readonly IntegrationTestingWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public AttributeControllerTests(IntegrationTestingWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task TestPostPriceAttribute_Ok()
    {
        // Arrange

        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var postAttributeWithData = new PostAttributeWithData()
        {
            AttributeType = AttributeType.Price,
            AttributeName = "PostPriceAttribute",
            DefaultLiteralValue = 15.9684M.ToString(CultureInfo.CurrentCulture),
            EntityId = productEntity.EntityId,
        };

        // Act

        var response = await _httpClient.PostAsJsonAsync("attribute", postAttributeWithData);
        var savedAttributeId = Guid.Parse((await response.Content.ReadAsStringAsync()).Trim('"'));

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes
            .AnyAsync(x => x.AttributeId == savedAttributeId)));
        Assert.True(await _factory.UseDbContext(db => db.PriceAttributes
            .AnyAsync(x => x.AttributeId == savedAttributeId)));

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .AttributeName == postAttributeWithData.AttributeName);

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .EntityId == postAttributeWithData.EntityId);

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .AttributeType == postAttributeWithData.AttributeType);

        Assert.True((await
                _factory.UseDbContext(db => db.PriceAttributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .DefaultValue == Decimal.Parse(postAttributeWithData.DefaultLiteralValue));
    }

    [Fact]
    public async Task TestPostTextAttribute_Ok()
    {
        // Arrange

        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var postAttributeWithData = new PostAttributeWithData()
        {
            AttributeType = AttributeType.Text,
            AttributeName = "PostTextAttribute",
            DefaultLiteralValue = 15.9684M.ToString(CultureInfo.CurrentCulture),
            EntityId = productEntity.EntityId,
        };

        // Act

        var response = await _httpClient.PostAsJsonAsync("attribute", postAttributeWithData);
        var savedAttributeId = Guid.Parse((await response.Content.ReadAsStringAsync()).Trim('"'));

        // Assert
        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes
            .AnyAsync(x => x.AttributeId == savedAttributeId)));
        Assert.True(await _factory.UseDbContext(db => db.TextAttributes
            .AnyAsync(x => x.AttributeId == savedAttributeId)));

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .AttributeName == postAttributeWithData.AttributeName);

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .EntityId == postAttributeWithData.EntityId);

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .AttributeType == postAttributeWithData.AttributeType);

        Assert.True((await
                _factory.UseDbContext(db => db.TextAttributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .DefaultValue == postAttributeWithData.DefaultLiteralValue);
    }

    [Fact]
    public async Task TestPostSelectableAttribute_Ok()
    {
        // Arrange

        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var postAttributeWithData = new PostAttributeWithData()
        {
            AttributeType = AttributeType.Selectable,
            AttributeName = "PostSelectableAttribute",
            EntityId = productEntity.EntityId,
            IsMultipleSelect = true,
            SelectableOptions = new List<PostSelectableOption>()
            {
                new()
                {
                    IsDefaultValue = true,
                    Value = "Adidas"
                },
                new()
                {
                    IsDefaultValue = false,
                    Value = "Puma"
                }
            }
        };

        // Act

        var response = await _httpClient.PostAsJsonAsync("attribute", postAttributeWithData);
        var savedAttributeId = Guid.Parse((await response.Content.ReadAsStringAsync()).Trim('"'));

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes
            .AnyAsync(x => x.AttributeId == savedAttributeId)));
        Assert.True(await _factory.UseDbContext(db => db.SelectableAttributes
            .AnyAsync(x => x.AttributeId == savedAttributeId)));

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .AttributeName == postAttributeWithData.AttributeName);

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .EntityId == postAttributeWithData.EntityId);

        Assert.True((await
                _factory.UseDbContext(db => db.Attributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .AttributeType == postAttributeWithData.AttributeType);

        Assert.True((await
                _factory.UseDbContext(db =>
                    db.SelectableAttributes.SingleAsync(x => x.AttributeId == savedAttributeId)))
            .IsMultipleSelect == (bool)postAttributeWithData.IsMultipleSelect);

        Assert.True((await
                _factory.UseDbContext(db =>
                    db.SelectableAttributes.Include(attrData => attrData.Values)
                        .SingleAsync(x => x.AttributeId == savedAttributeId)))
            .Values.Count() == postAttributeWithData.SelectableOptions.Count());

        foreach (var option in postAttributeWithData.SelectableOptions)
        {
            Assert.Contains((await
                    _factory.UseDbContext(db =>
                        db.SelectableAttributes.Include(attrData => attrData.Values)
                            .SingleAsync(x => x.AttributeId == savedAttributeId)))
                .Values, opt => opt.Value == option.Value && opt.IsDefaultValue == option.IsDefaultValue);
        }
    }

    [Fact]
    public async Task TestGetAllAttributes_Ok()
    {
        // Arrange

        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var testAttribute = new AttributeEntity()
        {
            AttributeName = "TestAttributeGet",
            AttributeType = AttributeType.Price,
            EntityId = productEntity.EntityId
        };

        var testAttributeData = new PriceAttributeData()
        {
            Attribute = testAttribute,
            DefaultValue = 1568.318M,
        };

        var attributeId = await _factory.UseDbContext(async db =>
        {
            await db.Attributes.AddAsync(testAttribute);
            await db.PriceAttributes.AddAsync(testAttributeData);
            await db.SaveChangesAsync();
            return testAttribute.AttributeId;
        });

        // Act

        var response = await _httpClient.GetAsync("attribute");

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var responseCollection = JsonConvert.DeserializeObject<IEnumerable<AttributeWithData>>(json) ??
                                 throw new JsonSerializationException("Response is not deserializable");

        Assert.True(_factory.UseDbContext(db =>
            db.Attributes.Count() == responseCollection.Count()));

        Assert.Contains(responseCollection,
            attr => attr.AttributeId == attributeId && attr.AttributeName == testAttribute.AttributeName &&
                    attr.DefaultLiteralValue == testAttributeData.DefaultValue.ToString(CultureInfo.CurrentCulture));
    }

    [Fact]
    public async Task TestGetAttributeById()
    {
        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var testAttribute = new AttributeEntity()
        {
            AttributeName = "TestAttributeGetById",
            AttributeType = AttributeType.Price,
            EntityId = productEntity.EntityId
        };

        var testAttributeData = new PriceAttributeData()
        {
            Attribute = testAttribute,
            DefaultValue = 1567.315M,
        };

        var attributeId = await _factory.UseDbContext(async db =>
        {
            await db.Attributes.AddAsync(testAttribute);
            await db.PriceAttributes.AddAsync(testAttributeData);
            await db.SaveChangesAsync();
            return testAttribute.AttributeId;
        });

        // Act

        var response = await _httpClient.GetAsync($"attribute/{attributeId}");

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var responseAttributeWithData = JsonConvert.DeserializeObject<AttributeWithData>(json) ??
                                        throw new JsonSerializationException("Response is not deserializable");

        Assert.True(responseAttributeWithData.AttributeId == attributeId &&
                    responseAttributeWithData.AttributeName == testAttribute.AttributeName &&
                    responseAttributeWithData.DefaultLiteralValue ==
                    testAttributeData.DefaultValue.ToString(CultureInfo.CurrentCulture));
    }

    [Fact]
    public async Task TestDeletePriceAttributeById_Ok()
    {
        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var testAttribute = new AttributeEntity()
        {
            AttributeName = "TestAttributeDeleteByIdPrice",
            AttributeType = AttributeType.Price,
            EntityId = productEntity.EntityId
        };

        var testAttributeData = new PriceAttributeData()
        {
            Attribute = testAttribute,
            DefaultValue = 1567.315M,
        };

        var attributeId = await _factory.UseDbContext(async db =>
        {
            await db.Attributes.AddAsync(testAttribute);
            await db.PriceAttributes.AddAsync(testAttributeData);
            await db.SaveChangesAsync();
            return testAttribute.AttributeId;
        });

        // Act

        var response = await _httpClient.DeleteAsync($"attribute/{attributeId}");

        // Assert

        Assert.True(response.IsSuccessStatusCode);
        Assert.False(await _factory.UseDbContext(async db => await (db.Attributes.AnyAsync(x =>
            x.AttributeId == attributeId)) && await (db.PriceAttributes.AnyAsync(d => d.AttributeId == attributeId))));
    }

    [Fact]
    public async Task TestDeleteTextAttributeById_Ok()
    {
        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var testAttribute = new AttributeEntity()
        {
            AttributeName = "TestAttributeDeleteByIdText",
            AttributeType = AttributeType.Text,
            EntityId = productEntity.EntityId
        };

        var testAttributeData = new TextAttributeData()
        {
            Attribute = testAttribute,
            DefaultValue = 1567.315M.ToString(CultureInfo.CurrentCulture),
        };

        var attributeId = await _factory.UseDbContext(async db =>
        {
            await db.Attributes.AddAsync(testAttribute);
            await db.TextAttributes.AddAsync(testAttributeData);
            await db.SaveChangesAsync();
            return testAttribute.AttributeId;
        });

        // Act

        var response = await _httpClient.DeleteAsync($"attribute/{attributeId}");

        // Assert

        Assert.True(response.IsSuccessStatusCode);
        Assert.False(await _factory.UseDbContext(async db => await (db.Attributes.AnyAsync(x =>
            x.AttributeId == attributeId)) && await (db.TextAttributes.AnyAsync(d => d.AttributeId == attributeId))));
    }

    [Fact]
    public async Task TestDeleteSelectableAttributeById_Ok()
    {
        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var testAttribute = new AttributeEntity()
        {
            AttributeName = "TestAttributeDeleteByIdSelectable",
            AttributeType = AttributeType.Selectable,
            EntityId = productEntity.EntityId
        };

        var testAttributeData = new SelectableAttributeData()
        {
            Attribute = testAttribute,
            IsMultipleSelect = false,
            Values = new List<SelectableAttributeValue>()
            {
                new SelectableAttributeValue()
                {
                    IsDefaultValue = true,
                    Value = "TestDeleteSelectableValueAfterDeletingSelectableAttribute"
                }
            }
        };

        var attributeId = await _factory.UseDbContext(async db =>
        {
            await db.Attributes.AddAsync(testAttribute);
            await db.SelectableAttributes.AddAsync(testAttributeData);
            await db.SaveChangesAsync();
            return testAttribute.AttributeId;
        });

        // Act

        var response = await _httpClient.DeleteAsync($"attribute/{attributeId}");

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.False(await _factory.UseDbContext(async db => await db.Attributes.AnyAsync(x =>
            x.AttributeId == attributeId)));

        Assert.False(await _factory.UseDbContext(async db => await db.SelectableAttributes.AnyAsync(d =>
            d.AttributeId == attributeId)));

        Assert.False(await _factory.UseDbContext(async db => await db
            .SelectableAttributeValues
            .AnyAsync(v => v.Value ==
                           "TestDeleteSelectableValueAfterDeletingSelectableAttribute")));
    }


    [Fact]
    public async Task TestPutPriceAttributeNoTypeChange_Ok()
    {
        // Arrange

        var priceAttributeId = await CreatePriceAttribute();

        var putRequest = new PutAttributeWithData()
        {
            AttributeId = priceAttributeId,
            AttributeName = "TestPutPriceAttributeNoTypeChange",
            DefaultLiteralValue = "9999.9999"
        };

        // Act

        var response = await _httpClient.PutAsJsonAsync("attribute", putRequest);

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes.AnyAsync(x => x.AttributeId == priceAttributeId)));
        Assert.True(await _factory.UseDbContext(db =>
            db.PriceAttributes.AnyAsync(x => x.AttributeId == priceAttributeId)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.Attributes.SingleOrDefaultAsync(x =>
                x.AttributeId == priceAttributeId &&
                x.AttributeName == putRequest.AttributeName &&
                x.AttributeType == AttributeType.Price)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.PriceAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == priceAttributeId &&
                x.DefaultValue == Decimal.Parse(putRequest.DefaultLiteralValue))));
    }

    [Fact]
    public async Task TestPutTextAttributeNoTypeChange_Ok()
    {
        // Arrange

        var textAttributeId = await CreateTextAttribute();

        var putRequest = new PutAttributeWithData()
        {
            AttributeId = textAttributeId,
            AttributeName = "TestPutTextAttributeNoTypeChange",
            DefaultLiteralValue = "TestPutTextAttributeNoTypeChange"
        };

        // Act

        var response = await _httpClient.PutAsJsonAsync("attribute", putRequest);

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes.AnyAsync(x => x.AttributeId == textAttributeId)));
        Assert.True(await _factory.UseDbContext(db =>
            db.TextAttributes.AnyAsync(x => x.AttributeId == textAttributeId)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.Attributes.SingleOrDefaultAsync(x =>
                x.AttributeId == textAttributeId &&
                x.AttributeName == putRequest.AttributeName &&
                x.AttributeType == AttributeType.Text)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.TextAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == textAttributeId &&
                x.DefaultValue == putRequest.DefaultLiteralValue)));
    }

    [Fact]
    public async Task TestPutSelectableAttributeNoTypeChange_Ok()
    {
        // Arrange

        var selectableAttributeId = await CreateSelectableAttribute();
        var savedAttribute = await _factory.UseDbContext(db => db.SelectableAttributes
            .Include(x => x.Values)
            .SingleAsync(data => data.AttributeId == selectableAttributeId));

        var defaultOption = savedAttribute.Values.Single(x => x.IsDefaultValue && x.Value != "Untouchable")
            ;

        var nonDefaultOption = savedAttribute.Values
                .Single(x => x.IsDefaultValue == false && x.Value != "Untouchable")
            ;

        var untouchableOption = savedAttribute.Values
                .Single(x => x.Value == "Untouchable")
            ;

        var putRequest = new PutAttributeWithData()
        {
            AttributeId = selectableAttributeId,
            AttributeName = "TestPutSelectableAttributeNoTypeChange",
            IsMultipleSelect = !savedAttribute.IsMultipleSelect,
            SelectableOptions = new List<PutSelectableOption>
            {
                new()
                {
                    IsDefaultValue = true,
                    Value = "Adding new option"
                },

                new()
                {
                    OptionId = nonDefaultOption.SelectableAttributeValueId,
                    IsDefaultValue = true,
                    Value = "NowThisOptionIsDefault"
                },

                new()
                {
                    OptionId = defaultOption.SelectableAttributeValueId,
                    IsToDelete = true
                }
            }
        };

        // Act

        var response = await _httpClient.PutAsJsonAsync("attribute", putRequest);

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db =>
            db.Attributes.AnyAsync(x => x.AttributeId == selectableAttributeId)));
        Assert.True(await _factory.UseDbContext(db =>
            db.SelectableAttributes.AnyAsync(x => x.AttributeId == selectableAttributeId)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.Attributes.SingleOrDefaultAsync(x =>
                x.AttributeId == selectableAttributeId &&
                x.AttributeName == putRequest.AttributeName &&
                x.AttributeType == AttributeType.Selectable)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.SelectableAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == selectableAttributeId &&
                x.IsMultipleSelect != savedAttribute.IsMultipleSelect)));

        Assert.Contains(await _factory.UseDbContext(db =>
                db.SelectableAttributeValues.Where(x => x.AttributeId == savedAttribute.SelectableAttributeId)
                    .ToListAsync()),
            opt => opt.SelectableAttributeValueId == untouchableOption.SelectableAttributeValueId &&
                   opt.IsDefaultValue == untouchableOption.IsDefaultValue && opt.Value == untouchableOption.Value);

        Assert.Contains(await _factory.UseDbContext(db =>
                db.SelectableAttributeValues.Where(x => x.AttributeId == savedAttribute.SelectableAttributeId)
                    .ToListAsync()),
            opt => opt.SelectableAttributeValueId == nonDefaultOption.SelectableAttributeValueId &&
                   opt.IsDefaultValue != nonDefaultOption.IsDefaultValue && opt.Value == "NowThisOptionIsDefault");

        Assert.Contains(await _factory.UseDbContext(db =>
                db.SelectableAttributeValues.Where(x => x.AttributeId == savedAttribute.SelectableAttributeId)
                    .ToListAsync()),
            opt => opt.Value == "Adding new option" && opt.IsDefaultValue);

        Assert.DoesNotContain(await _factory.UseDbContext(db =>
                db.SelectableAttributeValues.Where(x => x.AttributeId == savedAttribute.SelectableAttributeId)
                    .ToListAsync()),
            opt => opt.SelectableAttributeValueId == defaultOption.SelectableAttributeValueId);
    }

    [Fact]
    public async Task TestPutPriceAttributeWithConversionToTextAttribute_Ok()
    {
        // Arrange

        var priceAttributeId = await CreatePriceAttribute();

        var putRequest = new PutAttributeWithData()
        {
            AttributeId = priceAttributeId,
            AttributeType = AttributeType.Text,
            AttributeName = "TestPutPriceAttributeToTextConversion",
            DefaultLiteralValue = "NewTextValue"
        };

        // Act

        var response = await _httpClient.PutAsJsonAsync("attribute", putRequest);

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes.AnyAsync(x => x.AttributeId == priceAttributeId)));

        Assert.True(await _factory.UseDbContext(db =>
            db.TextAttributes.AnyAsync(x => x.AttributeId == priceAttributeId)));

        Assert.Null(await _factory.UseDbContext(db =>
            db.PriceAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == priceAttributeId)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.Attributes.SingleOrDefaultAsync(x =>
                x.AttributeId == priceAttributeId &&
                x.AttributeName == putRequest.AttributeName &&
                x.AttributeType == AttributeType.Text)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.TextAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == priceAttributeId &&
                x.DefaultValue == putRequest.DefaultLiteralValue)));
    }

    [Fact]
    public async Task TestPutPriceAttributeWithConversionToSelectableAttribute_Ok()
    {
        // Arrange

        var priceAttributeId = await CreatePriceAttribute();

        var putRequest = new PutAttributeWithData()
        {
            AttributeId = priceAttributeId,
            AttributeType = AttributeType.Selectable,
            AttributeName = "TestPutPriceAttributeToSelectableConversion",
            IsMultipleSelect = true,
            SelectableOptions = new List<PutSelectableOption>()
            {
                new()
                {
                    IsDefaultValue = true,
                    Value = "Adding new option"
                },
                new()
                {
                    IsDefaultValue = false,
                    Value = "Adding second option"
                }
            }
        };

        // Act

        var response = await _httpClient.PutAsJsonAsync("attribute", putRequest);

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes.AnyAsync(x => x.AttributeId == priceAttributeId)));

        Assert.True(await _factory.UseDbContext(db =>
            db.SelectableAttributes.AnyAsync(x => x.AttributeId == priceAttributeId)));

        Assert.Null(await _factory.UseDbContext(db =>
            db.PriceAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == priceAttributeId)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.Attributes.SingleOrDefaultAsync(x =>
                x.AttributeId == priceAttributeId &&
                x.AttributeName == putRequest.AttributeName &&
                x.AttributeType == AttributeType.Selectable)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.SelectableAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == priceAttributeId)));

        Assert.Contains((await _factory.UseDbContext(db =>
                db.SelectableAttributes.Include(x => x.Values)
                    .SingleAsync(attr => attr.AttributeId == priceAttributeId))).Values,
            value => value.IsDefaultValue && value.Value == "Adding new option");
        
        Assert.Contains((await _factory.UseDbContext(db =>
                db.SelectableAttributes.Include(x => x.Values)
                    .SingleAsync(attr => attr.AttributeId == priceAttributeId))).Values,
            value => !value.IsDefaultValue && value.Value == "Adding second option");
    }

    [Fact]
    public async Task TestPutTextAttributeWithConversionToPriceAttribute_Ok()
    {
        // Arrange

        var textAttributeId = await CreateTextAttribute();

        var putRequest = new PutAttributeWithData()
        {
            AttributeId = textAttributeId,
            AttributeName = "TestPutTextAttributeToPriceConversion",
            DefaultLiteralValue = "1558489.2678946",
            AttributeType = AttributeType.Price
        };

        // Act

        var response = await _httpClient.PutAsJsonAsync("attribute", putRequest);
        
        
        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes.AnyAsync(x => x.AttributeId == textAttributeId)));

        Assert.True(await _factory.UseDbContext(db =>
            db.PriceAttributes.AnyAsync(x => x.AttributeId == textAttributeId)));

        Assert.Null(await _factory.UseDbContext(db =>
            db.TextAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == textAttributeId)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.Attributes.SingleOrDefaultAsync(x =>
                x.AttributeId == textAttributeId &&
                x.AttributeName == putRequest.AttributeName &&
                x.AttributeType == AttributeType.Price)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.PriceAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == textAttributeId &&
                x.DefaultValue == decimal.Parse(putRequest.DefaultLiteralValue))));
    }

    [Fact]
    public async Task TestPutTextAttributeWithConversionToSelectableAttribute_Ok()
    {
        // Arrange
        
        var textAttributeId = await CreateTextAttribute();

        var putRequest = new PutAttributeWithData()
        {
            AttributeId = textAttributeId,
            AttributeType = AttributeType.Selectable,
            AttributeName = "TestPutPriceAttributeToSelectableConversion",
            IsMultipleSelect = true,
            SelectableOptions = new List<PutSelectableOption>()
            {
                new()
                {
                    IsDefaultValue = true,
                    Value = "Adding new option"
                },
                new()
                {
                    IsDefaultValue = false,
                    Value = "Adding second option"
                }
            }
        };

        // Act

        var response = await _httpClient.PutAsJsonAsync("attribute", putRequest);

        // Assert

        Assert.True(response.IsSuccessStatusCode);

        Assert.True(await _factory.UseDbContext(db => db.Attributes.AnyAsync(x => x.AttributeId == textAttributeId)));

        Assert.True(await _factory.UseDbContext(db =>
            db.SelectableAttributes.AnyAsync(x => x.AttributeId == textAttributeId)));

        Assert.Null(await _factory.UseDbContext(db =>
            db.TextAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == textAttributeId)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.Attributes.SingleOrDefaultAsync(x =>
                x.AttributeId == textAttributeId &&
                x.AttributeName == putRequest.AttributeName &&
                x.AttributeType == AttributeType.Selectable)));

        Assert.NotNull(await _factory.UseDbContext(db =>
            db.SelectableAttributes.SingleOrDefaultAsync(x =>
                x.AttributeId == textAttributeId)));

        Assert.Contains((await _factory.UseDbContext(db =>
                db.SelectableAttributes.Include(x => x.Values)
                    .SingleAsync(attr => attr.AttributeId == textAttributeId))).Values,
            value => value.IsDefaultValue && value.Value == "Adding new option");
        
        Assert.Contains((await _factory.UseDbContext(db =>
                db.SelectableAttributes.Include(x => x.Values)
                    .SingleAsync(attr => attr.AttributeId == textAttributeId))).Values,
            value => !value.IsDefaultValue && value.Value == "Adding second option");
    }

    [Fact]
    public async Task TestPutSelectableAttributeWithConversionToPriceAttribute_Ok()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task TestPutSelectableAttributeWithConversionToTextAttribute_Ok()
    {
        // Arrange

        // Act

        // Assert
    }


    private async Task<Guid> CreatePriceAttribute()
    {
        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var testAttribute = new AttributeEntity()
        {
            AttributeName = "TestAttributeGet",
            AttributeType = AttributeType.Price,
            EntityId = productEntity.EntityId
        };

        var testAttributeData = new PriceAttributeData()
        {
            Attribute = testAttribute,
            DefaultValue = 1568.318M,
        };

        return await _factory.UseDbContext(async db =>
        {
            await db.Attributes.AddAsync(testAttribute);
            await db.PriceAttributes.AddAsync(testAttributeData);
            await db.SaveChangesAsync();
            return testAttribute.AttributeId;
        });
    }

    private async Task<Guid> CreateTextAttribute()
    {
        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var testAttribute = new AttributeEntity()
        {
            AttributeName = "TestAttributeDeleteByIdText",
            AttributeType = AttributeType.Text,
            EntityId = productEntity.EntityId
        };

        var testAttributeData = new TextAttributeData()
        {
            Attribute = testAttribute,
            DefaultValue = 1567.315M.ToString(CultureInfo.CurrentCulture),
        };

        return await _factory.UseDbContext(async db =>
        {
            await db.Attributes.AddAsync(testAttribute);
            await db.TextAttributes.AddAsync(testAttributeData);
            await db.SaveChangesAsync();
            return testAttribute.AttributeId;
        });
    }

    private async Task<Guid> CreateSelectableAttribute()
    {
        var productEntity =
            await _factory.UseDbContext(db => db.Entities.SingleAsync(x => x.EntityType == EntityType.Product));

        var testAttribute = new AttributeEntity()
        {
            AttributeName = "TestAttributeDeleteByIdSelectable",
            AttributeType = AttributeType.Selectable,
            EntityId = productEntity.EntityId
        };

        var testAttributeData = new SelectableAttributeData()
        {
            Attribute = testAttribute,
            IsMultipleSelect = false,
            Values = new List<SelectableAttributeValue>()
            {
                new SelectableAttributeValue()
                {
                    IsDefaultValue = true,
                    Value = "TestCreateDefaultSelectableValue"
                },
                new()
                {
                    IsDefaultValue = false,
                    Value = "TestCreateNonDefaultSelectableValue"
                },

                new()
                {
                    IsDefaultValue = false,
                    Value = "Untouchable"
                }
            }
        };

        return await _factory.UseDbContext(async db =>
        {
            await db.Attributes.AddAsync(testAttribute);
            await db.SelectableAttributes.AddAsync(testAttributeData);
            await db.SaveChangesAsync();
            return testAttribute.AttributeId;
        });
    }
}
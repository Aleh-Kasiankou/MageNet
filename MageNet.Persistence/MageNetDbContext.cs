using MageNet.Persistence.Models;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using Microsoft.EntityFrameworkCore;

namespace MageNet.Persistence;

public class MageNetDbContext : DbContext
{
    public DbSet<Entity> Entities { get; set; }
    public DbSet<AttributeEntity> Attributes { get; set; }
    public DbSet<PriceAttributeData> PriceAttributes { get; set; }
    public DbSet<SelectableAttributeData> SelectableAttributes { get; set; }
    public DbSet<SelectableAttributeValue> SelectableAttributeValues { get; set; }
    public DbSet<TextAttributeData> TextAttributes { get; set; }

    public MageNetDbContext(DbContextOptions<MageNetDbContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>().HasKey(x => x.EntityId);
        modelBuilder.Entity<Entity>().Property(x => x.Name).HasColumnType("nvarchar(100)");
        modelBuilder.Entity<Entity>().HasData(
            new List<Entity>()
            {
                new()
                {
                    EntityId = Guid.Parse("1d5b9219-9b8c-4996-8a35-51a27feaa74b"),
                    Name = "Product",
                    EntityType = EntityType.Product
                },
                new()
                {
                    EntityId = Guid.Parse("dee22e09-d91c-4037-8ad5-dd4213efe33f"),
                    Name = "Order",
                    EntityType = EntityType.Order
                },
                new()
                {
                    EntityId = Guid.Parse("122a1127-6afb-4cc6-976e-148eed423dcd"),
                    Name = "Customer",
                    EntityType = EntityType.Customer
                },
                new()
                {
                    EntityId = Guid.Parse("18f522b6-697d-4c96-ae91-018d3374ed14"),
                    Name = "Quote",
                    EntityType = EntityType.Quote
                }
            }
        );


        modelBuilder.Entity<AttributeEntity>().HasKey(x => x.AttributeId);
        modelBuilder.Entity<AttributeEntity>().Property(x => x.AttributeType).IsRequired()
            .HasColumnType("tinyint");

        modelBuilder.Entity<AttributeEntity>().Property(x => x.EntityId).IsRequired();
        modelBuilder.Entity<AttributeEntity>().Property(x => x.AttributeName).IsRequired();
        modelBuilder.Entity<AttributeEntity>().Property(x => x.AttributeName).HasColumnType("nvarchar(100)");
        modelBuilder.Entity<AttributeEntity>()
            .HasOne(x => x.Entity)
            .WithMany().HasForeignKey(x => x.EntityId);


        modelBuilder.Entity<PriceAttributeData>().HasKey(x => x.PriceAttributeId);
        modelBuilder.Entity<PriceAttributeData>().Property(x => x.DefaultValue).IsRequired();
        modelBuilder.Entity<PriceAttributeData>().Property(x => x.DefaultValue).HasColumnType("decimal")
            .HasPrecision(16, 6);
        modelBuilder.Entity<PriceAttributeData>().HasOne(x => x.Attribute)
            .WithOne().HasForeignKey<PriceAttributeData>(x => x.AttributeId).OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<TextAttributeData>().HasKey(x => x.TextAttributeId);
        modelBuilder.Entity<TextAttributeData>().Property(x => x.DefaultValue).IsRequired();
        modelBuilder.Entity<TextAttributeData>().Property(x => x.DefaultValue).HasColumnType("nvarchar(255)");
        modelBuilder.Entity<TextAttributeData>().HasOne(x => x.Attribute)
            .WithOne().HasForeignKey<TextAttributeData>(x => x.AttributeId).OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<SelectableAttributeData>().HasKey(x => x.SelectableAttributeId);
        modelBuilder.Entity<SelectableAttributeData>().Property(x => x.IsMultipleSelect).IsRequired();
        modelBuilder.Entity<SelectableAttributeData>().HasOne(x => x.Attribute)
            .WithOne().HasForeignKey<SelectableAttributeData>(x => x.AttributeId).OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<SelectableAttributeValue>().HasKey(x => x.SelectableAttributeValueId);
        modelBuilder.Entity<SelectableAttributeValue>().Property(x => x.AttributeId).IsRequired();
        modelBuilder.Entity<SelectableAttributeValue>().Property(x => x.Value).IsRequired();
        modelBuilder.Entity<SelectableAttributeValue>().Property(x => x.Value).HasColumnType("nvarchar(255)");
        modelBuilder.Entity<SelectableAttributeValue>().HasOne(x => x.Attribute)
            .WithMany(x => x.Values).HasForeignKey(x => x.AttributeId).OnDelete(DeleteBehavior.Cascade);
    }
}
using MageNet.Persistence.Models;
using MageNet.Persistence.Models.AbstractModels.ModelEnums;
using MageNet.Persistence.Models.Attributes;
using Microsoft.EntityFrameworkCore;
using Attribute = MageNet.Persistence.Models.Attributes.Attribute;

namespace MageNet.Persistence;

public class MageNetDbContext : DbContext
{
    public DbSet<Entity> Entities { get; set; }
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<PriceAttribute> PriceAttributes { get; set; }
    public DbSet<SelectableAttribute> SelectableAttributes { get; set; }
    public DbSet<SelectableAttributeValue> SelectableAttributeValues { get; set; }
    public DbSet<TextAttribute> TextAttributes { get; set; }

    public MageNetDbContext(DbContextOptions<MageNetDbContext> options) : base(options)
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


        modelBuilder.Entity<Attribute>().HasKey(x => x.AttributeId);
        modelBuilder.Entity<Attribute>().Property(x => x.AttributeType).IsRequired();
        modelBuilder.Entity<Attribute>().Property(x => x.EntityId).IsRequired();
        modelBuilder.Entity<Attribute>().Property(x => x.AttributeName).IsRequired();
        modelBuilder.Entity<Attribute>().Property(x => x.AttributeName).HasColumnType("nvarchar(100)");
        modelBuilder.Entity<Attribute>()
            .HasOne(x => x.Entity)
            .WithMany().HasForeignKey(x => x.EntityId);

        modelBuilder.Entity<PriceAttribute>().HasKey(x => x.PriceAttributeId);
        modelBuilder.Entity<PriceAttribute>().Property(x => x.DefaultValue).IsRequired();
        modelBuilder.Entity<PriceAttribute>().Property(x => x.DefaultValue).HasColumnType("decimal")
            .HasPrecision(16, 6);
        modelBuilder.Entity<PriceAttribute>().HasOne(x => x.Attribute)
            .WithOne().HasForeignKey<PriceAttribute>(x => x.AttributeId).OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<TextAttribute>().HasKey(x => x.TextAttributeId);
        modelBuilder.Entity<TextAttribute>().Property(x => x.DefaultValue).IsRequired();
        modelBuilder.Entity<TextAttribute>().Property(x => x.DefaultValue).HasColumnType("nvarchar(255)");
        modelBuilder.Entity<TextAttribute>().HasOne(x => x.Attribute)
            .WithOne().HasForeignKey<TextAttribute>(x => x.AttributeId).OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<SelectableAttribute>().HasKey(x => x.SelectableAttributeId);
        modelBuilder.Entity<SelectableAttribute>().Property(x => x.IsMultipleSelect).IsRequired();
        modelBuilder.Entity<SelectableAttribute>().HasOne(x => x.Attribute)
            .WithOne().HasForeignKey<SelectableAttribute>(x => x.AttributeId).OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<SelectableAttributeValue>().HasKey(x => x.SelectableAttributeValueId);
        modelBuilder.Entity<SelectableAttributeValue>().Property(x => x.AttributeId).IsRequired();
        modelBuilder.Entity<SelectableAttributeValue>().Property(x => x.Value).IsRequired();
        modelBuilder.Entity<SelectableAttributeValue>().Property(x => x.Value).HasColumnType("nvarchar(255)");
        modelBuilder.Entity<SelectableAttributeValue>().HasOne(x => x.Attribute)
            .WithMany(x => x.Values).HasForeignKey(x => x.AttributeId).OnDelete(DeleteBehavior.Cascade);
    }
}
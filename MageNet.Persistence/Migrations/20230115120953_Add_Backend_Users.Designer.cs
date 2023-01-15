﻿// <auto-generated />
using System;
using MageNet.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MageNet.Persistence.Migrations
{
    [DbContext(typeof(MageNetDbContext))]
    [Migration("20230115120953_Add_Backend_Users")]
    partial class Add_Backend_Users
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.AttributeEntity", b =>
                {
                    b.Property<Guid>("AttributeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AttributeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte>("AttributeType")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AttributeId");

                    b.HasIndex("EntityId");

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.PriceAttributeData", b =>
                {
                    b.Property<Guid>("PriceAttributeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DefaultValue")
                        .HasPrecision(16, 6)
                        .HasColumnType("decimal(16,6)");

                    b.HasKey("PriceAttributeId");

                    b.HasIndex("AttributeId")
                        .IsUnique();

                    b.ToTable("PriceAttributes");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.SelectableAttributeData", b =>
                {
                    b.Property<Guid>("SelectableAttributeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsMultipleSelect")
                        .HasColumnType("bit");

                    b.HasKey("SelectableAttributeId");

                    b.HasIndex("AttributeId")
                        .IsUnique();

                    b.ToTable("SelectableAttributes");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.SelectableAttributeOption", b =>
                {
                    b.Property<Guid>("OptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeDataId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDefaultValue")
                        .HasColumnType("bit");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("OptionId");

                    b.HasIndex("AttributeDataId");

                    b.ToTable("SelectableAttributeValues");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.TextAttributeData", b =>
                {
                    b.Property<Guid>("TextAttributeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AttributeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DefaultValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("TextAttributeId");

                    b.HasIndex("AttributeId")
                        .IsUnique();

                    b.ToTable("TextAttributes");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.BackendUser", b =>
                {
                    b.Property<Guid>("BackendUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BackendUserId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("BackendUsers");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Entity", b =>
                {
                    b.Property<Guid>("EntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("EntityType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("EntityId");

                    b.ToTable("Entities");

                    b.HasData(
                        new
                        {
                            EntityId = new Guid("1d5b9219-9b8c-4996-8a35-51a27feaa74b"),
                            EntityType = 1,
                            Name = "Product"
                        },
                        new
                        {
                            EntityId = new Guid("dee22e09-d91c-4037-8ad5-dd4213efe33f"),
                            EntityType = 4,
                            Name = "Order"
                        },
                        new
                        {
                            EntityId = new Guid("122a1127-6afb-4cc6-976e-148eed423dcd"),
                            EntityType = 2,
                            Name = "Customer"
                        },
                        new
                        {
                            EntityId = new Guid("18f522b6-697d-4c96-ae91-018d3374ed14"),
                            EntityType = 3,
                            Name = "Quote"
                        });
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.AttributeEntity", b =>
                {
                    b.HasOne("MageNet.Persistence.Models.Entity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entity");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.PriceAttributeData", b =>
                {
                    b.HasOne("MageNet.Persistence.Models.Attributes.AttributeEntity", "Attribute")
                        .WithOne()
                        .HasForeignKey("MageNet.Persistence.Models.Attributes.PriceAttributeData", "AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.SelectableAttributeData", b =>
                {
                    b.HasOne("MageNet.Persistence.Models.Attributes.AttributeEntity", "Attribute")
                        .WithOne()
                        .HasForeignKey("MageNet.Persistence.Models.Attributes.SelectableAttributeData", "AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.SelectableAttributeOption", b =>
                {
                    b.HasOne("MageNet.Persistence.Models.Attributes.SelectableAttributeData", "Attribute")
                        .WithMany("Values")
                        .HasForeignKey("AttributeDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.TextAttributeData", b =>
                {
                    b.HasOne("MageNet.Persistence.Models.Attributes.AttributeEntity", "Attribute")
                        .WithOne()
                        .HasForeignKey("MageNet.Persistence.Models.Attributes.TextAttributeData", "AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");
                });

            modelBuilder.Entity("MageNet.Persistence.Models.Attributes.SelectableAttributeData", b =>
                {
                    b.Navigation("Values");
                });
#pragma warning restore 612, 618
        }
    }
}

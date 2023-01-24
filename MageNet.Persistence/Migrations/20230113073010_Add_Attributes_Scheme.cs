using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MageNet.Persistence.Migrations
{
    public partial class Add_Attributes_Scheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EntityType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Attributes",
                columns: table => new
                {
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attributes", x => x.AttributeId);
                    table.ForeignKey(
                        name: "FK_Attributes_Entities_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entities",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceAttributes",
                columns: table => new
                {
                    PriceAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultValue = table.Column<decimal>(type: "decimal(16,6)", precision: 16, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceAttributes", x => x.PriceAttributeId);
                    table.ForeignKey(
                        name: "FK_PriceAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectableAttributes",
                columns: table => new
                {
                    SelectableAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMultipleSelect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectableAttributes", x => x.SelectableAttributeId);
                    table.ForeignKey(
                        name: "FK_SelectableAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextAttributes",
                columns: table => new
                {
                    TextAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextAttributes", x => x.TextAttributeId);
                    table.ForeignKey(
                        name: "FK_TextAttributes_Attributes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "Attributes",
                        principalColumn: "AttributeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectableAttributeValues",
                columns: table => new
                {
                    OptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    IsDefaultValue = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectableAttributeValues", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_SelectableAttributeValues_SelectableAttributes_AttributeDataId",
                        column: x => x.AttributeDataId,
                        principalTable: "SelectableAttributes",
                        principalColumn: "SelectableAttributeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Entities",
                columns: new[] { "EntityId", "EntityType", "Name" },
                values: new object[,]
                {
                    { new Guid("122a1127-6afb-4cc6-976e-148eed423dcd"), 2, "Customer" },
                    { new Guid("18f522b6-697d-4c96-ae91-018d3374ed14"), 3, "Quote" },
                    { new Guid("1d5b9219-9b8c-4996-8a35-51a27feaa74b"), 1, "Product" },
                    { new Guid("dee22e09-d91c-4037-8ad5-dd4213efe33f"), 4, "Order" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_EntityId",
                table: "Attributes",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAttributes_AttributeId",
                table: "PriceAttributes",
                column: "AttributeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SelectableAttributes_AttributeId",
                table: "SelectableAttributes",
                column: "AttributeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SelectableAttributeValues_AttributeDataId",
                table: "SelectableAttributeValues",
                column: "AttributeDataId");

            migrationBuilder.CreateIndex(
                name: "IX_TextAttributes_AttributeId",
                table: "TextAttributes",
                column: "AttributeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceAttributes");

            migrationBuilder.DropTable(
                name: "SelectableAttributeValues");

            migrationBuilder.DropTable(
                name: "TextAttributes");

            migrationBuilder.DropTable(
                name: "SelectableAttributes");

            migrationBuilder.DropTable(
                name: "Attributes");

            migrationBuilder.DropTable(
                name: "Entities");
        }
    }
}

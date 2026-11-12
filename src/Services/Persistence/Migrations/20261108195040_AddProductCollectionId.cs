using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductCollectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CollectionId",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "Products");
        }
    }
}

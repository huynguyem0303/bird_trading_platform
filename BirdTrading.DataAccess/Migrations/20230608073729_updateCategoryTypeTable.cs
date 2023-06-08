using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirdTrading.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateCategoryTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "CategoryType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "CategoryType");
        }
    }
}

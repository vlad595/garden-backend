using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace garden_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToPlants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Plants",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Plants");
        }
    }
}

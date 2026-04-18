using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace garden_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "age",
                table: "Plants",
                newName: "Age");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Plants",
                newName: "age");
        }
    }
}

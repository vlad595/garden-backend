using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace garden_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToCareResources2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CareResources",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CareResources_UserId",
                table: "CareResources",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareResources_Users_UserId",
                table: "CareResources",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareResources_Users_UserId",
                table: "CareResources");

            migrationBuilder.DropIndex(
                name: "IX_CareResources_UserId",
                table: "CareResources");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CareResources");
        }
    }
}

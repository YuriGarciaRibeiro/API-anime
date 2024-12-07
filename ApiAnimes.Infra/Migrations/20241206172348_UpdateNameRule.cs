using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAnimes.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNameRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Animes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_Name",
                table: "Animes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Animes_Name",
                table: "Animes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

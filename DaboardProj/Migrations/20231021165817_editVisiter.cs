using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaboardProj.Migrations
{
    /// <inheritdoc />
    public partial class editVisiter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Visiters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Visiters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

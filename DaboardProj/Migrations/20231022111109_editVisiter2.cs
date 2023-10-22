using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaboardProj.Migrations
{
    /// <inheritdoc />
    public partial class editVisiter2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNewVisiter",
                table: "Visiters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNewVisiter",
                table: "Visiters");
        }
    }
}

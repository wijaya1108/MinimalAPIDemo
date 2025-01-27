using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoMinimalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressColumnToDeptEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Departments");
        }
    }
}

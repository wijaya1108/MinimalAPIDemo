using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoMinimalAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAddressColumnFromDeptEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Departments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

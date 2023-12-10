using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RadzenBook.Infrastructure.Persistence.Migrations.MSSQL
{
    public partial class AddPhotoToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "catalog",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "PersonalPhoto",
                schema: "catalog",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalPhoto",
                schema: "catalog",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "catalog",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

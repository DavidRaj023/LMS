using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Migrations
{
    public partial class AddUserRoleByQuery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Roles (Name) VALUES ('Admin')");
            migrationBuilder.Sql("INSERT INTO Roles (Name) VALUES ('Customer')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

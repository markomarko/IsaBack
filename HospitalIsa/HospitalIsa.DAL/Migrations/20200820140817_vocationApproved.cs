using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalIsa.DAL.Migrations
{
    public partial class vocationApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Vocations",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Vocations");
        }
    }
}

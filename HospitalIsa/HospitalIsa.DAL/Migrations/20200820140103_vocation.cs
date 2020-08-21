using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalIsa.DAL.Migrations
{
    public partial class vocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    doctorId = table.Column<Guid>(nullable: false),
                    startDate = table.Column<DateTime>(nullable: false),
                    endDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vocations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vocations");
        }
    }
}

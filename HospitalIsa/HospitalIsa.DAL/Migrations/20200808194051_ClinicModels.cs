using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalIsa.DAL.Migrations
{
    public partial class ClinicModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicId",
                table: "Employees",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ClinicId",
                table: "Employees",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ClinicId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Employees");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalIsa.DAL.Migrations
{
    public partial class employeeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClinicId",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "ClinicId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClinicId",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Clinics_ClinicId",
                table: "Employees",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "ClinicId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

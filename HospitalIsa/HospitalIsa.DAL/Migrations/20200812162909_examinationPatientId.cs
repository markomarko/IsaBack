using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalIsa.DAL.Migrations
{
    public partial class examinationPatientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "Examinations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Examinations");
        }
    }
}

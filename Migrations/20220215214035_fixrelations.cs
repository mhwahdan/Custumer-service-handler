using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POC.Migrations
{
    public partial class fixrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_service_EmployeeId",
                table: "service",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_service_employee_EmployeeId",
                table: "service",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_service_employee_EmployeeId",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_service_EmployeeId",
                table: "service");
        }
    }
}

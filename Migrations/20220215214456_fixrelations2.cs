using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POC.Migrations
{
    public partial class fixrelations2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "service_location",
                table: "service");

            migrationBuilder.DropIndex(
                name: "IX_service_branchid",
                table: "service");

            migrationBuilder.DropColumn(
                name: "branchid",
                table: "service");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "branchid",
                table: "service",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_service_branchid",
                table: "service",
                column: "branchid");

            migrationBuilder.AddForeignKey(
                name: "service_location",
                table: "service",
                column: "branchid",
                principalTable: "branch",
                principalColumn: "branchid");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alerts.Persistence.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_Application_ApplicationCode",
                table: "Alert");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_Application_ApplicationCode",
                table: "Alert",
                column: "ApplicationCode",
                principalTable: "Application",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_Application_ApplicationCode",
                table: "Alert");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_Application_ApplicationCode",
                table: "Alert",
                column: "ApplicationCode",
                principalTable: "Application",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odontio.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WorkspaceNewFieldsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Workspaces",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Workspaces",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhoneNumber",
                table: "Workspaces",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "ContactPhoneNumber",
                table: "Workspaces");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Workspaces",
                newName: "Phone");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odontio.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WorkspaceUserDeleteCascadeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Workspaces_WorkspaceId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Workspaces_WorkspaceId",
                table: "Users",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Workspaces_WorkspaceId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Workspaces_WorkspaceId",
                table: "Users",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id");
        }
    }
}

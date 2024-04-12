using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odontio.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BusinessNameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Workspaces",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Workspaces");
        }
    }
}

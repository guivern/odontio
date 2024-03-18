using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odontio.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ToothMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teeth",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Odontogram",
                table: "Teeth",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Odontogram",
                table: "Teeth");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Teeth",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odontio.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DiseasesByWorkspaceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientDiseases_Diseases_DiseaseId",
                table: "PatientDiseases");

            migrationBuilder.AddColumn<long>(
                name: "WorkspaceId",
                table: "Diseases",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Diseases_WorkspaceId",
                table: "Diseases",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diseases_Workspaces_WorkspaceId",
                table: "Diseases",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDiseases_Diseases_DiseaseId",
                table: "PatientDiseases",
                column: "DiseaseId",
                principalTable: "Diseases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diseases_Workspaces_WorkspaceId",
                table: "Diseases");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientDiseases_Diseases_DiseaseId",
                table: "PatientDiseases");

            migrationBuilder.DropIndex(
                name: "IX_Diseases_WorkspaceId",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Diseases");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDiseases_Diseases_DiseaseId",
                table: "PatientDiseases",
                column: "DiseaseId",
                principalTable: "Diseases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

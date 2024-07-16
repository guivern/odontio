using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odontio.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PatientTreatmentDiagnosisMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTreatments_Teeth_ToothId",
                table: "PatientTreatments");

            migrationBuilder.RenameColumn(
                name: "ToothId",
                table: "PatientTreatments",
                newName: "DiagnosisId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTreatments_ToothId",
                table: "PatientTreatments",
                newName: "IX_PatientTreatments_DiagnosisId");

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "Budgets",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTreatments_Diagnoses_DiagnosisId",
                table: "PatientTreatments",
                column: "DiagnosisId",
                principalTable: "Diagnoses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientTreatments_Diagnoses_DiagnosisId",
                table: "PatientTreatments");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "Budgets");

            migrationBuilder.RenameColumn(
                name: "DiagnosisId",
                table: "PatientTreatments",
                newName: "ToothId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientTreatments_DiagnosisId",
                table: "PatientTreatments",
                newName: "IX_PatientTreatments_ToothId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientTreatments_Teeth_ToothId",
                table: "PatientTreatments",
                column: "ToothId",
                principalTable: "Teeth",
                principalColumn: "Id");
        }
    }
}

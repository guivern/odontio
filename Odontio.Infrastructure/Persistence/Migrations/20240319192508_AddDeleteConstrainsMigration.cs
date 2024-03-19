﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Odontio.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteConstrainsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Patients_PatientId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_PatientTreatments_PatientTreatmentId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Budgets_BudgetId",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Patients_PatientId",
                table: "Budgets",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_PatientTreatments_PatientTreatmentId",
                table: "MedicalRecords",
                column: "PatientTreatmentId",
                principalTable: "PatientTreatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Budgets_BudgetId",
                table: "Payments",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Patients_PatientId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_PatientTreatments_PatientTreatmentId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Budgets_BudgetId",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Patients_PatientId",
                table: "Budgets",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_PatientTreatments_PatientTreatmentId",
                table: "MedicalRecords",
                column: "PatientTreatmentId",
                principalTable: "PatientTreatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Budgets_BudgetId",
                table: "Payments",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

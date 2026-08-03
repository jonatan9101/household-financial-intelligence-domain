using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseholdFinancialIntelligence.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialMovement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HouseholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    FinancialAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    MovementType = table.Column<string>(type: "text", nullable: false),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EvidenceReference = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialMovement", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "UX_FinancialMovement_Household_EvidenceReference",
                table: "FinancialMovement",
                columns: new[] { "HouseholdId", "EvidenceReference" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialMovement");
        }
    }
}

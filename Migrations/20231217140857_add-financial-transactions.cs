using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace market.Migrations
{
    /// <inheritdoc />
    public partial class addfinancialtransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinancialTransactionId",
                table: "Order",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FinancialTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    FinancialTransactionStatus = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    FinancialTransactionType = table.Column<int>(type: "integer", nullable: false),
                    CreateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransaction", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_FinancialTransactionId",
                table: "Order",
                column: "FinancialTransactionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_FinancialTransaction_FinancialTransactionId",
                table: "Order",
                column: "FinancialTransactionId",
                principalTable: "FinancialTransaction",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_FinancialTransaction_FinancialTransactionId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "FinancialTransaction");

            migrationBuilder.DropIndex(
                name: "IX_Order_FinancialTransactionId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FinancialTransactionId",
                table: "Order");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace market.Migrations
{
    /// <inheritdoc />
    public partial class add_financial_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_FinancialTransaction_FinancialTransactionId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "FinancialTransactionStatus",
                table: "FinancialTransaction");

            migrationBuilder.RenameColumn(
                name: "FinancialTransactionId",
                table: "Order",
                newName: "FinancialDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_FinancialTransactionId",
                table: "Order",
                newName: "IX_Order_FinancialDocumentId");

            migrationBuilder.RenameColumn(
                name: "FinancialTransactionType",
                table: "FinancialTransaction",
                newName: "TransactionFactor");

            migrationBuilder.AddColumn<int>(
                name: "FinancialAccountId",
                table: "FinancialTransaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinancialDocumentId",
                table: "FinancialTransaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BankTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReferenceCode = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Session = table.Column<string>(type: "text", nullable: false),
                    CreateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankTransaction_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PanelId = table.Column<int>(type: "integer", nullable: true),
                    CustomerId = table.Column<int>(type: "integer", nullable: true),
                    CreateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAccount_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialAccount_Panel_PanelId",
                        column: x => x.PanelId,
                        principalTable: "Panel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FinancialDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialDocument", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_FinancialAccountId",
                table: "FinancialTransaction",
                column: "FinancialAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_FinancialDocumentId",
                table: "FinancialTransaction",
                column: "FinancialDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BankTransaction_CustomerId",
                table: "BankTransaction",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_CustomerId",
                table: "FinancialAccount",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_PanelId",
                table: "FinancialAccount",
                column: "PanelId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialTransaction_FinancialAccount_FinancialAccountId",
                table: "FinancialTransaction",
                column: "FinancialAccountId",
                principalTable: "FinancialAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialTransaction_FinancialDocument_FinancialDocumentId",
                table: "FinancialTransaction",
                column: "FinancialDocumentId",
                principalTable: "FinancialDocument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_FinancialDocument_FinancialDocumentId",
                table: "Order",
                column: "FinancialDocumentId",
                principalTable: "FinancialDocument",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialTransaction_FinancialAccount_FinancialAccountId",
                table: "FinancialTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialTransaction_FinancialDocument_FinancialDocumentId",
                table: "FinancialTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_FinancialDocument_FinancialDocumentId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "BankTransaction");

            migrationBuilder.DropTable(
                name: "FinancialAccount");

            migrationBuilder.DropTable(
                name: "FinancialDocument");

            migrationBuilder.DropIndex(
                name: "IX_FinancialTransaction_FinancialAccountId",
                table: "FinancialTransaction");

            migrationBuilder.DropIndex(
                name: "IX_FinancialTransaction_FinancialDocumentId",
                table: "FinancialTransaction");

            migrationBuilder.DropColumn(
                name: "FinancialAccountId",
                table: "FinancialTransaction");

            migrationBuilder.DropColumn(
                name: "FinancialDocumentId",
                table: "FinancialTransaction");

            migrationBuilder.RenameColumn(
                name: "FinancialDocumentId",
                table: "Order",
                newName: "FinancialTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_FinancialDocumentId",
                table: "Order",
                newName: "IX_Order_FinancialTransactionId");

            migrationBuilder.RenameColumn(
                name: "TransactionFactor",
                table: "FinancialTransaction",
                newName: "FinancialTransactionType");

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransactionStatus",
                table: "FinancialTransaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_FinancialTransaction_FinancialTransactionId",
                table: "Order",
                column: "FinancialTransactionId",
                principalTable: "FinancialTransaction",
                principalColumn: "Id");
        }
    }
}

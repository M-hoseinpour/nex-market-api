using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace market.Migrations
{
    /// <inheritdoc />
    public partial class Panel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PanelId",
                table: "Staff",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Panel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    CreateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteMoment = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Panel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Panel_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_PanelId",
                table: "Staff",
                column: "PanelId");

            migrationBuilder.CreateIndex(
                name: "IX_Panel_ManagerId",
                table: "Panel",
                column: "ManagerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Panel_PanelId",
                table: "Staff",
                column: "PanelId",
                principalTable: "Panel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Panel_PanelId",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "Panel");

            migrationBuilder.DropIndex(
                name: "IX_Staff_PanelId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "PanelId",
                table: "Staff");
        }
    }
}

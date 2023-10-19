using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market.Migrations
{
    /// <inheritdoc />
    public partial class nullableStaffPanelId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Panel_PanelId",
                table: "Staff");

            migrationBuilder.AlterColumn<int>(
                name: "PanelId",
                table: "Staff",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Panel_PanelId",
                table: "Staff",
                column: "PanelId",
                principalTable: "Panel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Panel_PanelId",
                table: "Staff");

            migrationBuilder.AlterColumn<int>(
                name: "PanelId",
                table: "Staff",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Panel_PanelId",
                table: "Staff",
                column: "PanelId",
                principalTable: "Panel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

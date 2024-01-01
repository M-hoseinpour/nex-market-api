using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace market.Migrations
{
    /// <inheritdoc />
    public partial class modify_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarLogo",
                table: "User");

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarFileId",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPrice",
                table: "Product",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PanelId",
                table: "Banner",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "FileCategory",
                columns: new[] { "Id", "Description", "SubDirectory", "Title" },
                values: new object[,]
                {
                    { (short)1, "All The public pictures, can be hosted in cdn", "pictures", "Pictures" },
                    { (short)2, "All kind of documents, like identity or ownership documents. This files are private all the time", "documents", "Documents" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_AvatarFileId",
                table: "User",
                column: "AvatarFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banner_PanelId",
                table: "Banner",
                column: "PanelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banner_Panel_PanelId",
                table: "Banner",
                column: "PanelId",
                principalTable: "Panel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_File_AvatarFileId",
                table: "User",
                column: "AvatarFileId",
                principalTable: "File",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banner_Panel_PanelId",
                table: "Banner");

            migrationBuilder.DropForeignKey(
                name: "FK_User_File_AvatarFileId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_AvatarFileId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Banner_PanelId",
                table: "Banner");

            migrationBuilder.DeleteData(
                table: "FileCategory",
                keyColumn: "Id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "FileCategory",
                keyColumn: "Id",
                keyValue: (short)2);

            migrationBuilder.DropColumn(
                name: "AvatarFileId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PanelId",
                table: "Banner");

            migrationBuilder.AddColumn<string>(
                name: "AvatarLogo",
                table: "User",
                type: "text",
                nullable: true);
        }
    }
}

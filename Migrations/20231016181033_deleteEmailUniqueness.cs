using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace market.Migrations
{
    /// <inheritdoc />
    public partial class deleteEmailUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class changesInTrackingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataChangeUserId",
                table: "Tracking",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracking_DataChangeUserId",
                table: "Tracking",
                column: "DataChangeUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracking_AspNetUsers_DataChangeUserId",
                table: "Tracking",
                column: "DataChangeUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracking_AspNetUsers_DataChangeUserId",
                table: "Tracking");

            migrationBuilder.DropIndex(
                name: "IX_Tracking_DataChangeUserId",
                table: "Tracking");

            migrationBuilder.DropColumn(
                name: "DataChangeUserId",
                table: "Tracking");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace remont_kvartir.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewRequestLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepairRequestId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RepairRequestId",
                table: "Reviews",
                column: "RepairRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_RepairRequests_RepairRequestId",
                table: "Reviews",
                column: "RepairRequestId",
                principalTable: "RepairRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_RepairRequests_RepairRequestId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RepairRequestId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "RepairRequestId",
                table: "Reviews");
        }
    }
}

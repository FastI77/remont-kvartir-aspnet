using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace remont_kvartir.Migrations
{
    /// <inheritdoc />
    public partial class AddedClientInfoToRequestss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "RepairRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientPhone",
                table: "RepairRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "RepairRequests");

            migrationBuilder.DropColumn(
                name: "ClientPhone",
                table: "RepairRequests");
        }
    }
}

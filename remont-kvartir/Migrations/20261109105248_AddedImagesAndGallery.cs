using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace remont_kvartir.Migrations
{
    /// <inheritdoc />
    public partial class AddedImagesAndGallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GalleryImages",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GalleryImages",
                table: "PortfolioItems");
        }
    }
}

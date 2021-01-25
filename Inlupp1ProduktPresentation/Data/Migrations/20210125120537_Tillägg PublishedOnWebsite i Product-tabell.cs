using Microsoft.EntityFrameworkCore.Migrations;

namespace Inlupp1ProduktPresentation.Data.Migrations
{
    public partial class TilläggPublishedOnWebsiteiProducttabell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PublishedOnWebsite",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedOnWebsite",
                table: "Products");
        }
    }
}

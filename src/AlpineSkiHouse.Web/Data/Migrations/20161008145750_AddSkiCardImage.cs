using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlpineSkiHouse.Web.Data.Migrations
{
    public partial class AddSkiCardImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CardImageId",
                table: "SkiCards",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardImageId",
                table: "SkiCards");
        }
    }
}

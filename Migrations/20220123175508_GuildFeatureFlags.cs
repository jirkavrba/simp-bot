using Microsoft.EntityFrameworkCore.Migrations;
using SimpBot.Models;

#nullable disable

namespace SimpBot.Migrations
{
    public partial class GuildFeatureFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnabledFeatures",
                table: "GuildSettings",
                type: "integer",
                nullable: false,
                defaultValue: 3
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnabledFeatures",
                table: "GuildSettings");
        }
    }
}

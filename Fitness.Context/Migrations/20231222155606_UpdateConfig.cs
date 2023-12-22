using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fitness.Context.Migrations
{
    public partial class UpdateConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Club_Title",
                table: "Clubs",
                newName: "Club_Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "Club_Email",
                table: "Clubs",
                newName: "IX_Club_Title");
        }
    }
}

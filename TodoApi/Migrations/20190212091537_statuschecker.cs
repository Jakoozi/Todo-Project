using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoApi.Migrations
{
    public partial class statuschecker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "TodoItems");

            migrationBuilder.AddColumn<int>(
                name: "StatusReturner",
                table: "TodoItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StatusCheckers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCheckers", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusCheckers");

            migrationBuilder.DropColumn(
                name: "StatusReturner",
                table: "TodoItems");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TodoItems",
                nullable: false,
                defaultValue: 0);
        }
    }
}

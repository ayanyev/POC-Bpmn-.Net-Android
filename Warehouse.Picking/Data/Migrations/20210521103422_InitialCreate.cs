using Microsoft.EntityFrameworkCore.Migrations;

namespace Warehouse.Picking.Api.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    NoteId = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Gtin = table.Column<string>(type: "TEXT", nullable: true),
                    IsSuspended = table.Column<bool>(type: "INTEGER", nullable: false),
                    Quantity_Expected = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantity_Processed = table.Column<int>(type: "INTEGER", nullable: true),
                    Bundle_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Bundle_Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => new { x.Id, x.NoteId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "articles");
        }
    }
}

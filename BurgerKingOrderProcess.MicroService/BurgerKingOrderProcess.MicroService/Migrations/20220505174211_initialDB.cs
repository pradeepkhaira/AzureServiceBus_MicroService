using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BurgerKingOrderProcess.MicroService.Migrations
{
    public partial class initialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BurgerKingOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OderId = table.Column<int>(type: "int", nullable: false),
                    OrderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMeal = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BurgerKingOrder", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BurgerKingOrder");
        }
    }
}

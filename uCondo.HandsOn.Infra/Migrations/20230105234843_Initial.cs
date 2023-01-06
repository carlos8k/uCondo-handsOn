using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uCondo.HandsOn.Infra.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AllowEntries = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_ParentCode",
                        column: x => x.ParentCode,
                        principalTable: "Accounts",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentCode",
                table: "Accounts",
                column: "ParentCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}

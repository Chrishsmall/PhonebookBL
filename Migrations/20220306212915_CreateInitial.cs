using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhonebookBL.Migrations
{
    public partial class CreateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Phonebook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phonebook", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PbEntry",
                columns: table => new
                {
                    EntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhonebookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PbEntry", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_PbEntry_Phonebook_PhonebookId",
                        column: x => x.PhonebookId,
                        principalTable: "Phonebook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PbEntry_PhonebookId_Name",
                table: "PbEntry",
                columns: new[] { "PhonebookId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phonebook_Name",
                table: "Phonebook",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PbEntry");

            migrationBuilder.DropTable(
                name: "Phonebook");
        }
    }
}

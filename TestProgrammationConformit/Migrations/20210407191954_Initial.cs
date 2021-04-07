using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestProgrammationConformit.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    InvolvedPerson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DateOfCreation = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Description", "InvolvedPerson", "Title" },
                values: new object[,]
                {
                    { new Guid("99c36725-04ec-4336-815c-c6206ca40384"), "On Sunday, March 14, 2021, the 63rd GRAMMY Awards will be happening.", "John Legend", "Grammys 2021" },
                    { new Guid("fc8fde5a-d62e-41d0-a740-372c31cdeb5c"), "The 93rd Academy Awards ceremony, presented by the Academy of Motion Picture Arts and Sciences (AMPAS), will honor the best films released between January 1, 2020, and February 28, 2021.", "Ashley Fox", "Oscars 2021" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "DateOfCreation", "Description", "EventId" },
                values: new object[,]
                {
                    { new Guid("004a4887-4011-44b8-940a-cbac81bade25"), new DateTimeOffset(new DateTime(2021, 4, 7, 20, 19, 53, 812, DateTimeKind.Unspecified).AddTicks(8539), new TimeSpan(0, 1, 0, 0, 0)), "Breath taking performance by Adele", new Guid("99c36725-04ec-4336-815c-c6206ca40384") },
                    { new Guid("10dfede3-a6dc-4e88-b191-66b9d3ed6fae"), new DateTimeOffset(new DateTime(2021, 4, 7, 20, 19, 53, 814, DateTimeKind.Unspecified).AddTicks(8769), new TimeSpan(0, 1, 0, 0, 0)), "Demi Lovato was just Amazing !!!", new Guid("99c36725-04ec-4336-815c-c6206ca40384") },
                    { new Guid("14f25238-fb47-4c3c-9962-7f051141bfd9"), new DateTimeOffset(new DateTime(2021, 4, 7, 20, 19, 53, 814, DateTimeKind.Unspecified).AddTicks(8842), new TimeSpan(0, 1, 0, 0, 0)), " The choice is so hard this time ! everybody deserve to win !", new Guid("fc8fde5a-d62e-41d0-a740-372c31cdeb5c") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_EventId",
                table: "Comments",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}

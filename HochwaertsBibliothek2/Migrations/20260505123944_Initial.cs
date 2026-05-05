using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HochwaertsBibliothek2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buecher",
                columns: table => new
                {
                    BuchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Erscheinungsjahr = table.Column<int>(type: "int", nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Klappentext = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Inhalt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buecher", x => x.BuchId);
                });

            migrationBuilder.CreateTable(
                name: "Exemplare",
                columns: table => new
                {
                    ExemplarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuchId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Beschädigung = table.Column<int>(type: "int", nullable: false),
                    InventarCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerschollenSeit = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exemplare", x => x.ExemplarId);
                    table.ForeignKey(
                        name: "FK_Exemplare_Buecher_BuchId",
                        column: x => x.BuchId,
                        principalTable: "Buecher",
                        principalColumn: "BuchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ausleihen",
                columns: table => new
                {
                    AusleiheId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExemplarId = table.Column<int>(type: "int", nullable: false),
                    AusleiherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AusleihDatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RueckgabeDatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Mahnstufe = table.Column<int>(type: "int", nullable: false),
                    LetzteMahnungAm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ausleihen", x => x.AusleiheId);
                    table.ForeignKey(
                        name: "FK_Ausleihen_Exemplare_ExemplarId",
                        column: x => x.ExemplarId,
                        principalTable: "Exemplare",
                        principalColumn: "ExemplarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Buecher",
                columns: new[] { "BuchId", "Autor", "Erscheinungsjahr", "Inhalt", "Isbn", "Klappentext", "Titel" },
                values: new object[,]
                {
                    { 1, "Irma Pince", 2021, null, "978-0000000001", "Ein Leitfaden für relationale Magie in Tabellenform.", "Zauberhafte Datenbanken" },
                    { 2, "Prof. Wandermann", 2023, null, "978-0000000002", "Model, View, Controller – ganz ohne Zauberstab.", "MVC für Muggel" },
                    { 3, "Eleonore Federkiel", 2020, null, "978-0000000003", "Zustände elegant verwalten: Ausleihbar bis Verkaufbar.", "Buchstabeneulen & Bibliothekslogik" },
                    { 4, "Klaus Spellman", 2024, null, "978-0000000004", "C#-Zaubertricks für sauberen Code.", "Hexenwerk mit C#" },
                    { 5, "Ilias Ink", 2022, null, "978-0000000005", "Fristen, Erinnerungen und Ordnung in der Bibliothek.", "Die Kunst der Mahnung" },
                    { 6, "Stefan Müller", 2025, null, "978-0000000006", "UI, das nach Hochwärts aussieht – ohne CSS-Schmerz.", "Bootstrap & Bannergold" },
                    { 7, "Valeria Wurm", 2021, null, "978-0000000007", "Code-First, Migrationen und magische Daten.", "EF Core Codex" },
                    { 8, "Robin Rot", 2019, null, "978-0000000008", "Kurzgeschichten aus dem Roten Haus.", "Die Leseratte liest" },
                    { 9, "Bruno Grün", 2018, null, "978-0000000009", "Geschichten für lange Winterabende.", "Vorlesebär Bruno" },
                    { 10, "Archiv Hochwärts", 2017, null, "978-0000000010", "Von ERM bis PK/FK – Grundlagen der Ordnung.", "Zauberschule der Tabellen" },
                    { 11, "Bibliotheksrat", 2016, null, "978-0000000011", "Wenn der Text hinten besser ist als vorne.", "Das große Klappentext-Kompendium" },
                    { 12, "Eleonore Federkiel", 2022, null, "978-0000000012", "Use Cases, Klassen, Zustände – verständlich.", "Eule Eleonore erklärt UML" }
                });

            migrationBuilder.InsertData(
                table: "Exemplare",
                columns: new[] { "ExemplarId", "Beschädigung", "BuchId", "InventarCode", "Status", "VerschollenSeit" },
                values: new object[,]
                {
                    { 101, 0, 1, "HW-DB-101", 0, null },
                    { 102, 4, 1, "HW-DB-102", 4, null },
                    { 103, 1, 1, "HW-DB-103", 1, null },
                    { 201, 0, 2, "HW-MVC-201", 0, null },
                    { 202, 2, 2, "HW-MVC-202", 2, null },
                    { 301, 1, 3, "HW-LOG-301", 0, null },
                    { 302, 4, 3, "HW-LOG-302", 4, null },
                    { 401, 0, 4, "HW-CS-401", 0, null },
                    { 402, 0, 4, "HW-CS-402", 1, null },
                    { 501, 0, 5, "HW-MAH-501", 0, null },
                    { 502, 3, 5, "HW-MAH-502", 0, null },
                    { 601, 1, 6, "HW-BS-601", 2, null },
                    { 602, 0, 6, "HW-BS-602", 0, null },
                    { 701, 0, 7, "HW-EF-701", 0, null },
                    { 702, 2, 7, "HW-EF-702", 3, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 801, 0, 8, "HW-ROT-801", 0, null },
                    { 901, 4, 9, "HW-GRN-901", 4, null },
                    { 1001, 0, 10, "HW-ERM-1001", 0, null },
                    { 1101, 0, 11, "HW-KT-1101", 0, null },
                    { 1201, 1, 12, "HW-UML-1201", 1, null },
                    { 1202, 0, 12, "HW-UML-1202", 0, null }
                });

            migrationBuilder.InsertData(
                table: "Ausleihen",
                columns: new[] { "AusleiheId", "AusleihDatum", "AusleiherName", "ExemplarId", "LetzteMahnungAm", "Mahnstufe", "RueckgabeDatum" },
                values: new object[,]
                {
                    { 9001, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), "Max Mustermann", 103, null, 0, null },
                    { 9002, new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Erika Beispiel", 402, null, 0, null },
                    { 9003, new DateTime(2025, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Tim Test", 1201, null, 0, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ausleihen_ExemplarId",
                table: "Ausleihen",
                column: "ExemplarId");

            migrationBuilder.CreateIndex(
                name: "IX_Buecher_Isbn",
                table: "Buecher",
                column: "Isbn");

            migrationBuilder.CreateIndex(
                name: "IX_Exemplare_BuchId",
                table: "Exemplare",
                column: "BuchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ausleihen");

            migrationBuilder.DropTable(
                name: "Exemplare");

            migrationBuilder.DropTable(
                name: "Buecher");
        }
    }
}

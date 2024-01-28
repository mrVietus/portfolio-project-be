﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crawler.Infrastructure.Persistance.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrawlResult");

            migrationBuilder.DropTable(
                name: "Crawl");

            migrationBuilder.CreateTable(
                name: "CrawlEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CrawlResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawlEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrawlResultEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopWordsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageWordsCount = table.Column<int>(type: "int", nullable: false),
                    CapturedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CrawlId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawlResultEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrawlResultEntity_CrawlEntity_CrawlId",
                        column: x => x.CrawlId,
                        principalTable: "CrawlEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrawlResultEntity_CrawlId",
                table: "CrawlResultEntity",
                column: "CrawlId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrawlResultEntity");

            migrationBuilder.DropTable(
                name: "CrawlEntity");

            migrationBuilder.CreateTable(
                name: "Crawl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CrawlResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crawl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrawlResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CrawlId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CapturedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageWordsCount = table.Column<int>(type: "int", nullable: false),
                    TopWordsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawlResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrawlResult_Crawl_CrawlId",
                        column: x => x.CrawlId,
                        principalTable: "Crawl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrawlResult_CrawlId",
                table: "CrawlResult",
                column: "CrawlId",
                unique: true);
        }
    }
}

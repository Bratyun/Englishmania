using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Englishmania.DAL.Migrations
{
    public partial class Database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Texts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    LevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopicVocabularies",
                columns: table => new
                {
                    TopicId = table.Column<int>(nullable: false),
                    VocabularyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicVocabularies", x => new { x.TopicId, x.VocabularyId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    LevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserVocabularies",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    VocabularyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVocabularies", x => new { x.UserId, x.VocabularyId });
                });

            migrationBuilder.CreateTable(
                name: "Vocabularies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false),
                    LevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vocabularies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    English = table.Column<string>(nullable: true),
                    Russian = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordTexts",
                columns: table => new
                {
                    WordId = table.Column<int>(nullable: false),
                    TextId = table.Column<int>(nullable: false),
                    NumberInText = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTexts", x => new { x.WordId, x.TextId });
                });

            migrationBuilder.CreateTable(
                name: "WordUsers",
                columns: table => new
                {
                    WordId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    LastUse = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordUsers", x => new { x.WordId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "WordVocabularies",
                columns: table => new
                {
                    WordId = table.Column<int>(nullable: false),
                    VocabularyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordVocabularies", x => new { x.WordId, x.VocabularyId });
                });

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Low" });

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Medium" });

            migrationBuilder.InsertData(
                table: "Levels",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "High" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true,
                filter: "[Login] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Texts");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "TopicVocabularies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserVocabularies");

            migrationBuilder.DropTable(
                name: "Vocabularies");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "WordTexts");

            migrationBuilder.DropTable(
                name: "WordUsers");

            migrationBuilder.DropTable(
                name: "WordVocabularies");
        }
    }
}

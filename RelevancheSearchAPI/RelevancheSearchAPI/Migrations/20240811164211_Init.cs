using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RelevancheSearchAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticlesCosSims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId1 = table.Column<int>(type: "int", nullable: false),
                    ArticleId2 = table.Column<int>(type: "int", nullable: false),
                    CosSimilarity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlesCosSims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleVectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Vector = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleVectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsCosSims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId1 = table.Column<int>(type: "int", nullable: false),
                    QuestionId2 = table.Column<int>(type: "int", nullable: false),
                    CosSimilarity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsCosSims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionVectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Vector = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionVectors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticlesCosSims_ArticleId1_ArticleId2",
                table: "ArticlesCosSims",
                columns: new[] { "ArticleId1", "ArticleId2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleVectors_ArticleId",
                table: "ArticleVectors",
                column: "ArticleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsCosSims_QuestionId1_QuestionId2",
                table: "QuestionsCosSims",
                columns: new[] { "QuestionId1", "QuestionId2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionVectors_QuestionId",
                table: "QuestionVectors",
                column: "QuestionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticlesCosSims");

            migrationBuilder.DropTable(
                name: "ArticleVectors");

            migrationBuilder.DropTable(
                name: "QuestionsCosSims");

            migrationBuilder.DropTable(
                name: "QuestionVectors");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace KelloWorld.Migrations
{/// <summary>
/// CReate unique index on Blog URL
/// </summary>
    public partial class BlogIndexes : Migration
    {
        const string IndexName = "IDX_ByUrl";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(IndexName, "Blogs", "Url", null, true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(IndexName, "Blogs");
        }
    }
}

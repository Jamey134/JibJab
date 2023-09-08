using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JibJab.Migrations
{
    public partial class _8thmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bloggers_Blogs_BlogPostId",
                table: "Bloggers");

            migrationBuilder.DropIndex(
                name: "IX_Bloggers_BlogPostId",
                table: "Bloggers");

            migrationBuilder.DropColumn(
                name: "BlogPostId",
                table: "Bloggers");

            migrationBuilder.CreateIndex(
                name: "IX_Bloggers_PostId",
                table: "Bloggers",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bloggers_Blogs_PostId",
                table: "Bloggers",
                column: "PostId",
                principalTable: "Blogs",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bloggers_Blogs_PostId",
                table: "Bloggers");

            migrationBuilder.DropIndex(
                name: "IX_Bloggers_PostId",
                table: "Bloggers");

            migrationBuilder.AddColumn<int>(
                name: "BlogPostId",
                table: "Bloggers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bloggers_BlogPostId",
                table: "Bloggers",
                column: "BlogPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bloggers_Blogs_BlogPostId",
                table: "Bloggers",
                column: "BlogPostId",
                principalTable: "Blogs",
                principalColumn: "PostId");
        }
    }
}

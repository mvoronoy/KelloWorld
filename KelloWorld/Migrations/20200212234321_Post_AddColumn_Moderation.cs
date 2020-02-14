using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace KelloWorld.Migrations
{
    public partial class Post_AddColumn_Moderation : Migration
    {
        private const string ColumnNameModerationDate = "ModerationDate";
        private const string ColumnNameModerationStatus = "ModerationStatus";
        private const string TableName = "Posts";
        private const string IndexName = "IDX_Posts_DateStatus";
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<DateTime>(
                name: ColumnNameModerationDate, 
                table: TableName, 
                nullable: false, 
                defaultValue: DateTime.Now);
            migrationBuilder.AddColumn<int>(
                name: ColumnNameModerationStatus,
                table: TableName,
                nullable: false,
                defaultValue: 0);
            migrationBuilder.CreateIndex(IndexName, TableName, 
                columns:new[] { ColumnNameModerationStatus, ColumnNameModerationDate});

        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(IndexName, TableName);

            migrationBuilder.DropColumn(ColumnNameModerationStatus, TableName);            migrationBuilder.DropIndex(IndexName, "Blogs");

            migrationBuilder.DropColumn(ColumnNameModerationDate, TableName);
        }
    }
}

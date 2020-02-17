using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MindHorizon.Data.Migrations
{
    public partial class UpdatePostTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInternal",
                table: "Post");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDateTime",
                table: "Post",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "CONVERT(datetime,GetDate())");

            migrationBuilder.AddColumn<string>(
                name: "Abstract",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abstract",
                table: "Post");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDateTime",
                table: "Post",
                nullable: true,
                defaultValueSql: "CONVERT(datetime,GetDate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInternal",
                table: "Post",
                nullable: false,
                defaultValue: false);
        }
    }
}

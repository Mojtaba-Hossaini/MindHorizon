﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MindHorizon.Data.Migrations
{
    public partial class UpdateAppUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDateTime",
                table: "AppUsers",
                nullable: true,
                defaultValueSql: "CONVERT(datetime,GetDate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AppUsers",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisterDateTime",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "CONVERT(datetime,GetDate())");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "AppUsers",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "1");
        }
    }
}

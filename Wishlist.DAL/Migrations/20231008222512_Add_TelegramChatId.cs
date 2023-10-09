﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wishlist.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_TelegramChatId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TelegramChatId",
                table: "Users",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramChatId",
                table: "Users");
        }
    }
}

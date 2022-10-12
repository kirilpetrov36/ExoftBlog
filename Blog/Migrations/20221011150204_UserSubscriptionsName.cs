using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class UserSubscriptionsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptions_AspNetUsers_FollowedUserId",
                table: "UserSubscriptions");

            migrationBuilder.RenameColumn(
                name: "FollowedUserId",
                table: "UserSubscriptions",
                newName: "UserToSubscribeId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscriptions_FollowedUserId",
                table: "UserSubscriptions",
                newName: "IX_UserSubscriptions_UserToSubscribeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptions_AspNetUsers_UserToSubscribeId",
                table: "UserSubscriptions",
                column: "UserToSubscribeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscriptions_AspNetUsers_UserToSubscribeId",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserSubscriptions");

            migrationBuilder.RenameColumn(
                name: "UserToSubscribeId",
                table: "UserSubscriptions",
                newName: "FollowedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSubscriptions_UserToSubscribeId",
                table: "UserSubscriptions",
                newName: "IX_UserSubscriptions_FollowedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscriptions_AspNetUsers_FollowedUserId",
                table: "UserSubscriptions",
                column: "FollowedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

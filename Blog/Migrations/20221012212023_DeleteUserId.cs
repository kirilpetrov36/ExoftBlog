using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    public partial class DeleteUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleLikes_AspNetUsers_UserId",
                table: "ArticleLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_AspNetUsers_UserId",
                table: "CommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_AspNetUsers_UserId",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_UserFiles_UserId",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_CommentLikes_UserId",
                table: "CommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_ArticleLikes_UserId",
                table: "ArticleLikes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CommentLikes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ArticleLikes");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_CreatedBy",
                table: "UserFiles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedBy",
                table: "Comments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_CreatedBy",
                table: "CommentLikes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLikes_CreatedBy",
                table: "ArticleLikes",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleLikes_AspNetUsers_CreatedBy",
                table: "ArticleLikes",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_AspNetUsers_CreatedBy",
                table: "CommentLikes",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedBy",
                table: "Comments",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFiles_AspNetUsers_CreatedBy",
                table: "UserFiles",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleLikes_AspNetUsers_CreatedBy",
                table: "ArticleLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_AspNetUsers_CreatedBy",
                table: "CommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_CreatedBy",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFiles_AspNetUsers_CreatedBy",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_UserFiles_CreatedBy",
                table: "UserFiles");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CreatedBy",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_CommentLikes_CreatedBy",
                table: "CommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_ArticleLikes_CreatedBy",
                table: "ArticleLikes");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserFiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CommentLikes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ArticleLikes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_UserId",
                table: "UserFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_UserId",
                table: "CommentLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleLikes_UserId",
                table: "ArticleLikes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleLikes_AspNetUsers_UserId",
                table: "ArticleLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_AspNetUsers_UserId",
                table: "CommentLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFiles_AspNetUsers_UserId",
                table: "UserFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

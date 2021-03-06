using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class Rebase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewsModel_UserReview_user_reviewId",
                table: "ReviewsModel");

            migrationBuilder.DropTable(
                name: "UserReview");

            migrationBuilder.DropIndex(
                name: "IX_ReviewsModel_user_reviewId",
                table: "ReviewsModel");

            migrationBuilder.DropColumn(
                name: "user_reviewId",
                table: "ReviewsModel");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ReviewsModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "ReviewsModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "ReviewsModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "writtenBy",
                table: "ReviewsModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ReviewsModel");

            migrationBuilder.DropColumn(
                name: "comment",
                table: "ReviewsModel");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "ReviewsModel");

            migrationBuilder.DropColumn(
                name: "writtenBy",
                table: "ReviewsModel");

            migrationBuilder.AddColumn<int>(
                name: "user_reviewId",
                table: "ReviewsModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_AT = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: false),
                    writtenBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReview", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewsModel_user_reviewId",
                table: "ReviewsModel",
                column: "user_reviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewsModel_UserReview_user_reviewId",
                table: "ReviewsModel",
                column: "user_reviewId",
                principalTable: "UserReview",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

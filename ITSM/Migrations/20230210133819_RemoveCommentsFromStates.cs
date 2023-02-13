using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSM.Migrations
{
    public partial class RemoveCommentsFromStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_States_StateId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_StateId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StateId",
                table: "Comments",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_States_StateId",
                table: "Comments",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSM.Migrations
{
    public partial class TaskType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "RemainingWork",
                table: "WorkItems",
                type: "TINYINT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(5,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RemainingWork",
                table: "WorkItems",
                type: "DECIMAL(5,2)",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "TINYINT",
                oldNullable: true);
        }
    }
}

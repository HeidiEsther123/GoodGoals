using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodGoals.Migrations
{
    /// <inheritdoc />
    public partial class AgregarGoalIdANotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoalId",
                table: "Notes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_GoalId",
                table: "Notes",
                column: "GoalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Goals_GoalId",
                table: "Notes",
                column: "GoalId",
                principalTable: "Goals",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Goals_GoalId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_GoalId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "GoalId",
                table: "Notes");
        }
    }
}

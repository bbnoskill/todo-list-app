using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_TodoLists_TodoListEntityId",
                table: "TodoTasks");

            migrationBuilder.DropIndex(
                name: "IX_TodoTasks_TodoListEntityId",
                table: "TodoTasks");

            migrationBuilder.DropColumn(
                name: "TodoListEntityId",
                table: "TodoTasks");

            migrationBuilder.CreateTable(
                name: "TaskHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TodoListId",
                table: "TodoTasks",
                column: "TodoListId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_TodoLists_TodoListId",
                table: "TodoTasks",
                column: "TodoListId",
                principalTable: "TodoLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTasks_TodoLists_TodoListId",
                table: "TodoTasks");

            migrationBuilder.DropTable(
                name: "TaskHistories");

            migrationBuilder.DropIndex(
                name: "IX_TodoTasks_TodoListId",
                table: "TodoTasks");

            migrationBuilder.AddColumn<int>(
                name: "TodoListEntityId",
                table: "TodoTasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TodoListEntityId",
                table: "TodoTasks",
                column: "TodoListEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTasks_TodoLists_TodoListEntityId",
                table: "TodoTasks",
                column: "TodoListEntityId",
                principalTable: "TodoLists",
                principalColumn: "Id");
        }
    }
}

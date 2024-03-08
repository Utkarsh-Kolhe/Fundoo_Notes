using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository_Layer.Migrations
{
    /// <inheritdoc />
    public partial class dds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Registrations_Details_UsersId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UsersId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Notes");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Registrations_Details_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "Registrations_Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Registrations_Details_UserId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UserId",
                table: "Notes");

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UsersId",
                table: "Notes",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Registrations_Details_UsersId",
                table: "Notes",
                column: "UsersId",
                principalTable: "Registrations_Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

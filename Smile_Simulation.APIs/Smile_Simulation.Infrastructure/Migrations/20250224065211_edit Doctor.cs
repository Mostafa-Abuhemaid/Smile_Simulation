using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smile_Simulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "qualification",
                table: "Doctors",
                newName: "Qualification");

            migrationBuilder.AlterColumn<int>(
                name: "Experience",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Qualification",
                table: "Doctors",
                newName: "qualification");

            migrationBuilder.AlterColumn<string>(
                name: "Experience",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}

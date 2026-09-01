using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDiario.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "EntradasDiario",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "EntradasDiario");
        }
    }
}

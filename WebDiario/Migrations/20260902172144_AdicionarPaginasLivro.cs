using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDiario.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPaginasLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaginasLidas",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPaginas",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaginasLidas",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "TotalPaginas",
                table: "Livros");
        }
    }
}

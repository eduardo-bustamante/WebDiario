using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDiario.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCapa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoCapa",
                table: "Livros",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoCapa",
                table: "Livros");
        }
    }
}

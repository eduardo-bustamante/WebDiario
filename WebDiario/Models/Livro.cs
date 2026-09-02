using System.ComponentModel.DataAnnotations;

namespace WebDiario.Models;

public class Livro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Autor { get; set; }

    [StringLength(50)]
    public string? Categoria { get; set; }

    public string Status { get; set; } = "Quero Ler";

    [Range(1, 5)]
    public int? Avaliacao { get; set; }

    // --- Novos campos de páginas ---
    [Display(Name = "Páginas Lidas")]
    [Range(0, 5000, ErrorMessage = "Valor inválido de páginas lidas.")]
    public int PaginasLidas { get; set; } = 0;

    [Display(Name = "Total de Páginas")]
    [Range(0, 5000, ErrorMessage = "Valor inválido para o total de páginas.")]
    public int TotalPaginas { get; set; } = 0;

    // Propriedade calculada (não cria coluna no banco)
    public int PercentualLido => TotalPaginas > 0
        ? Math.Min(100, (int)Math.Round((double)PaginasLidas / TotalPaginas * 100))
        : (Status == "Lido" ? 100 : 0);
    // ---------------------------------

    public string? Notas { get; set; }

    [DataType(DataType.Date)]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public string? UsuarioId { get; set; }

    [Display(Name = "Capa do Livro")]
    public string? FotoCapa { get; set; }
}
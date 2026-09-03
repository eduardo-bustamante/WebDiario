using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDiario.Models;

public class Livro
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título não pode exceder 150 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "O nome do autor não pode exceder 100 caracteres.")]
    [Display(Name = "Autor")]
    public string? Autor { get; set; }

    [StringLength(50)]
    [Display(Name = "Gênero / Categoria")]
    public string? Categoria { get; set; }

    [Display(Name = "Capa do Livro")]
    public string? FotoCapa { get; set; }

    [Display(Name = "Páginas Lidas")]
    [Range(0, 10000, ErrorMessage = "O número de páginas lidas deve ser positivo.")]
    public int PaginasLidas { get; set; } = 0;

    [Display(Name = "Total de Páginas")]
    [Range(0, 10000, ErrorMessage = "O total de páginas deve ser positivo.")]
    public int TotalPaginas { get; set; } = 0;

    [Required]
    [StringLength(20)]
    [Display(Name = "Status de Leitura")]
    public string Status { get; set; } = "Quero Ler"; // Quero Ler, Lendo, Lido, Abandonado

    [Range(1, 5, ErrorMessage = "A avaliação deve ser entre 1 e 5 estrelas.")]
    [Display(Name = "Avaliação")]
    public int? Avaliacao { get; set; }

    [Display(Name = "Notas / Resenha")]
    [DataType(DataType.MultilineText)]
    public string? Notas { get; set; }

    [Display(Name = "Data de Cadastro")]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    // Vínculo com o usuário do Identity
    public string? UsuarioId { get; set; }

    // Propriedade calculada de progresso (não cria coluna no banco de dados)
    [NotMapped]
    public int PercentualLido => TotalPaginas > 0
        ? Math.Min(100, (int)Math.Round((double)PaginasLidas / TotalPaginas * 100))
        : (Status == "Lido" ? 100 : 0);
}
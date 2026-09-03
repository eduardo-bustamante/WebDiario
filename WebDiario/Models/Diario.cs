using System.ComponentModel.DataAnnotations;

namespace WebDiario.Models;

public class Diario
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(150, ErrorMessage = "O título não pode exceder 150 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O conteúdo da anotação é obrigatório.")]
    [Display(Name = "Conteúdo")]
    [DataType(DataType.MultilineText)]
    public string Conteudo { get; set; } = string.Empty;

    [Display(Name = "Data do Registro")]
    [DataType(DataType.Date)]
    public DateTime DataRegistro { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Selecione como você está se sentindo.")]
    [Range(1, 5, ErrorMessage = "O humor deve estar entre 1 e 5.")]
    [Display(Name = "Estágio de Humor")]
    public int NivelHumor { get; set; } = 3; // 1: Muito Difícil, 2: Para Baixo, 3: Neutro, 4: Bem, 5: Excelente

    [Display(Name = "Data de Criação")]
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    // Vínculo com o usuário do Identity
    public string? UsuarioId { get; set; }
}
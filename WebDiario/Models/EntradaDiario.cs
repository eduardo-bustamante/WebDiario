using System.ComponentModel.DataAnnotations;

namespace WebDiario.Models
{
    public class EntradaDiario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título não pode passar de 100 caracteres")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O conteúdo é obrigatório")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime DataCriacao { get; set; } = DateTime.Today;

        [Display(Name = "Sentimento / Humor")]
        public string? Humor { get; set; } // Ex: Feliz, Reflexivo, Produtivo, Cansado

        // Identificador único do usuário dono deste registro
        public string? UsuarioId { get; set; }
    }
}

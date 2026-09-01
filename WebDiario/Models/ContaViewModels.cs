using System.ComponentModel.DataAnnotations;

namespace WebDiario.Models;

public class RegistroViewModel
{
    [Required(ErrorMessage = "O nome de usuário é obrigatório")]
    [StringLength(30, ErrorMessage = "O usuário deve ter entre {2} e {1} caracteres.", MinimumLength = 3)]
    [Display(Name = "Usuário")]
    public string Usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Senha")]
    [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Informe seu usuário")]
    [Display(Name = "Usuário")]
    public string Usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe sua senha")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;

    [Display(Name = "Lembrar de mim")]
    public bool LembrarMe { get; set; }

    public string? ReturnUrl { get; set; }
}
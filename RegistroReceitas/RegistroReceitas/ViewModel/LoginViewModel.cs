using System.ComponentModel.DataAnnotations;

namespace RegistroReceitas.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "O login é obrigatório.")]
    [Display(Name = "Login")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;
}

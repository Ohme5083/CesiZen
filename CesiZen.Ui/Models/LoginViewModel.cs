using System.ComponentModel.DataAnnotations;

namespace CesiZen.Ui.Models;
public class LoginViewModel
{
    [Required]
    [Display(Name = "Nom d'utilisateur ou Email")]
    public string UserNameOrEmail { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; }

    [Display(Name = "Se souvenir de moi")]
    public bool RememberMe { get; set; }
}
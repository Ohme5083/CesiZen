using System.ComponentModel.DataAnnotations;

namespace CesiZen.Ui.Models;
public class RegisterViewModel
{
    [Required]
    [Display(Name = "Prénom")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Nom")]
    public string LastName { get; set; }

    [Required]
    [Display(Name = "Nom d'utilisateur")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmer le mot de passe")]
    [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
    public string ConfirmPassword { get; set; }
}

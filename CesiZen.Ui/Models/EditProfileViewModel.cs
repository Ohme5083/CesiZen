using System.ComponentModel.DataAnnotations;

namespace CesiZen.Ui.Models;
public class EditProfileViewModel
{
    [Required]
    [Display(Name = "Prénom")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Nom")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Adresse email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Nom d'utilisateur")]
    public string UserName { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Nouveau mot de passe")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirmer le mot de passe")]
    [Compare("NewPassword", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas.")]
    public string? ConfirmPassword { get; set; }

}

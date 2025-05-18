namespace CesiZen.Ui.Models;

public class ChangeUserRoleViewModel
{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public List<string> AllRoles { get; set; } = new();
    public List<string> UserRoles { get; set; } = new();
}

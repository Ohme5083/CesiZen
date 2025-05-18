﻿namespace CesiZen.Ui.Models;

public class UserWithRolesViewModel
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new();
}

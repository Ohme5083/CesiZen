using Microsoft.AspNetCore.Identity;

namespace CesiZen.Data.Models
{
    public class UserModel : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool Active { get; set; } = true;
    }
}

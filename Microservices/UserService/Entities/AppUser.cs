using Microsoft.AspNetCore.Identity;

namespace UserService.Entities
{
    public class AppUser: IdentityUser
    {
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual DateTime? RegistrationDate { get; set; }
    }
}

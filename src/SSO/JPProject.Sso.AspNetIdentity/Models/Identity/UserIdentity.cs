using JPProject.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace JPProject.Sso.AspNetIdentity.Models.Identity
{
    public class UserIdentity : IdentityUser, IDomainUser
    {
        public void ConfirmEmail()
        {
            EmailConfirmed = true;
        }
    }

    public class RoleIdentity : IdentityRole
    {
        public RoleIdentity() : base() { }
        public RoleIdentity(string name) : base(name) { }
    }
}
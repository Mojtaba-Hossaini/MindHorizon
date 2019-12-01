using Microsoft.AspNetCore.Identity;

namespace MindHorizon.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<string>
    {
        public virtual Role Role { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace MindHorizon.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }
}

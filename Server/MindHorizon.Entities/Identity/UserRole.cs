using Microsoft.AspNetCore.Identity;

namespace MindHorizon.Entities.Identity
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}

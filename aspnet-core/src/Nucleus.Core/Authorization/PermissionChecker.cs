using Abp.Authorization;
using Nucleus.Authorization.Roles;
using Nucleus.Authorization.Users;

namespace Nucleus.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}

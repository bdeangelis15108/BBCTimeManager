using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Nucleus.Authorization.Users;
using Nucleus.MultiTenancy;

namespace Nucleus.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}
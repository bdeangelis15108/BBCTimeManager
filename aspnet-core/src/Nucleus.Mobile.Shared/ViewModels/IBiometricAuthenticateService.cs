using Nucleus.ViewModels.Base;
using System;
using System.Threading.Tasks;

namespace Nucleus.ViewModels
{
    public interface IBiometricAuthenticateService
    {
        String GetAuthenticationType();
        Task<bool> AuthenticateUserIDWithTouchID();
        bool fingerprintEnabled();
    }
}

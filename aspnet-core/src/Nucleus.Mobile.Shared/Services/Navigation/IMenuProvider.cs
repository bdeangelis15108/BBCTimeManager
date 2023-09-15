using System.Collections.Generic;
using MvvmHelpers;
using Nucleus.Models.NavigationMenu;

namespace Nucleus.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}
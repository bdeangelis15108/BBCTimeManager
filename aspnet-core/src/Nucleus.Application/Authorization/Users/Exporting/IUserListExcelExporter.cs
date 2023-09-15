using System.Collections.Generic;
using Nucleus.Authorization.Users.Dto;
using Nucleus.Dto;

namespace Nucleus.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}
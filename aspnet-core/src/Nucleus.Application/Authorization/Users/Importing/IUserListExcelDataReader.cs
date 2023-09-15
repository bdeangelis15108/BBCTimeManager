using System.Collections.Generic;
using Nucleus.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Nucleus.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}

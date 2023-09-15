using System.Collections.Generic;
using Nucleus.Authorization.Users.Importing.Dto;
using Nucleus.Dto;

namespace Nucleus.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}

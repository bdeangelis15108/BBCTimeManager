using System.Collections.Generic;
using Nucleus.Account.Dtos;
using Nucleus.Dto;

namespace Nucleus.Account.Exporting
{
    public interface IAccountsExcelExporter
    {
        FileDto ExportToFile(List<GetAccountsForViewDto> accounts);
    }
}
using System.Collections.Generic;
using Nucleus.Address.Dtos;
using Nucleus.Dto;

namespace Nucleus.Address.Exporting
{
    public interface IAddressesesExcelExporter
    {
        FileDto ExportToFile(List<GetAddressesForViewDto> addresseses);
    }
}
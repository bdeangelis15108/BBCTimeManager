using System.Collections.Generic;
using Nucleus.PRCLASSES.Dtos;
using Nucleus.Dto;

namespace Nucleus.PRCLASSES.Exporting
{
    public interface IPRCLASSsExcelExporter
    {
        FileDto ExportToFile(List<GetPRCLASSForViewDto> prclasSs);
    }
}
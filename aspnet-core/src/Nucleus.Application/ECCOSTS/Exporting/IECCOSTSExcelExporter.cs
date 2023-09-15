using System.Collections.Generic;
using Nucleus.ECCOSTS.Dtos;
using Nucleus.Dto;

namespace Nucleus.ECCOSTS.Exporting
{
    public interface IECCOSTSExcelExporter
    {
        FileDto ExportToFile(List<GetECCOSTForViewDto> eccosts);
    }
}
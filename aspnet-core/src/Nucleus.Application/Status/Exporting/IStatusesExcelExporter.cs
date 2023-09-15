using System.Collections.Generic;
using Nucleus.Status.Dtos;
using Nucleus.Dto;

namespace Nucleus.Status.Exporting
{
    public interface IStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetStatusesForViewDto> statuses);
    }
}
using System.Collections.Generic;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.Dto;

namespace Nucleus.StatusUpdate.Exporting
{
    public interface IStatusUpdatesExcelExporter
    {
        FileDto ExportToFile(List<GetStatusUpdatesForViewDto> statusUpdates);
    }
}
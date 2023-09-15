using System.Collections.Generic;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.Dto;

namespace Nucleus.ResourceWorkerInfo.Exporting
{
    public interface IResourceWorkerInfosesExcelExporter
    {
        FileDto ExportToFile(List<GetResourceWorkerInfosForViewDto> resourceWorkerInfoses);
    }
}
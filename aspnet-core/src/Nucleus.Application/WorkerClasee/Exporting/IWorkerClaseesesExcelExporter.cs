using System.Collections.Generic;
using Nucleus.WorkerClasee.Dtos;
using Nucleus.Dto;

namespace Nucleus.WorkerClasee.Exporting
{
    public interface IWorkerClaseesesExcelExporter
    {
        FileDto ExportToFile(List<GetWorkerClaseesForViewDto> workerClaseeses);
    }
}
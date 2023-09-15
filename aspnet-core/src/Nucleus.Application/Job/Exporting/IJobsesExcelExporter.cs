using System.Collections.Generic;
using Nucleus.Job.Dtos;
using Nucleus.Dto;

namespace Nucleus.Job.Exporting
{
    public interface IJobsesExcelExporter
    {
        FileDto ExportToFile(List<GetJobsForViewDto> jobses);
    }
}
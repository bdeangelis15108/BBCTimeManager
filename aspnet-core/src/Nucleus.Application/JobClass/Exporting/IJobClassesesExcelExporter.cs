using System.Collections.Generic;
using Nucleus.JobClass.Dtos;
using Nucleus.Dto;

namespace Nucleus.JobClass.Exporting
{
    public interface IJobClassesesExcelExporter
    {
        FileDto ExportToFile(List<GetJobClassesForViewDto> jobClasseses);
    }
}
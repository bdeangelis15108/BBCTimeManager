using System.Collections.Generic;
using Nucleus.JobUnion.Dtos;
using Nucleus.Dto;

namespace Nucleus.JobUnion.Exporting
{
    public interface IJobUnionsExcelExporter
    {
        FileDto ExportToFile(List<GetJobUnionsForViewDto> jobUnions);
    }
}
using System.Collections.Generic;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.Dto;

namespace Nucleus.JobPhaseCode.Exporting
{
    public interface IJobPhaseCodesExcelExporter
    {
        FileDto ExportToFile(List<GetJobPhaseCodesForViewDto> jobPhaseCodes);
    }
}
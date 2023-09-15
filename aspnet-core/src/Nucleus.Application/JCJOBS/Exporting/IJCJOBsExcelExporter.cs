using System.Collections.Generic;
using Nucleus.JCJOBS.Dtos;
using Nucleus.Dto;

namespace Nucleus.JCJOBS.Exporting
{
    public interface IJCJOBsExcelExporter
    {
        FileDto ExportToFile(List<GetJCJOBForViewDto> jcjoBs);
    }
}
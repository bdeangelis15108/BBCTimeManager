using System.Collections.Generic;
using Nucleus.JCUNIONS.Dtos;
using Nucleus.Dto;

namespace Nucleus.JCUNIONS.Exporting
{
    public interface IJCUNIONsExcelExporter
    {
        FileDto ExportToFile(List<GetJCUNIONForViewDto> jcunioNs);
    }
}
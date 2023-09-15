using System.Collections.Generic;
using Nucleus.JCCAT.Dtos;
using Nucleus.Dto;

namespace Nucleus.JCCAT.Exporting
{
    public interface IJACCATsExcelExporter
    {
        FileDto ExportToFile(List<GetJACCATForViewDto> jaccaTs);
    }
}
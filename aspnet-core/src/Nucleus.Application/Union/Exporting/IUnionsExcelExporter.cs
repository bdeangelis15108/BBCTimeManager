using System.Collections.Generic;
using Nucleus.Union.Dtos;
using Nucleus.Dto;

namespace Nucleus.Union.Exporting
{
    public interface IUnionsExcelExporter
    {
        FileDto ExportToFile(List<GetUnionsForViewDto> unions);
    }
}
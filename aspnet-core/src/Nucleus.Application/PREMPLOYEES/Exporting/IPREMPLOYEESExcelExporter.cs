using System.Collections.Generic;
using Nucleus.PREMPLOYEES.Dtos;
using Nucleus.Dto;

namespace Nucleus.PREMPLOYEES.Exporting
{
    public interface IPREMPLOYEESExcelExporter
    {
        FileDto ExportToFile(List<GetPREMPLOYEEForViewDto> premployees);
    }
}
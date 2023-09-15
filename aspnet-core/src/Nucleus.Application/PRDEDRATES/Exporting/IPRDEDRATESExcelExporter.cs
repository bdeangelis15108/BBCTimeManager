using System.Collections.Generic;
using Nucleus.PRDEDRATES.Dtos;
using Nucleus.Dto;

namespace Nucleus.PRDEDRATES.Exporting
{
    public interface IPRDEDRATESExcelExporter
    {
        FileDto ExportToFile(List<GetPRDEDRATEForViewDto> prdedrates);
    }
}
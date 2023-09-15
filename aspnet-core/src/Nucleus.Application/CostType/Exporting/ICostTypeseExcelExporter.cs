using System.Collections.Generic;
using Nucleus.CostType.Dtos;
using Nucleus.Dto;

namespace Nucleus.CostType.Exporting
{
    public interface ICostTypeseExcelExporter
    {
        FileDto ExportToFile(List<GetCostTypesForViewDto> costTypese);
    }
}
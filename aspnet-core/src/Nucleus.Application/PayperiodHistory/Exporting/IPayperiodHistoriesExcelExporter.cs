using System.Collections.Generic;
using Nucleus.PayperiodHistory.Dtos;
using Nucleus.Dto;

namespace Nucleus.PayperiodHistory.Exporting
{
    public interface IPayperiodHistoriesExcelExporter
    {
        FileDto ExportToFile(List<GetPayperiodHistoriesForViewDto> payperiodHistories);
    }
}
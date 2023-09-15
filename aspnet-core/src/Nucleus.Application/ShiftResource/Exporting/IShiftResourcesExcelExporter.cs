using System.Collections.Generic;
using Nucleus.ShiftResource.Dtos;
using Nucleus.Dto;

namespace Nucleus.ShiftResource.Exporting
{
    public interface IShiftResourcesExcelExporter
    {
        FileDto ExportToFile(List<GetShiftResourcesForViewDto> shiftResources);
    }
}
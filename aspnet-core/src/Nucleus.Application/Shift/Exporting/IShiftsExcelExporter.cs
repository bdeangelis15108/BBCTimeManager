using System.Collections.Generic;
using Nucleus.Shift.Dtos;
using Nucleus.Dto;

namespace Nucleus.Shift.Exporting
{
    public interface IShiftsExcelExporter
    {
        FileDto ExportToFile(List<GetShiftsForViewDto> shifts);
    }
}
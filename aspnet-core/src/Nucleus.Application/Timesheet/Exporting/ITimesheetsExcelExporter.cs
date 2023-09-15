using System.Collections.Generic;
using Nucleus.Timesheet.Dtos;
using Nucleus.Dto;

namespace Nucleus.Timesheet.Exporting
{
    public interface ITimesheetsExcelExporter
    {
        FileDto ExportToFile(List<GetTimesheetsForViewDto> timesheets);
    }
}
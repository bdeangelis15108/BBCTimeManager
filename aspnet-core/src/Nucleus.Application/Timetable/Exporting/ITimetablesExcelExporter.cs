using System.Collections.Generic;
using Nucleus.Timetable.Dtos;
using Nucleus.Dto;

namespace Nucleus.Timetable.Exporting
{
    public interface ITimetablesExcelExporter
    {
        FileDto ExportToFile(List<GetTimetablesForViewDto> timetables, string[] days);
    }
}
using System.Collections.Generic;
using Nucleus.EquipTimetable.Dtos;
using Nucleus.Dto;

namespace Nucleus.EquipTimetable.Exporting
{
    public interface IEquipTimetablesExcelExporter
    {
        FileDto ExportToFile(List<GetEquipTimetablesForViewDto> equipTimetables, string[] days);
    }
}
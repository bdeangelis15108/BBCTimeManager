using System.Collections.Generic;
using Nucleus.EQUIPMENTS.Dtos;
using Nucleus.Dto;

namespace Nucleus.EQUIPMENTS.Exporting
{
    public interface IEQUIPMENTSExcelExporter
    {
        FileDto ExportToFile(List<GetEQUIPMENTForViewDto> equipments);
    }
}
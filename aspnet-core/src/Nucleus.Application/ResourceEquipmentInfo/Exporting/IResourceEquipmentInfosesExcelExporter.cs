using System.Collections.Generic;
using Nucleus.ResourceEquipmentInfo.Dtos;
using Nucleus.Dto;

namespace Nucleus.ResourceEquipmentInfo.Exporting
{
    public interface IResourceEquipmentInfosesExcelExporter
    {
        FileDto ExportToFile(List<GetResourceEquipmentInfosForViewDto> resourceEquipmentInfoses);
    }
}
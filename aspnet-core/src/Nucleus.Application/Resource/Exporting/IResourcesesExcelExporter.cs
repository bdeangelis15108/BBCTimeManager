using System.Collections.Generic;
using Nucleus.Resource.Dtos;
using Nucleus.Dto;

namespace Nucleus.Resource.Exporting
{
    public interface IResourcesesExcelExporter
    {
        FileDto ExportToFile(List<GetResourcesForViewDto> resourceses);
    }
}
using System.Collections.Generic;
using Nucleus.JobCategory.Dtos;
using Nucleus.Dto;

namespace Nucleus.JobCategory.Exporting
{
    public interface IJobCategoriesExcelExporter
    {
        FileDto ExportToFile(List<GetJobCategoriesForViewDto> jobCategories);
    }
}
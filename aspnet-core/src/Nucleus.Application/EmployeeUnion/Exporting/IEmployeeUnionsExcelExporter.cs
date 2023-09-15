using System.Collections.Generic;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.Dto;

namespace Nucleus.EmployeeUnion.Exporting
{
    public interface IEmployeeUnionsExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeUnionsForViewDto> employeeUnions);
    }
}
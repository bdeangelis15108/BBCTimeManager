using System.Collections.Generic;
using Nucleus.ExpenseType.Dtos;
using Nucleus.Dto;

namespace Nucleus.ExpenseType.Exporting
{
    public interface IExpenseTypesesExcelExporter
    {
        FileDto ExportToFile(List<GetExpenseTypesForViewDto> expenseTypeses);
    }
}
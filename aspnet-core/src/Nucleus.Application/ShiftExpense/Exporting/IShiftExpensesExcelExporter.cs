using System.Collections.Generic;
using Nucleus.ShiftExpense.Dtos;
using Nucleus.Dto;

namespace Nucleus.ShiftExpense.Exporting
{
    public interface IShiftExpensesExcelExporter
    {
        FileDto ExportToFile(List<GetShiftExpensesForViewDto> shiftExpenses);
    }
}
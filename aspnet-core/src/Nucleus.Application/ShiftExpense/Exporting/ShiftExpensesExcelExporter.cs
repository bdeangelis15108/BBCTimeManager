using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.ShiftExpense.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.ShiftExpense.Exporting
{
    public class ShiftExpensesExcelExporter : EpPlusExcelExporterBase, IShiftExpensesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShiftExpensesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShiftExpensesForViewDto> shiftExpenses)
        {
            return CreateExcelPackage(
                "ShiftExpenses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ShiftExpenses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Amount"),
                        (L("ShiftResources")) + L("Name"),
                        (L("ExpenseTypes")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, shiftExpenses,
                        _ => _.ShiftExpenses.Name,
                        _ => _.ShiftExpenses.Amount,
                        _ => _.ShiftResourcesName,
                        _ => _.ExpenseTypesName
                        );

					
					
                });
        }
    }
}

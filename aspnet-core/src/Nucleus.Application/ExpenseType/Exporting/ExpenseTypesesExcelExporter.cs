using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.ExpenseType.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.ExpenseType.Exporting
{
    public class ExpenseTypesesExcelExporter : EpPlusExcelExporterBase, IExpenseTypesesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ExpenseTypesesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetExpenseTypesForViewDto> expenseTypeses)
        {
            return CreateExcelPackage(
                "ExpenseTypeses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ExpenseTypeses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Code"),
                        L("Icon")
                        );

                    AddObjects(
                        sheet, 2, expenseTypeses,
                        _ => _.ExpenseTypes.Name,
                        _ => _.ExpenseTypes.Description,
                        _ => _.ExpenseTypes.Code,
                        _ => _.ExpenseTypes.Icon
                        );

					
					
                });
        }
    }
}

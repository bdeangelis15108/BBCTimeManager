using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.EmployeeUnion.Exporting
{
    public class EmployeeUnionsExcelExporter : EpPlusExcelExporterBase, IEmployeeUnionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeUnionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeUnionsForViewDto> employeeUnions)
        {
            return CreateExcelPackage(
                "EmployeeUnions.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeUnions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LocalNumber"),
                        (L("Unions")) + L("Number"),
                        (L("Resources")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, employeeUnions,
                        _ => _.EmployeeUnions.LocalNumber,
                        _ => _.UnionsNumber,
                        _ => _.ResourcesName
                        );

					
					
                });
        }
    }
}

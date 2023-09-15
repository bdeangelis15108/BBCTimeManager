using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.ShiftResource.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.ShiftResource.Exporting
{
    public class ShiftResourcesExcelExporter : EpPlusExcelExporterBase, IShiftResourcesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShiftResourcesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShiftResourcesForViewDto> shiftResources)
        {
            return CreateExcelPackage(
                "ShiftResources.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ShiftResources"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("HoursWorked"),
                        L("Name"),
                        (L("Resources")) + L("Name"),
                        (L("PayTypes")) + L("Code"),
                        (L("JobPhaseCodes")) + L("Name"),
                        (L("JobCategories")) + L("Name"),
                        (L("Timesheets")) + L("Name"),
                        (L("Shifts")) + L("Name"),
                        (L("WorkerClasees")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, shiftResources,
                        _ => _.ShiftResources.HoursWorked,
                        _ => _.ShiftResources.Name,
                        _ => _.ResourcesName,
                        _ => _.PayTypesCode,
                        _ => _.JobPhaseCodesName,
                        _ => _.JobCategoriesName,
                        _ => _.TimesheetsName,
                        _ => _.ShiftsName,
                        _ => _.WorkerClaseesName
                        );

					
					
                });
        }
    }
}

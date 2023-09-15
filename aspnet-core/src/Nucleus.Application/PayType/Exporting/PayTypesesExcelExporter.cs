using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.PayType.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.PayType.Exporting
{
    public class PayTypesesExcelExporter : EpPlusExcelExporterBase, IPayTypesesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PayTypesesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPayTypesForViewDto> payTypeses)
        {
            return CreateExcelPackage(
                "PayTypeses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PayTypeses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Description"),
                        L("Multiplier"),
                        L("Section1"),
                        L("Section2"),
                        L("Section3")
                        );

                    AddObjects(
                        sheet, 2, payTypeses,
                        _ => _.PayTypes.Code,
                        _ => _.PayTypes.Description,
                        _ => _.PayTypes.Multiplier,
                        _ => _.PayTypes.Section1,
                        _ => _.PayTypes.Section2,
                        _ => _.PayTypes.Section3
                        );

					
					
                });
        }
    }
}

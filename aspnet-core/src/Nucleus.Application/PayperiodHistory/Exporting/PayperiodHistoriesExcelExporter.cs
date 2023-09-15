using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.PayperiodHistory.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.PayperiodHistory.Exporting
{
    public class PayperiodHistoriesExcelExporter : EpPlusExcelExporterBase, IPayperiodHistoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PayperiodHistoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPayperiodHistoriesForViewDto> payperiodHistories)
        {
            return CreateExcelPackage(
                "PayperiodHistories.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PayperiodHistories"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("period"),
                        L("active"),
                        (L("PayPeriods")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, payperiodHistories,
                        _ => _.PayperiodHistories.period,
                        _ => _.PayperiodHistories.active,
                        _ => _.PayPeriodsName
                        );

					
					
                });
        }
    }
}

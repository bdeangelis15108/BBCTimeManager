using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.PayPeriod.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.PayPeriod.Exporting
{
    public class PayPeriodsExcelExporter : EpPlusExcelExporterBase, IPayPeriodsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PayPeriodsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPayPeriodsForViewDto> payPeriods)
        {
            return CreateExcelPackage(
                "PayPeriods.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PayPeriods"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("StartDate"),
                        L("EndDate"),
                        L("Name"),
                        L("IsActive")
                        );

                    AddObjects(
                        sheet, 2, payPeriods,
                        _ => _timeZoneConverter.Convert(_.PayPeriods.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.PayPeriods.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.PayPeriods.Name,
                        _ => _.PayPeriods.IsActive
                        );

					var startDateColumn = sheet.Column(1);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(2);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					
					
                });
        }
    }
}

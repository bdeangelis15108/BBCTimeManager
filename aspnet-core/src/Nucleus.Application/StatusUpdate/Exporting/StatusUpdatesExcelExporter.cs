using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.StatusUpdate.Exporting
{
    public class StatusUpdatesExcelExporter : EpPlusExcelExporterBase, IStatusUpdatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StatusUpdatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStatusUpdatesForViewDto> statusUpdates)
        {
            return CreateExcelPackage(
                "StatusUpdates.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("StatusUpdates"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ModifiedOn"),
                        L("Name"),
                        L("OriginalstatusId"),
                        L("ActualCreateDateTime"),
                        L("TimeshetIds"),
                        (L("Timesheets")) + L("Name"),
                        (L("Statuses")) + L("Name"),
                        (L("Jobs")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, statusUpdates,
                        _ => _timeZoneConverter.Convert(_.StatusUpdates.ModifiedOn, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.StatusUpdates.Name,
                        _ => _.StatusUpdates.OriginalstatusId,
                        _ => _timeZoneConverter.Convert(_.StatusUpdates.ActualCreateDateTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.StatusUpdates.TimeshetIds,
                        _ => _.TimesheetsName,
                        _ => _.StatusesName,
                        _ => _.JobsName,
                        _ => _.UserName
                        );

					var modifiedOnColumn = sheet.Column(1);
                    modifiedOnColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					modifiedOnColumn.AutoFit();
					var actualCreateDateTimeColumn = sheet.Column(4);
                    actualCreateDateTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					actualCreateDateTimeColumn.AutoFit();
					
					
                });
        }
    }
}

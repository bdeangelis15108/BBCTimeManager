using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.Timesheet.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.Timesheet.Exporting
{
    public class TimesheetsExcelExporter : EpPlusExcelExporterBase, ITimesheetsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TimesheetsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTimesheetsForViewDto> timesheets)
        {
            return CreateExcelPackage(
                "Timesheets.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Timesheets"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CreatedDate"),
                        L("SubmitedDate"),
                        L("Name"),
                        (L("Statuses")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, timesheets,
                        _ => _timeZoneConverter.Convert(_.Timesheets.CreatedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Timesheets.SubmitedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Timesheets.Name,
                        _ => _.StatusesName
                        );

					var createdDateColumn = sheet.Column(1);
                    createdDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createdDateColumn.AutoFit();
					var submitedDateColumn = sheet.Column(2);
                    submitedDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					submitedDateColumn.AutoFit();
					
					
                });
        }
    }
}

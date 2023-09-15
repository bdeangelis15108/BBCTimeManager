using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.Shift.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.Shift.Exporting
{
    public class ShiftsExcelExporter : EpPlusExcelExporterBase, IShiftsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShiftsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShiftsForViewDto> shifts)
        {
            return CreateExcelPackage(
                "Shifts.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Shifts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ScheduledStart"),
                        L("ScheduledEnd"),
                        L("Name"),
                        (L("Jobs")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, shifts,
                        _ => _timeZoneConverter.Convert(_.Shifts.ScheduledStart, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Shifts.ScheduledEnd, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Shifts.Name,
                        _ => _.JobsName
                        );

					var scheduledStartColumn = sheet.Column(1);
                    scheduledStartColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					scheduledStartColumn.AutoFit();
					var scheduledEndColumn = sheet.Column(2);
                    scheduledEndColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					scheduledEndColumn.AutoFit();
					
					
                });
        }
    }
}

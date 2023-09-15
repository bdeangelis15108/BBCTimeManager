using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.Job.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.Job.Exporting
{
    public class JobsesExcelExporter : EpPlusExcelExporterBase, IJobsesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobsesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobsForViewDto> jobses)
        {
            return CreateExcelPackage(
                "Jobses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Jobses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("StartDate"),
                        L("EndDate"),
                        L("Status"),
                        (L("Addresses")) + L("Linne1"),
                        (L("JobClasses")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, jobses,
                        _ => _.Jobs.Code,
                        _ => _.Jobs.Name,
                        _ => _timeZoneConverter.Convert(_.Jobs.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Jobs.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Jobs.Status,
                        _ => _.AddressesLinne1,
                        _ => _.JobClassesName
                        );

					var startDateColumn = sheet.Column(3);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(4);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					
					
                });
        }
    }
}

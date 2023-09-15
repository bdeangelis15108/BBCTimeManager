using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.JobClass.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.JobClass.Exporting
{
    public class JobClassesesExcelExporter : EpPlusExcelExporterBase, IJobClassesesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobClassesesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobClassesForViewDto> jobClasseses)
        {
            return CreateExcelPackage(
                "JobClasseses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("JobClasseses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, jobClasseses,
                        _ => _.JobClasses.Code,
                        _ => _.JobClasses.Name
                        );

					
					
                });
        }
    }
}

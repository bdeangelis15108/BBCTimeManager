using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.JobUnion.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.JobUnion.Exporting
{
    public class JobUnionsExcelExporter : EpPlusExcelExporterBase, IJobUnionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobUnionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobUnionsForViewDto> jobUnions)
        {
            return CreateExcelPackage(
                "JobUnions.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("JobUnions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Number"),
                        (L("Jobs")) + L("Name"),
                        (L("Unions")) + L("Number")
                        );

                    AddObjects(
                        sheet, 2, jobUnions,
                        _ => _.JobUnions.Number,
                        _ => _.JobsName,
                        _ => _.UnionsNumber
                        );

					
					
                });
        }
    }
}

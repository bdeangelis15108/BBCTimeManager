using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.JobPhaseCode.Exporting
{
    public class JobPhaseCodesExcelExporter : EpPlusExcelExporterBase, IJobPhaseCodesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobPhaseCodesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobPhaseCodesForViewDto> jobPhaseCodes)
        {
            return CreateExcelPackage(
                "JobPhaseCodes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("JobPhaseCodes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        (L("Jobs")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, jobPhaseCodes,
                        _ => _.JobPhaseCodes.Code,
                        _ => _.JobPhaseCodes.Name,
                        _ => _.JobsName
                        );

					
					
                });
        }
    }
}

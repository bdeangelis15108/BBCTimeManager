using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.JCJOBS.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.JCJOBS.Exporting
{
    public class JCJOBsExcelExporter : EpPlusExcelExporterBase, IJCJOBsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JCJOBsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJCJOBForViewDto> jcjoBs)
        {
            return CreateExcelPackage(
                "JCJOBs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("JCJOBs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("STATE"),
                        L("LOCALITY"),
                        L("CLASS"),
                        L("CLOSED"),
                        (L("JACCAT")) + L("JOBNUM")
                        );

                    AddObjects(
                        sheet, 2, jcjoBs,
                        _ => _.JCJOB.STATE,
                        _ => _.JCJOB.LOCALITY,
                        _ => _.JCJOB.CLASS,
                        _ => _.JCJOB.CLOSED,
                        _ => _.JACCATJOBNUM
                        );

					

                });
        }
    }
}

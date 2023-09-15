using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.JCUNIONS.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.JCUNIONS.Exporting
{
    public class JCUNIONsExcelExporter : EpPlusExcelExporterBase, IJCUNIONsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JCUNIONsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJCUNIONForViewDto> jcunioNs)
        {
            return CreateExcelPackage(
                "JCUNIONs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("JCUNIONs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UNIONNUM"),
                        L("UNIONLOCAL"),
                        (L("JACCAT")) + L("JOBNUM")
                        );

                    AddObjects(
                        sheet, 2, jcunioNs,
                        _ => _.JCUNION.UNIONNUM,
                        _ => _.JCUNION.UNIONLOCAL,
                        _ => _.JACCATJOBNUM
                        );

					

                });
        }
    }
}

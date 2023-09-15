using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.JCCAT.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.JCCAT.Exporting
{
    public class JACCATsExcelExporter : EpPlusExcelExporterBase, IJACCATsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JACCATsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJACCATForViewDto> jaccaTs)
        {
            return CreateExcelPackage(
                "JACCATs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("JACCATs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("SEQUENCE"),
                        L("JOBNUM"),
                        L("PHASENUM"),
                        L("CATNUM"),
                        L("NAME")
                        );

                    AddObjects(
                        sheet, 2, jaccaTs,
                        _ => _.JACCAT.SEQUENCE,
                        _ => _.JACCAT.JOBNUM,
                        _ => _.JACCAT.PHASENUM,
                        _ => _.JACCAT.CATNUM,
                        _ => _.JACCAT.NAME
                        );

					

                });
        }
    }
}

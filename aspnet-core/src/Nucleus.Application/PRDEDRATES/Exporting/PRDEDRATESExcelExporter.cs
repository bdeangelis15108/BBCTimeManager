using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.PRDEDRATES.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.PRDEDRATES.Exporting
{
    public class PRDEDRATESExcelExporter : EpPlusExcelExporterBase, IPRDEDRATESExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PRDEDRATESExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPRDEDRATEForViewDto> prdedrates)
        {
            return CreateExcelPackage(
                "PRDEDRATES.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PRDEDRATES"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UNIONLOCAL"),
                        L("CLASS"),
                        L("DEDTYPE"),
                        L("PERHR"),
                        (L("PRCLASS")) + L("UNIONNUM")
                        );

                    AddObjects(
                        sheet, 2, prdedrates,
                        _ => _.PRDEDRATE.UNIONLOCAL,
                        _ => _.PRDEDRATE.CLASS,
                        _ => _.PRDEDRATE.DEDTYPE,
                        _ => _.PRDEDRATE.PERHR,
                        _ => _.PRCLASSUNIONNUM
                        );

					

                });
        }
    }
}

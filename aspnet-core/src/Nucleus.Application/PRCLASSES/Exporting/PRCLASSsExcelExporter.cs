using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.PRCLASSES.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.PRCLASSES.Exporting
{
    public class PRCLASSsExcelExporter : EpPlusExcelExporterBase, IPRCLASSsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PRCLASSsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPRCLASSForViewDto> prclasSs)
        {
            return CreateExcelPackage(
                "PRCLASSs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PRCLASSs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("NAME"),
                        (L("JCUNION")) + L("UNIONNUM"),
                        (L("PREMPLOYEE")) + L("CLASS")
                        );

                    AddObjects(
                        sheet, 2, prclasSs,
                        _ => _.PRCLASS.NAME,
                        _ => _.JCUNIONUNIONNUM,
                        _ => _.PREMPLOYEECLASS
                        );

					

                });
        }
    }
}

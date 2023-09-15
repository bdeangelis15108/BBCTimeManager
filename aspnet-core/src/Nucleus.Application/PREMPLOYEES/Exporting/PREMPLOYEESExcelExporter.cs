using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.PREMPLOYEES.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.PREMPLOYEES.Exporting
{
    public class PREMPLOYEESExcelExporter : EpPlusExcelExporterBase, IPREMPLOYEESExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PREMPLOYEESExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPREMPLOYEEForViewDto> premployees)
        {
            return CreateExcelPackage(
                "PREMPLOYEES.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PREMPLOYEES"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EMPNUM"),
                        L("NAME"),
                        L("UNIONNUM"),
                        L("UNIONLOCAL"),
                        L("CLASS"),
                        L("WCOMPNUM1"),
                        L("LASTNAME"),
                        L("FIRSTNAME"),
                        L("STATUS"),
                        L("PAYRATE")
                        );

                    AddObjects(
                        sheet, 2, premployees,
                        _ => _.PREMPLOYEE.EMPNUM,
                        _ => _.PREMPLOYEE.NAME,
                        _ => _.PREMPLOYEE.UNIONNUM,
                        _ => _.PREMPLOYEE.UNIONLOCAL,
                        _ => _.PREMPLOYEE.CLASS,
                        _ => _.PREMPLOYEE.WCOMPNUM1,
                        _ => _.PREMPLOYEE.LASTNAME,
                        _ => _.PREMPLOYEE.FIRSTNAME,
                        _ => _.PREMPLOYEE.STATUS,
                        _ => _.PREMPLOYEE.PAYRATE
                        );

					

                });
        }
    }
}

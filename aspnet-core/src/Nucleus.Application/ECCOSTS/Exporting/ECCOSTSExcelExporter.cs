using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.ECCOSTS.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.ECCOSTS.Exporting
{
    public class ECCOSTSExcelExporter : EpPlusExcelExporterBase, IECCOSTSExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ECCOSTSExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetECCOSTForViewDto> eccosts)
        {
            return CreateExcelPackage(
                "ECCOSTS.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ECCOSTS"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CODENUM"),
                        L("ESTHOURLY"),
                        (L("EQUIPMENT")) + L("EQUIPNUM")
                        );

                    AddObjects(
                        sheet, 2, eccosts,
                        _ => _.ECCOST.CODENUM,
                        _ => _.ECCOST.ESTHOURLY,
                        _ => _.EQUIPMENTEQUIPNUM
                        );

					
					
                });
        }
    }
}

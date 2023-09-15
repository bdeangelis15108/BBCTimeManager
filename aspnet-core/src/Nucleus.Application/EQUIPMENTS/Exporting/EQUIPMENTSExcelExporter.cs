using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.EQUIPMENTS.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.EQUIPMENTS.Exporting
{
    public class EQUIPMENTSExcelExporter : EpPlusExcelExporterBase, IEQUIPMENTSExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EQUIPMENTSExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEQUIPMENTForViewDto> equipments)
        {
            return CreateExcelPackage(
                "EQUIPMENTS.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EQUIPMENTS"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EQUIPNUM")
                        );

                    AddObjects(
                        sheet, 2, equipments,
                        _ => _.EQUIPMENT.EQUIPNUM
                        );

					
					
                });
        }
    }
}

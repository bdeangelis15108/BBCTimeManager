using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.CostType.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.CostType.Exporting
{
    public class CostTypeseExcelExporter : EpPlusExcelExporterBase, ICostTypeseExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CostTypeseExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCostTypesForViewDto> costTypese)
        {
            return CreateExcelPackage(
                "CostTypese.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CostTypese"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Code")
                        );

                    AddObjects(
                        sheet, 2, costTypese,
                        _ => _.CostTypes.Name,
                        _ => _.CostTypes.Code
                        );

					
					
                });
        }
    }
}

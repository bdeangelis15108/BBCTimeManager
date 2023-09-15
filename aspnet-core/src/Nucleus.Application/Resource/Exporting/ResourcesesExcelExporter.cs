using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.Resource.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.Resource.Exporting
{
    public class ResourcesesExcelExporter : EpPlusExcelExporterBase, IResourcesesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ResourcesesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetResourcesForViewDto> resourceses)
        {
            return CreateExcelPackage(
                "Resourceses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Resourceses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Type"),
                        L("CostPerHour"),
                        L("CostPerUser"),
                        L("CostPerDay"),
                        L("ResourceNumber")
                        );

                    AddObjects(
                        sheet, 2, resourceses,
                        _ => _.Resources.Name,
                        _ => _.Resources.Type,
                        _ => _.Resources.CostPerHour,
                        _ => _.Resources.CostPerUser,
                        _ => _.Resources.CostPerDay,
                        _ => _.Resources.ResourceNumber
                        );

					
					
                });
        }
    }
}

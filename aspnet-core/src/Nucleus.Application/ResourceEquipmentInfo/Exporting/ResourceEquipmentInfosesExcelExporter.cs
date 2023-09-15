using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.ResourceEquipmentInfo.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.ResourceEquipmentInfo.Exporting
{
    public class ResourceEquipmentInfosesExcelExporter : EpPlusExcelExporterBase, IResourceEquipmentInfosesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ResourceEquipmentInfosesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetResourceEquipmentInfosForViewDto> resourceEquipmentInfoses)
        {
            return CreateExcelPackage(
                "ResourceEquipmentInfoses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ResourceEquipmentInfoses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, resourceEquipmentInfoses,
                        _ => _.ResourceEquipmentInfos.Name
                        );

					
					
                });
        }
    }
}

using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.ResourceWorkerInfo.Exporting
{
    public class ResourceWorkerInfosesExcelExporter : EpPlusExcelExporterBase, IResourceWorkerInfosesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ResourceWorkerInfosesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetResourceWorkerInfosForViewDto> resourceWorkerInfoses)
        {
            return CreateExcelPackage(
                "ResourceWorkerInfoses.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ResourceWorkerInfoses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FirstName"),
                        L("LastName"),
                        L("UnionNumber"),
                        L("UnionLocal"),
                        L("Wcomp1"),
                        (L("WorkerClasees")) + L("Name"),
                        (L("Resources")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, resourceWorkerInfoses,
                        _ => _.ResourceWorkerInfos.FirstName,
                        _ => _.ResourceWorkerInfos.LastName,
                        _ => _.ResourceWorkerInfos.UnionNumber,
                        _ => _.ResourceWorkerInfos.UnionLocal,
                        _ => _.ResourceWorkerInfos.Wcomp1,
                        _ => _.WorkerClaseesName,
                        _ => _.ResourcesName
                        );

					

                });
        }
    }
}

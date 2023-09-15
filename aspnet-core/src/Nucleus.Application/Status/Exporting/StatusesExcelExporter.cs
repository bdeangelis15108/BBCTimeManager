using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.Status.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.Status.Exporting
{
    public class StatusesExcelExporter : EpPlusExcelExporterBase, IStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStatusesForViewDto> statuses)
        {
            return CreateExcelPackage(
                "Statuses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Statuses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("IsDefault"),
                        L("ForwardName"),
                        L("ReverseName"),
                        L("ForwardId"),
                        L("ReverseId")
                        );

                    AddObjects(
                        sheet, 2, statuses,
                        _ => _.Statuses.Name,
                        _ => _.Statuses.IsDefault,
                        _ => _.Statuses.ForwardName,
                        _ => _.Statuses.ReverseName,
                        _ => _.Statuses.ForwardId,
                        _ => _.Statuses.ReverseId
                        );

					
					
                });
        }
    }
}

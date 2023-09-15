using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.ResourceReservation.Exporting
{
    public class ResourceReservationsesExcelExporter : EpPlusExcelExporterBase, IResourceReservationsesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ResourceReservationsesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetResourceReservationsForViewDto> resourceReservationses)
        {
            return CreateExcelPackage(
                "ResourceReservationses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ResourceReservationses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ReservedFrom"),
                        L("ReservedUntil"),
                        (L("User")) + L("Name"),
                        (L("Resources")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, resourceReservationses,
                        _ => _timeZoneConverter.Convert(_.ResourceReservations.ReservedFrom, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.ResourceReservations.ReservedUntil, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.ResourcesName
                        );

					var reservedFromColumn = sheet.Column(1);
                    reservedFromColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					reservedFromColumn.AutoFit();
					var reservedUntilColumn = sheet.Column(2);
                    reservedUntilColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					reservedUntilColumn.AutoFit();
					
					
                });
        }
    }
}

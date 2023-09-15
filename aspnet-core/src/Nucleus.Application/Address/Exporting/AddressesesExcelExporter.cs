using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.Address.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.Address.Exporting
{
    public class AddressesesExcelExporter : EpPlusExcelExporterBase, IAddressesesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AddressesesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAddressesForViewDto> addresseses)
        {
            return CreateExcelPackage(
                "Addresseses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Addresseses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Linne1"),
                        L("Line2"),
                        L("City"),
                        L("State"),
                        L("Zip"),
                        L("Lan"),
                        L("Lat")
                        );

                    AddObjects(
                        sheet, 2, addresseses,
                        _ => _.Addresses.Linne1,
                        _ => _.Addresses.Line2,
                        _ => _.Addresses.City,
                        _ => _.Addresses.State,
                        _ => _.Addresses.Zip,
                        _ => _.Addresses.Lan,
                        _ => _.Addresses.Lat
                        );

					
					
                });
        }
    }
}

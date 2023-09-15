using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.Union.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.Union.Exporting
{
    public class UnionsExcelExporter : EpPlusExcelExporterBase, IUnionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UnionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUnionsForViewDto> unions)
        {
            return CreateExcelPackage(
                "Unions.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Unions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Number"),
                        L("LocalNumber")
                        );

                    AddObjects(
                        sheet, 2, unions,
                        _ => _.Unions.Number,
                        _ => _.Unions.LocalNumber
                        );

					
					
                });
        }
    }
}

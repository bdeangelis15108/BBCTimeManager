using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.UnionPayRate.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.UnionPayRate.Exporting
{
    public class UnionPayRatesExcelExporter : EpPlusExcelExporterBase, IUnionPayRatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UnionPayRatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUnionPayRatesForViewDto> unionPayRates)
        {
            return CreateExcelPackage(
                "UnionPayRates.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("UnionPayRates"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Class"),
                        L("Dedtype"),
                        L("Perhour"),
                        (L("Unions")) + L("Number")
                        );

                    AddObjects(
                        sheet, 2, unionPayRates,
                        _ => _.UnionPayRates.Class,
                        _ => _.UnionPayRates.Dedtype,
                        _ => _.UnionPayRates.Perhour,
                        _ => _.UnionsNumber
                        );

					
					
                });
        }
    }
}

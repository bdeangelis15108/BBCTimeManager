using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.WorkerClasee.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.WorkerClasee.Exporting
{
    public class WorkerClaseesesExcelExporter : EpPlusExcelExporterBase, IWorkerClaseesesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WorkerClaseesesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWorkerClaseesForViewDto> workerClaseeses)
        {
            return CreateExcelPackage(
                "WorkerClaseeses.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("WorkerClaseeses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, workerClaseeses,
                        _ => _.WorkerClasees.Code,
                        _ => _.WorkerClasees.Name
                        );

					
					
                });
        }
    }
}

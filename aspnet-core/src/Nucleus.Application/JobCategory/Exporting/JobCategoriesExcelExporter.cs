using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.JobCategory.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;

namespace Nucleus.JobCategory.Exporting
{
    public class JobCategoriesExcelExporter : EpPlusExcelExporterBase, IJobCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobCategoriesForViewDto> jobCategories)
        {
            return CreateExcelPackage(
                "JobCategories.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("JobCategories"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name")
                        );

                    AddObjects(
                        sheet, 2, jobCategories,
                        _ => _.JobCategories.Code,
                        _ => _.JobCategories.Name
                        );

					
					
                });
        }
    }
}

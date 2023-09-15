using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.EquipTimetable.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;
using System.Linq;

namespace Nucleus.EquipTimetable.Exporting
{
    public class EquipTimetablesExcelExporter : EpPlusExcelExporterBase, IEquipTimetablesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EquipTimetablesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }
      

        public FileDto ExportToFile(List<GetEquipTimetablesForViewDto> equipTimetables, string[] days)
        {
            var equipmentExportData = EquipmentTimetableExportDto.FromEquipmentExportResultSet(equipTimetables, days);
            return CreateExcelPackage(
                "EquipTimetables.csv",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EquipTimetables"));
                    sheet.OutLineApplyStyle = true;


                    AddHeader(
                        sheet,
                        nameof(EquipmentTimetableExportDto.Equip).ToLower(),
                        nameof(EquipmentTimetableExportDto.Code).ToLower(),
                        nameof(EquipmentTimetableExportDto.Date).ToLower(),
                        nameof(EquipmentTimetableExportDto.Job).ToLower(),
                        nameof(EquipmentTimetableExportDto.Phase).ToLower(),
                        nameof(EquipmentTimetableExportDto.Cat).ToLower(),
                        nameof(EquipmentTimetableExportDto.CostHours).ToLower(),
                        nameof(EquipmentTimetableExportDto.CostRate).ToLower(),
                        nameof(EquipmentTimetableExportDto.Cost).ToLower()
                        );
                    // todo: insert relevant values
                    AddObjects(
                        sheet, 2, equipmentExportData,
                        _ => _.Equip,
                        _ => _.Code,
                        _ => _.Date,
                        _ => _.Job,
                        _ => _.Phase,
                        _ => _.Cat,
                        _ => _.CostHours,
                        _ => _.CostRate,
                        _ => _.Cost
                        );

                    var createdOnColumn = sheet.Column(3);
                    createdOnColumn.Style.Numberformat.Format = "MM/dd/yyyy";
                    createdOnColumn.AutoFit();

                });
        }
    }
}
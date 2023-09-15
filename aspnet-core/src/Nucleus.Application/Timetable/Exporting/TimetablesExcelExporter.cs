using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Nucleus.DataExporting.Excel.EpPlus;
using Nucleus.Timetable.Dtos;
using Nucleus.Dto;
using Nucleus.Storage;
using OfficeOpenXml.DataValidation;

namespace Nucleus.Timetable.Exporting
{
    public class TimetablesExcelExporter : EpPlusExcelExporterBase, ITimetablesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TimetablesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTimetablesForViewDto> timetables, string[] days)
        {
            // todo: Adjust it accordingly
            return CreateExcelPackage(
                $"Timetables {days[0]}-{days[6]}.csv",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Timetables"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "Employee",
                        "Class",
                        "Pay Type",
                        "",
                        "Cost Code",
                        days[0],
                        days[1],
                        days[2],
                        days[3],
                        days[4],
                        days[5],
                        days[6],
                        "Rate",
                        "Amount",
                        "UnionLocal",
                        "State",
                        "Description",
                        "Cost Type",
                        "Account",
                        "Wcomp1"
                        );

                    AddObjects(
                        sheet, 2, timetables,
                        _ => _.ResourcesName,
                        _ => _.WorkerClaseesName,
                        _ => _.PayTypesCode,
                        _ => (_.Timetables.Multiplier == null || !_.Timetables.Multiplier.HasValue ? " x 0.0" : " x " + _.Timetables.Multiplier.ToString()),
                        _ => _.Timetables.CostCode,
                        _ => _.Timetables.Day1 ?? 0.0M,
                        _ => _.Timetables.Day2 ?? 0.0M,
                        _ => _.Timetables.Day3 ?? 0.0M,
                        _ => _.Timetables.Day4 ?? 0.0M,
                        _ => _.Timetables.Day5 ?? 0.0M,
                        _ => _.Timetables.Day6 ?? 0.0M,
                        _ => _.Timetables.Day7 ?? 0.0M,
                        _ => _.UnionPayRatesPerHour,
                        _ => _.Timetables.Amount,
                        _ => _.UnionsLocalNumber,
                        _ => _.AddressesState,
                        _ => _.ExpenseTypesDescription,
                        _ => _.CostTypesName,
                        _ => _.AccountsName,
                        _ => _.Timetables.Wcomp1);
                    //var col = sheet.Column(0);
                    //var workerClassesList = sheet.Cells[1, 1].DataValidation.AddListDataValidation() as ExcelDataValidationList;
                    //workerClassesList.Formula.Values.Add("somevalue");
                });
        }
    }
}
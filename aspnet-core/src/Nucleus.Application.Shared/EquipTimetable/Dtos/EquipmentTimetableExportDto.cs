using System;
using System.Collections.Generic;
using System.Text;

namespace Nucleus.EquipTimetable.Dtos
{
    public class EquipmentTimetableExportDto
    {
        public string Equip { get; set; }
        public string Code { get; set; } = "JC";
        public string Date { get; set; }
        public string Job { get; set; }
        public string Phase { get; set; }
        public string Cat { get; set; }
        public decimal? CostHours { get; set; }

        public decimal? CostRate { get; set; }
        public decimal? Cost { get; set; }

        public static List<EquipmentTimetableExportDto> FromEquipmentExportResultSet(List<GetEquipTimetablesForViewDto> equipTimetables, string[] days)
        {
            var list = new List<EquipmentTimetableExportDto>();

            foreach (var item in equipTimetables)
            {
                if (item.EquipTimetables.Day1.HasValue && item.EquipTimetables.Day1.Value > 0)
                {
                    list.Add(new EquipmentTimetableExportDto
                    {
                        Equip = item.ResourcesResourceName,
                        Job = item.JobCode,
                        Phase = item.PhaseCode,
                        Cat = item.CategoryCode,
                        CostRate = item.CostPerHour,
                        Date = days[0],
                        CostHours = item.EquipTimetables.Day1.HasValue ? item.EquipTimetables.Day1 : 0,
                        Cost = item.EquipTimetables.Day1.HasValue ? item.EquipTimetables.Day1 * item.CostPerHour : 0,
                    }); // Day1
                }

                if (item.EquipTimetables.Day2.HasValue && item.EquipTimetables.Day2.Value > 0)
                {
                    list.Add(new EquipmentTimetableExportDto
                    {
                        Equip = item.ResourcesResourceName,
                        Job = item.JobCode,
                        Phase = item.PhaseCode,
                        Cat = item.CategoryCode,
                        CostRate = item.CostPerHour,
                        Date = days[1],
                        CostHours = item.EquipTimetables.Day2.HasValue ? item.EquipTimetables.Day2 : 0,
                        Cost = item.EquipTimetables.Day2.HasValue ? item.EquipTimetables.Day2 * item.CostPerHour : 0,
                    }); // Day2
                }
                if (item.EquipTimetables.Day3.HasValue && item.EquipTimetables.Day3.Value > 0)
                {
                    list.Add(new EquipmentTimetableExportDto
                    {
                        Equip = item.ResourcesResourceName,
                        Job = item.JobCode,
                        Phase = item.PhaseCode,
                        Cat = item.CategoryCode,
                        CostRate = item.CostPerHour,
                        Date = days[2],
                        CostHours = item.EquipTimetables.Day3.HasValue ? item.EquipTimetables.Day3 : 0,
                        Cost = item.EquipTimetables.Day3.HasValue ? item.EquipTimetables.Day3 * item.CostPerHour : 0,
                    }); // Day3
                }

                if (item.EquipTimetables.Day4.HasValue && item.EquipTimetables.Day4.Value > 0)
                {
                    list.Add(new EquipmentTimetableExportDto
                    {
                        Equip = item.ResourcesResourceName,
                        Job = item.JobCode,
                        Phase = item.PhaseCode,
                        Cat = item.CategoryCode,
                        CostRate = item.CostPerHour,
                        Date = days[3],
                        CostHours = item.EquipTimetables.Day4.HasValue ? item.EquipTimetables.Day4 : 0,
                        Cost = item.EquipTimetables.Day4.HasValue ? item.EquipTimetables.Day4 * item.CostPerHour : 0,
                    }); // Day4
                }

                if (item.EquipTimetables.Day5.HasValue && item.EquipTimetables.Day5.Value > 0)
                {
                    list.Add(new EquipmentTimetableExportDto
                    {
                        Equip = item.ResourcesResourceName,
                        Job = item.JobCode,
                        Phase = item.PhaseCode,
                        Cat = item.CategoryCode,
                        CostRate = item.CostPerHour,
                        Date = days[4],
                        CostHours = item.EquipTimetables.Day5.HasValue ? item.EquipTimetables.Day5 : 0,
                        Cost = item.EquipTimetables.Day5.HasValue ? item.EquipTimetables.Day5 * item.CostPerHour : 0,
                    }); // Day5

                }


                if (item.EquipTimetables.Day6.HasValue && item.EquipTimetables.Day6.Value > 0)
                {
                    list.Add(new EquipmentTimetableExportDto
                    {
                        Equip = item.ResourcesResourceName,
                        Job = item.JobCode,
                        Phase = item.PhaseCode,
                        Cat = item.CategoryCode,
                        CostRate = item.CostPerHour,
                        Date = days[5],
                        CostHours = item.EquipTimetables.Day6.HasValue ? item.EquipTimetables.Day6 : 0,
                        Cost = item.EquipTimetables.Day6.HasValue ? item.EquipTimetables.Day6 * item.CostPerHour : 0,
                    }); // Day6
                }


                if (item.EquipTimetables.Day7.HasValue && item.EquipTimetables.Day7.Value > 0)
                {
                    list.Add(new EquipmentTimetableExportDto
                    {
                        Equip = item.ResourcesResourceName,
                        Job = item.JobCode,
                        Phase = item.PhaseCode,
                        Cat = item.CategoryCode,
                        CostRate = item.CostPerHour,
                        Date = days[6],
                        CostHours = item.EquipTimetables.Day7.HasValue ? item.EquipTimetables.Day7 : 0,
                        Cost = item.EquipTimetables.Day7.HasValue ? item.EquipTimetables.Day7 * item.CostPerHour : 0,
                    }); // Day7
                }





               
            }

            return list;
        }
    }
}

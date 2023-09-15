using System;
using Abp.Application.Services.Dto;

namespace Nucleus.EquipTimetable.Dtos
{
    public class EquipTimetablesDto : EntityDto
    {
        public decimal? Day1 { get; set; }

        public decimal? Day2 { get; set; }

        public decimal? Day3 { get; set; }

        public decimal? Day4 { get; set; }

        public decimal? Day5 { get; set; }

        public decimal? Day6 { get; set; }

        public decimal? Day7 { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool IsActive { get; set; }

        public int? PeriodDate { get; set; }

        public int? ResourcesCode { get; set; }

        public int? PhaseCode { get; set; }

        public int? CategoryCode { get; set; }

        public int? JobCode { get; set; }

    }
}
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Timetable.Dtos
{
    public class CreateOrEditTimetablesDto : EntityDto<int?>
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

        [Required]
        [StringLength(TimetablesConsts.MaxCostCodeLength, MinimumLength = TimetablesConsts.MinCostCodeLength)]
        public string CostCode { get; set; }

        public int? PeriodDate { get; set; }

        public int? ResourcesCode { get; set; }

        public int? Rate { get; set; }

        public int? Unionlocal { get; set; }

        public int? State { get; set; }

        public int? Description { get; set; }

        public int? CostTypesId { get; set; }

        public int? AccountsId { get; set; }

        public long? CreatedBy { get; set; }

        public int? PayTypesId { get; set; }

        public int? WorkerClaseesId { get; set; }

        public double? Multiplier { get; set; }

    }
}
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.EquipTimetable.Dtos
{
    public class CreateOrEditEquipTimetablesDto : EntityDto<int?>
    {

        [Range(EquipTimetablesConsts.MinDay1Value, EquipTimetablesConsts.MaxDay1Value)]
        public decimal? Day1 { get; set; }

        [Range(EquipTimetablesConsts.MinDay2Value, EquipTimetablesConsts.MaxDay2Value)]
        public decimal? Day2 { get; set; }

        [Range(EquipTimetablesConsts.MinDay3Value, EquipTimetablesConsts.MaxDay3Value)]
        public decimal? Day3 { get; set; }

        [Range(EquipTimetablesConsts.MinDay4Value, EquipTimetablesConsts.MaxDay4Value)]
        public decimal? Day4 { get; set; }

        [Range(EquipTimetablesConsts.MinDay5Value, EquipTimetablesConsts.MaxDay5Value)]
        public decimal? Day5 { get; set; }

        [Range(EquipTimetablesConsts.MinDay6Value, EquipTimetablesConsts.MaxDay6Value)]
        public decimal? Day6 { get; set; }

        [Range(EquipTimetablesConsts.MinDay7Value, EquipTimetablesConsts.MaxDay7Value)]
        public decimal? Day7 { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool IsActive { get; set; }

        public int? PeriodDate { get; set; }

        public int? ResourcesCode { get; set; }

        public int? PhaseCode { get; set; }

        public int? CategoryCode { get; set; }

        public int? JobCode { get; set; }
        public string JobCodeString { get; set; }

    }
}
using Nucleus.PayPeriod;
using Nucleus.Resource;
using Nucleus.JobPhaseCode;
using Nucleus.JobCategory;
using Nucleus.Job;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Nucleus.EquipTimetable
{
    [Table("EquipTimetables")]
    public class EquipTimetables : Entity
    {

        [Range(EquipTimetablesConsts.MinDay1Value, EquipTimetablesConsts.MaxDay1Value)]
        public virtual decimal? Day1 { get; set; }

        [Range(EquipTimetablesConsts.MinDay2Value, EquipTimetablesConsts.MaxDay2Value)]
        public virtual decimal? Day2 { get; set; }

        [Range(EquipTimetablesConsts.MinDay3Value, EquipTimetablesConsts.MaxDay3Value)]
        public virtual decimal? Day3 { get; set; }

        [Range(EquipTimetablesConsts.MinDay4Value, EquipTimetablesConsts.MaxDay4Value)]
        public virtual decimal? Day4 { get; set; }

        [Range(EquipTimetablesConsts.MinDay5Value, EquipTimetablesConsts.MaxDay5Value)]
        public virtual decimal? Day5 { get; set; }

        [Range(EquipTimetablesConsts.MinDay6Value, EquipTimetablesConsts.MaxDay6Value)]
        public virtual decimal? Day6 { get; set; }

        [Range(EquipTimetablesConsts.MinDay7Value, EquipTimetablesConsts.MaxDay7Value)]
        public virtual decimal? Day7 { get; set; }

        [Range(EquipTimetablesConsts.MinAmountValue, EquipTimetablesConsts.MaxAmountValue)]
        public virtual decimal? Amount { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int? PeriodDate { get; set; }

        [ForeignKey("PeriodDate")]
        public PayPeriods PeriodDateFk { get; set; }

        public virtual int? ResourcesCode { get; set; }

        [ForeignKey("ResourcesCode")]
        public Resources ResourcesCodeFk { get; set; }

        public virtual int? PhaseCode { get; set; }

        [ForeignKey("PhaseCode")]
        public JobPhaseCodes PhaseCodeFk { get; set; }

        public virtual int? CategoryCode { get; set; }

        [ForeignKey("CategoryCode")]
        public JobCategories CategoryCodeFk { get; set; }

        public virtual int? JobCode { get; set; }

        [ForeignKey("JobCode")]
        public Jobs JobCodeFk { get; set; }

    }
}
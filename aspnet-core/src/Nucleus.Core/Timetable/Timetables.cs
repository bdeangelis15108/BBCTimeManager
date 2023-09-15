using Nucleus.PayPeriod;
using Nucleus.Resource;
using Nucleus.UnionPayRate;
using Nucleus.Union;
using Nucleus.Address;
using Nucleus.ExpenseType;
using Nucleus.CostType;
using Nucleus.Account;
using Nucleus.Authorization.Users;
using Nucleus.PayType;
using Nucleus.WorkerClasee;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Timetable
{
    [Table("Timetables")]
    [Audited]
    public class Timetables : Entity
    {

        public virtual decimal? Day1 { get; set; }

        public virtual decimal? Day2 { get; set; }

        public virtual decimal? Day3 { get; set; }

        public virtual decimal? Day4 { get; set; }

        public virtual decimal? Day5 { get; set; }

        public virtual decimal? Day6 { get; set; }

        public virtual decimal? Day7 { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual DateTime? CreatedOn { get; set; }

        public virtual bool IsActive { get; set; }

        [Required]
        [StringLength(TimetablesConsts.MaxCostCodeLength, MinimumLength = TimetablesConsts.MinCostCodeLength)]
        public virtual string CostCode { get; set; }

        public virtual int? PeriodDate { get; set; }

        [ForeignKey("PeriodDate")]
        public PayPeriods PeriodDateFk { get; set; }

        public virtual int? ResourcesCode { get; set; }

        [ForeignKey("ResourcesCode")]
        public Resources ResourcesCodeFk { get; set; }

        public virtual int? Rate { get; set; }

        [ForeignKey("Rate")]
        public UnionPayRates RateFk { get; set; }

        public virtual int? Unionlocal { get; set; }

        [ForeignKey("Unionlocal")]
        public Unions UnionlocalFk { get; set; }

        public virtual int? State { get; set; }

        [ForeignKey("State")]
        public Addresses StateFk { get; set; }

        public virtual int? Description { get; set; }

        [ForeignKey("Description")]
        public ExpenseTypes DescriptionFk { get; set; }

        public virtual int? CostTypesId { get; set; }

        [ForeignKey("CostTypesId")]
        public CostTypes CostTypesFk { get; set; }

        public virtual int? AccountsId { get; set; }

        [ForeignKey("AccountsId")]
        public Accounts AccountsFk { get; set; }

        public virtual long? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public User CreatedByFk { get; set; }

        public virtual int? PayTypesId { get; set; }

        [ForeignKey("PayTypesId")]
        public PayTypes PayTypesFk { get; set; }

        public virtual int? WorkerClaseesId { get; set; }

        [ForeignKey("WorkerClaseesId")]
        public WorkerClasees WorkerClaseesFk { get; set; }

        public double? Multiplier { get; set; }

        [MaxLength(250)]
        public string Wcomp1 { get; set; }


    }
}
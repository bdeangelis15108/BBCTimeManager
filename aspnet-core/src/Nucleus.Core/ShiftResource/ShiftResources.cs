using Nucleus.Resource;
using Nucleus.PayType;
using Nucleus.JobPhaseCode;
using Nucleus.JobCategory;
using Nucleus.Timesheet;
using Nucleus.Shift;
using Nucleus.WorkerClasee;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.ShiftResource
{
	[Table("ShiftResources")]
    [Audited]
    public class ShiftResources : Entity 
    {

		public virtual decimal? HoursWorked { get; set; }
		
		[StringLength(ShiftResourcesConsts.MaxNameLength, MinimumLength = ShiftResourcesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		

		public virtual int ResourcesId { get; set; }
		
        [ForeignKey("ResourcesId")]
		public Resources ResourcesFk { get; set; }
		
		public virtual int? PayTypesId { get; set; }
		
        [ForeignKey("PayTypesId")]
		public PayTypes PayTypesFk { get; set; }
		
		public virtual int? JobPhaseCodesId { get; set; }
		
        [ForeignKey("JobPhaseCodesId")]
		public JobPhaseCodes JobPhaseCodesFk { get; set; }
		
		public virtual int? JobCategoriesId { get; set; }
		
        [ForeignKey("JobCategoriesId")]
		public JobCategories JobCategoriesFk { get; set; }
		
		public virtual int? TimesheetsId { get; set; }
		
        [ForeignKey("TimesheetsId")]
		public Timesheets TimesheetsFk { get; set; }
		
		public virtual int? ShiftsId { get; set; }
		
        [ForeignKey("ShiftsId")]
		public Shifts ShiftsFk { get; set; }
		
		public virtual int? WorkerClaseesId { get; set; }
		
        [ForeignKey("WorkerClaseesId")]
		public WorkerClasees WorkerClaseesFk { get; set; }
		
    }
}
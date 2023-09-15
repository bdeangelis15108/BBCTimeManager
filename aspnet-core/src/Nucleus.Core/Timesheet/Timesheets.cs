using Nucleus.Status;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Timesheet
{
	[Table("Timesheets")]
    [Audited]
    public class Timesheets : Entity 
    {

		public virtual DateTime? CreatedDate { get; set; }
		
		public virtual DateTime? SubmitedDate { get; set; }
		
		[StringLength(TimesheetsConsts.MaxNameLength, MinimumLength = TimesheetsConsts.MinNameLength)]
		public virtual string Name { get; set; }
		

		public virtual int? StatusesId { get; set; }
		
        [ForeignKey("StatusesId")]
		public Statuses StatusesFk { get; set; }
		
    }
}
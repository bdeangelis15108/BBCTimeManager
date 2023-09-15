using Nucleus.Timesheet;
using Nucleus.Status;
using Nucleus.Job;
using Nucleus.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Nucleus.StatusUpdate
{
	[Table("StatusUpdates")]
    public class StatusUpdates : Entity 
    {

		public virtual DateTime? ModifiedOn { get; set; }
		
		[StringLength(StatusUpdatesConsts.MaxNameLength, MinimumLength = StatusUpdatesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		public virtual int? OriginalstatusId { get; set; }
		
		public virtual DateTime? ActualCreateDateTime { get; set; }
		
		[StringLength(StatusUpdatesConsts.MaxTimeshetIdsLength, MinimumLength = StatusUpdatesConsts.MinTimeshetIdsLength)]
		public virtual string TimeshetIds { get; set; }
		

		public virtual int? TimesheetsId { get; set; }
		
        [ForeignKey("TimesheetsId")]
		public Timesheets TimesheetsFk { get; set; }
		
		public virtual int? NewStatusesId { get; set; }
		
        [ForeignKey("NewStatusesId")]
		public Statuses NewStatusesFk { get; set; }
		
		public virtual int? JobsId { get; set; }
		
        [ForeignKey("JobsId")]
		public Jobs JobsFk { get; set; }
		
		public virtual long? ModifiedBy { get; set; }
		
        [ForeignKey("ModifiedBy")]
		public User ModifiedByFk { get; set; }
		
    }
}
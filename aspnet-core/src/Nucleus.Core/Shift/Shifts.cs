using Nucleus.Job;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Shift
{
	[Table("Shifts")]
    [Audited]
    public class Shifts : Entity 
    {

		public virtual DateTime? ScheduledStart { get; set; }
		
		public virtual DateTime? ScheduledEnd { get; set; }
		
		public virtual string Name { get; set; }
		

		public virtual int JobsId { get; set; }
		
        [ForeignKey("JobsId")]
		public Jobs JobsFk { get; set; }
		
    }
}
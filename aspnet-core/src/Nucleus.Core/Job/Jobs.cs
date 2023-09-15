using Nucleus.Address;
using Nucleus.JobClass;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Job
{
	[Table("Jobses")]
    [Audited]
    public class Jobs : Entity 
    {

		[StringLength(JobsConsts.MaxCodeLength, MinimumLength = JobsConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[StringLength(JobsConsts.MaxNameLength, MinimumLength = JobsConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		public virtual DateTime? StartDate { get; set; }
		
		public virtual DateTime? EndDate { get; set; }
		
		public virtual int Status { get; set; }
		

		public virtual int? AddressesId { get; set; }
		
        [ForeignKey("AddressesId")]
		public Addresses AddressesFk { get; set; }
		
		public virtual int? JobClassesId { get; set; }
		
        [ForeignKey("JobClassesId")]
		public JobClasses JobClassesFk { get; set; }
		
    }
}
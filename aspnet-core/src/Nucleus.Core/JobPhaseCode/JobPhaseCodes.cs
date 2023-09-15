using Nucleus.Job;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.JobPhaseCode
{
	[Table("JobPhaseCodes")]
    [Audited]
    public class JobPhaseCodes : Entity 
    {

		[StringLength(JobPhaseCodesConsts.MaxCodeLength, MinimumLength = JobPhaseCodesConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[StringLength(JobPhaseCodesConsts.MaxNameLength, MinimumLength = JobPhaseCodesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		

		public virtual int? JobsId { get; set; }
		
        [ForeignKey("JobsId")]
		public Jobs JobsFk { get; set; }
		
    }
}
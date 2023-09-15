using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.JobClass
{
	[Table("JobClasseses")]
    [Audited]
    public class JobClasses : Entity 
    {

		[StringLength(JobClassesConsts.MaxCodeLength, MinimumLength = JobClassesConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[StringLength(JobClassesConsts.MaxNameLength, MinimumLength = JobClassesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		

    }
}
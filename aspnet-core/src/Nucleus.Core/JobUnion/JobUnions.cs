using Nucleus.Job;
using Nucleus.Union;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.JobUnion
{
	[Table("JobUnions")]
    [Audited]
    public class JobUnions : Entity 
    {

		[StringLength(JobUnionsConsts.MaxNumberLength, MinimumLength = JobUnionsConsts.MinNumberLength)]
		public virtual string Number { get; set; }
		

		public virtual int? JobsId { get; set; }
		
        [ForeignKey("JobsId")]
		public Jobs JobsFk { get; set; }
		
		public virtual int? UnionsId { get; set; }
		
        [ForeignKey("UnionsId")]
		public Unions UnionsFk { get; set; }
		
    }
}
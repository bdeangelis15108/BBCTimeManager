using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.JobCategory
{
	[Table("JobCategories")]
    [Audited]
    public class JobCategories : Entity 
    {

		[StringLength(JobCategoriesConsts.MaxCodeLength, MinimumLength = JobCategoriesConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[StringLength(JobCategoriesConsts.MaxNameLength, MinimumLength = JobCategoriesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		

    }
}
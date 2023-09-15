using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Resource
{
	[Table("Resourceses")]
    [Audited]
    public class Resources : Entity 
    {

		[Required]
		[StringLength(ResourcesConsts.MaxNameLength, MinimumLength = ResourcesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[Required]
		[StringLength(ResourcesConsts.MaxTypeLength, MinimumLength = ResourcesConsts.MinTypeLength)]
		public virtual string Type { get; set; }
		
		public virtual decimal? CostPerHour { get; set; }
		
		public virtual decimal? CostPerUser { get; set; }
		
		public virtual decimal? CostPerDay { get; set; }
		
		public virtual string ResourceNumber { get; set; }
		

    }
}
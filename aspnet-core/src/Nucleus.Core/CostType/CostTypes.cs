using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.CostType
{
	[Table("CostTypese")]
    [Audited]
    public class CostTypes : Entity 
    {

		[StringLength(CostTypesConsts.MaxNameLength, MinimumLength = CostTypesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(CostTypesConsts.MaxCodeLength, MinimumLength = CostTypesConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		

    }
}
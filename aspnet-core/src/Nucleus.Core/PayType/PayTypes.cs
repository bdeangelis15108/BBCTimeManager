using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.PayType
{
	[Table("PayTypeses")]
    [Audited]
    public class PayTypes : Entity 
    {

		[StringLength(PayTypesConsts.MaxCodeLength, MinimumLength = PayTypesConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		[StringLength(PayTypesConsts.MaxDescriptionLength, MinimumLength = PayTypesConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		public virtual decimal Multiplier { get; set; }
		
		public virtual bool Section1 { get; set; }
		
		public virtual bool Section2 { get; set; }
		
		public virtual bool Section3 { get; set; }
		

    }
}
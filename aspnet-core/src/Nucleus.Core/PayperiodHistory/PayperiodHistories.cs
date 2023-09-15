using Nucleus.PayPeriod;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.PayperiodHistory
{
	[Table("PayperiodHistories")]
    [Audited]
    public class PayperiodHistories : Entity 
    {

		[StringLength(PayperiodHistoriesConsts.MaxperiodLength, MinimumLength = PayperiodHistoriesConsts.MinperiodLength)]
		public virtual string period { get; set; }
		
		public virtual bool active { get; set; }
		

		public virtual int? PayPeriodsId { get; set; }
		
        [ForeignKey("PayPeriodsId")]
		public PayPeriods PayPeriodsFk { get; set; }
		
    }
}
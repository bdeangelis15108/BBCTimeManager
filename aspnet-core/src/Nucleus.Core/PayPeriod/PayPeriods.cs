using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.PayPeriod
{
	[Table("PayPeriods")]
    [Audited]
    public class PayPeriods : Entity 
    {

		public virtual DateTime StartDate { get; set; }
		
		public virtual DateTime EndDate { get; set; }
		
		public virtual string Name { get; set; }
		
		public virtual bool IsActive { get; set; }
		

    }
}
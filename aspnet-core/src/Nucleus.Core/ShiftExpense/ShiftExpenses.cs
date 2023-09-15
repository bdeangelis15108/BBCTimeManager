using Nucleus.ShiftResource;
using Nucleus.ExpenseType;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.ShiftExpense
{
	[Table("ShiftExpenses")]
    [Audited]
    public class ShiftExpenses : Entity 
    {

		[StringLength(ShiftExpensesConsts.MaxNameLength, MinimumLength = ShiftExpensesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		public virtual decimal Amount { get; set; }
		

		public virtual int? ShiftResourcesId { get; set; }
		
        [ForeignKey("ShiftResourcesId")]
		public ShiftResources ShiftResourcesFk { get; set; }
		
		public virtual int? ExpenseTypesId { get; set; }
		
        [ForeignKey("ExpenseTypesId")]
		public ExpenseTypes ExpenseTypesFk { get; set; }
		
    }
}
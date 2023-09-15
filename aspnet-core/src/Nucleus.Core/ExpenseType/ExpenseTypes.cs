using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.ExpenseType
{
	[Table("ExpenseTypeses")]
    [Audited]
    public class ExpenseTypes : Entity 
    {

		[StringLength(ExpenseTypesConsts.MaxNameLength, MinimumLength = ExpenseTypesConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		[StringLength(ExpenseTypesConsts.MaxDescriptionLength, MinimumLength = ExpenseTypesConsts.MinDescriptionLength)]
		public virtual string Description { get; set; }
		
		[StringLength(ExpenseTypesConsts.MaxCodeLength, MinimumLength = ExpenseTypesConsts.MinCodeLength)]
		public virtual string Code { get; set; }
		
		public virtual byte? Icon { get; set; }
		

    }
}
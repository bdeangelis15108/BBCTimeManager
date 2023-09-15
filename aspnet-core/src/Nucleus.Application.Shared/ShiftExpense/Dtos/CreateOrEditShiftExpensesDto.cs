
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ShiftExpense.Dtos
{
    public class CreateOrEditShiftExpensesDto : EntityDto<int?>
    {

		[StringLength(ShiftExpensesConsts.MaxNameLength, MinimumLength = ShiftExpensesConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		public decimal Amount { get; set; }
		
		
		 public int? ShiftResourcesId { get; set; }
		 
		 		 public int? ExpenseTypesId { get; set; }
		 
		 
    }
}
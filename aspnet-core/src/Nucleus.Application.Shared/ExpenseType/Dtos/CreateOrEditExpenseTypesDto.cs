
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ExpenseType.Dtos
{
    public class CreateOrEditExpenseTypesDto : EntityDto<int?>
    {

		[StringLength(ExpenseTypesConsts.MaxNameLength, MinimumLength = ExpenseTypesConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		[StringLength(ExpenseTypesConsts.MaxDescriptionLength, MinimumLength = ExpenseTypesConsts.MinDescriptionLength)]
		public string Description { get; set; }
		
		
		[StringLength(ExpenseTypesConsts.MaxCodeLength, MinimumLength = ExpenseTypesConsts.MinCodeLength)]
		public string Code { get; set; }
		
		
		public byte? Icon { get; set; }
		
		

    }
}
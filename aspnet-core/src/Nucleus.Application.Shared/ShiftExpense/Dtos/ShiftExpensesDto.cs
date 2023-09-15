
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.ShiftExpense.Dtos
{
    public class ShiftExpensesDto : EntityDto
    {
		public string Name { get; set; }

		public decimal Amount { get; set; }


		 public int? ShiftResourcesId { get; set; }

		 		 public int? ExpenseTypesId { get; set; }

		 
    }
}
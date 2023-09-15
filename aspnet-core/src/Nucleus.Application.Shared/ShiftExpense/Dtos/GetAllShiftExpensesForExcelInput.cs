using Abp.Application.Services.Dto;
using System;

namespace Nucleus.ShiftExpense.Dtos
{
    public class GetAllShiftExpensesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public decimal? MaxAmountFilter { get; set; }
		public decimal? MinAmountFilter { get; set; }


		 public string ShiftResourcesNameFilter { get; set; }

		 		 public string ExpenseTypesNameFilter { get; set; }

		 
    }
}
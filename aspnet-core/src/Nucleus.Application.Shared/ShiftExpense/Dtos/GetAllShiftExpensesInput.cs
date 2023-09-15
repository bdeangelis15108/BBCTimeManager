using Abp.Application.Services.Dto;
using System;

namespace Nucleus.ShiftExpense.Dtos
{
    public class GetAllShiftExpensesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public decimal? MaxAmountFilter { get; set; }
		public decimal? MinAmountFilter { get; set; }


		 public string ShiftResourcesNameFilter { get; set; }

		 		 public string ExpenseTypesNameFilter { get; set; }
        public int ShiftResourcesIdFilter { get; set; }

        public GetAllShiftExpensesInput()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }
    }
}
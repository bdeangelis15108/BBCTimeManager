using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ShiftExpense.Dtos
{
    public class GetShiftExpensesForEditOutput
    {
		public CreateOrEditShiftExpensesDto ShiftExpenses { get; set; }

		public string ShiftResourcesName { get; set;}

		public string ExpenseTypesName { get; set;}


    }
}
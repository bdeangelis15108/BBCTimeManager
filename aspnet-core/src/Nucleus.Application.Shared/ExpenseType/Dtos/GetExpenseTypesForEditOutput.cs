using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ExpenseType.Dtos
{
    public class GetExpenseTypesForEditOutput
    {
		public CreateOrEditExpenseTypesDto ExpenseTypes { get; set; }


    }
}
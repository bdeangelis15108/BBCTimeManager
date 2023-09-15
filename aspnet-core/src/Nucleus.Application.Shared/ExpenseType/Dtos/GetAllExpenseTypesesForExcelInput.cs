﻿using Abp.Application.Services.Dto;
using System;

namespace Nucleus.ExpenseType.Dtos
{
    public class GetAllExpenseTypesesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public string DescriptionFilter { get; set; }

		public string CodeFilter { get; set; }

		public byte? MaxIconFilter { get; set; }
		public byte? MinIconFilter { get; set; }



    }
}

using System;
using Abp.Application.Services.Dto;

namespace Nucleus.ExpenseType.Dtos
{
    public class ExpenseTypesDto : EntityDto
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public string Code { get; set; }

		public byte? Icon { get; set; }



    }
}
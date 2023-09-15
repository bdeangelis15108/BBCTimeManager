
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.ShiftResource.Dtos
{
    public class ShiftResourcesDto : EntityDto
    {
		public decimal? HoursWorked { get; set; }

		public string Name { get; set; }


		 public int ResourcesId { get; set; }

		 		 public int? PayTypesId { get; set; }

		 		 public int? JobPhaseCodesId { get; set; }

		 		 public int? JobCategoriesId { get; set; }

		 		 public int? TimesheetsId { get; set; }

		 		 public int? ShiftsId { get; set; }

		 		 public int? WorkerClaseesId { get; set; }

		 
    }
}
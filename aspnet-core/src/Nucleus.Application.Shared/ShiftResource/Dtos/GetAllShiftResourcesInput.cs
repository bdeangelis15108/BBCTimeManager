using Abp.Application.Services.Dto;
using System;

namespace Nucleus.ShiftResource.Dtos
{
    public class GetAllShiftResourcesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public decimal? MaxHoursWorkedFilter { get; set; }
		public decimal? MinHoursWorkedFilter { get; set; }

		public string NameFilter { get; set; }


		 public string ResourcesNameFilter { get; set; }

		 		 public string PayTypesCodeFilter { get; set; }

		 		 public string JobPhaseCodesNameFilter { get; set; }

		 		 public string JobCategoriesNameFilter { get; set; }

		 		 public string TimesheetsNameFilter { get; set; }

		 		 public string ShiftsNameFilter { get; set; }

		 		 public string WorkerClaseesNameFilter { get; set; }
		public int TimesheetId { get; set; }
		public DateTime? MaxCreatedDateFilter { get; set; }
		public DateTime? MinCreatedDateFilter { get; set; }
        public string ResourcesTypeFilter { get; set; }
        public string TimesheetsStatusFilter { get; set; }
    }
}
using Abp.Application.Services.Dto;
using System;

namespace Nucleus.StatusUpdate.Dtos
{
    public class GetAllStatusUpdatesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public DateTime? MaxModifiedOnFilter { get; set; }
		public DateTime? MinModifiedOnFilter { get; set; }

		public string NameFilter { get; set; }

		public int? MaxOriginalstatusIdFilter { get; set; }
		public int? MinOriginalstatusIdFilter { get; set; }

		public DateTime? MaxActualCreateDateTimeFilter { get; set; }
		public DateTime? MinActualCreateDateTimeFilter { get; set; }

		public string TimeshetIdsFilter { get; set; }


		 public string TimesheetsNameFilter { get; set; }

		 		 public string StatusesNameFilter { get; set; }

		 		 public string JobsNameFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}
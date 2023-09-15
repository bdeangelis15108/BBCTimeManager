
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.StatusUpdate.Dtos
{
    public class StatusUpdatesDto : EntityDto
    {
		public DateTime? ModifiedOn { get; set; }

		public string Name { get; set; }

		public int? OriginalstatusId { get; set; }

		public DateTime? ActualCreateDateTime { get; set; }

		public string TimeshetIds { get; set; }


		 public int? TimesheetsId { get; set; }

		 		 public int? NewStatusesId { get; set; }

		 		 public int? JobsId { get; set; }

		 		 public long? ModifiedBy { get; set; }

		 
    }
}
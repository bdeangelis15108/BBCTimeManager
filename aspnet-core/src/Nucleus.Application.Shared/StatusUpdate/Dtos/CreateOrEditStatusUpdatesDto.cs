
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.StatusUpdate.Dtos
{
    public class CreateOrEditStatusUpdatesDto : EntityDto<int?>
    {

		public DateTime? ModifiedOn { get; set; }
		
		
		[StringLength(StatusUpdatesConsts.MaxNameLength, MinimumLength = StatusUpdatesConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		public int? OriginalstatusId { get; set; }
		
		
		public DateTime? ActualCreateDateTime { get; set; }
		
		
		[StringLength(StatusUpdatesConsts.MaxTimeshetIdsLength, MinimumLength = StatusUpdatesConsts.MinTimeshetIdsLength)]
		public string TimeshetIds { get; set; }
		
		
		 public int? TimesheetsId { get; set; }
		 
		 		 public int? NewStatusesId { get; set; }
		 
		 		 public int? JobsId { get; set; }
		 
		 		 public long? ModifiedBy { get; set; }
		 
		 
    }
}
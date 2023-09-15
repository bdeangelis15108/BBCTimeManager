
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Status.Dtos
{
    public class CreateOrEditStatusesDto : EntityDto<int?>
    {

		[StringLength(StatusesConsts.MaxNameLength, MinimumLength = StatusesConsts.MinNameLength)]
		public string Name { get; set; }
		
		
		public bool IsDefault { get; set; }
		
		
		[StringLength(StatusesConsts.MaxForwardNameLength, MinimumLength = StatusesConsts.MinForwardNameLength)]
		public string ForwardName { get; set; }
		
		
		[StringLength(StatusesConsts.MaxReverseNameLength, MinimumLength = StatusesConsts.MinReverseNameLength)]
		public string ReverseName { get; set; }
		
		
		public int? ForwardId { get; set; }
		
		
		public int? ReverseId { get; set; }
		
		

    }
}
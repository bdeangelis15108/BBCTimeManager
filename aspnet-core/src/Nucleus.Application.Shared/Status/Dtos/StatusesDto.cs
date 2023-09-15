
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.Status.Dtos
{
    public class StatusesDto : EntityDto
    {
		public string Name { get; set; }

		public bool IsDefault { get; set; }

		public string ForwardName { get; set; }

		public string ReverseName { get; set; }

		public int? ForwardId { get; set; }

		public int? ReverseId { get; set; }



    }
}
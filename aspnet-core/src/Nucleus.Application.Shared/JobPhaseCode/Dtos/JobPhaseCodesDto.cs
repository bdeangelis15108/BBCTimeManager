
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.JobPhaseCode.Dtos
{
    public class JobPhaseCodesDto : EntityDto
    {
		public string Code { get; set; }

		public string Name { get; set; }


		 public int JobsId { get; set; }

		 
    }
}
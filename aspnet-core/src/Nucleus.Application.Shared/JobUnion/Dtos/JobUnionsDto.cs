
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.JobUnion.Dtos
{
    public class JobUnionsDto : EntityDto
    {
		public string Number { get; set; }


		 public int? JobsId { get; set; }

		 		 public int? UnionsId { get; set; }

		public Nucleus.Job.Dtos.JobsDto Jobs { get; set; }
	}
}
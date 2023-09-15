
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.Job.Dtos
{
    public class JobsDto : EntityDto
    {
		public string number { get; set; }
		public string Code { get; set; }

		public string Name { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public int Status { get; set; }


		 public int? AddressesId { get; set; }

		 		 public int? JobClassesId { get; set; }

		 
    }
}
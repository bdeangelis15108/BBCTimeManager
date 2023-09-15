
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.ResourceWorkerInfo.Dtos
{
    public class ResourceWorkerInfosDto : EntityDto
    {
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string UnionNumber { get; set; }

		public string UnionLocal { get; set; }
		public string RefNumber { get; set; }
		public string Wcomp1 { get; set; }


		 public int? WorkerClaseesId { get; set; }

		 		 public int? ResourcesId { get; set; }

		 
    }
}
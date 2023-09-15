using Abp.Application.Services.Dto;
using System;

namespace Nucleus.ResourceWorkerInfo.Dtos
{
    public class GetAllResourceWorkerInfosesForExcelInput
    {
		public string Filter { get; set; }

		public string FirstNameFilter { get; set; }

		public string LastNameFilter { get; set; }

		public string UnionNumberFilter { get; set; }

		public string UnionLocalFilter { get; set; }

		public string Wcomp1Filter { get; set; }


		 public string WorkerClaseesNameFilter { get; set; }

		 		 public string ResourcesNameFilter { get; set; }

		 
    }
}
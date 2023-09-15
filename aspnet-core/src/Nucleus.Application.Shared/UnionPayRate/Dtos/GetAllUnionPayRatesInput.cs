using Abp.Application.Services.Dto;
using System;

namespace Nucleus.UnionPayRate.Dtos
{
    public class GetAllUnionPayRatesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string ClassFilter { get; set; }

		public string DedtypeFilter { get; set; }

		public decimal? MaxPerhourFilter { get; set; }
		public decimal? MinPerhourFilter { get; set; }


		 public string UnionsNumberFilter { get; set; }

		 
    }
}
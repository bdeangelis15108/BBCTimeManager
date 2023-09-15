using Abp.Application.Services.Dto;
using System;

namespace Nucleus.Status.Dtos
{
    public class GetAllStatusesForExcelInput
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }

		public int IsDefaultFilter { get; set; }

		public string ForwardNameFilter { get; set; }

		public string ReverseNameFilter { get; set; }

		public int? MaxForwardIdFilter { get; set; }
		public int? MinForwardIdFilter { get; set; }

		public int? MaxReverseIdFilter { get; set; }
		public int? MinReverseIdFilter { get; set; }



    }
}
using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JCCAT.Dtos
{
    public class GetAllJACCATsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxSEQUENCEFilter { get; set; }
		public int? MinSEQUENCEFilter { get; set; }

		public string JOBNUMFilter { get; set; }

		public string PHASENUMFilter { get; set; }

		public string CATNUMFilter { get; set; }

		public string NAMEFilter { get; set; }



    }
}
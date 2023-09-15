using Abp.Application.Services.Dto;
using System;

namespace Nucleus.ECCOSTS.Dtos
{
    public class GetAllECCOSTSForExcelInput
    {
		public string Filter { get; set; }

		public string CODENUMFilter { get; set; }

		public string ESTHOURLYFilter { get; set; }


		 public string EQUIPMENTEQUIPNUMFilter { get; set; }

		 
    }
}
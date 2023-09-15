using Abp.Application.Services.Dto;
using System;

namespace Nucleus.JCUNIONS.Dtos
{
    public class GetAllJCUNIONsForExcelInput
    {
		public string Filter { get; set; }

		public string UNIONNUMFilter { get; set; }

		public string UNIONLOCALFilter { get; set; }


		 public string JACCATJOBNUMFilter { get; set; }

		 
    }
}
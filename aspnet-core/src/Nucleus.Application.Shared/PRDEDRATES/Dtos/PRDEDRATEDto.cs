
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.PRDEDRATES.Dtos
{
    public class PRDEDRATEDto : EntityDto
    {
		public string UNIONLOCAL { get; set; }

		public string CLASS { get; set; }

		public int DEDTYPE { get; set; }

		public decimal PERHR { get; set; }


		 public int UNIONNUM { get; set; }

		 
    }
}
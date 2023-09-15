
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.PREMPLOYEES.Dtos
{
    public class PREMPLOYEEDto : EntityDto
    {
		public string EMPNUM { get; set; }

		public string NAME { get; set; }

		public string UNIONNUM { get; set; }

		public string UNIONLOCAL { get; set; }

		public string CLASS { get; set; }

		public string WCOMPNUM1 { get; set; }

		public string LASTNAME { get; set; }

		public string FIRSTNAME { get; set; }

		public string STATUS { get; set; }

		public decimal PAYRATE { get; set; }



    }
}
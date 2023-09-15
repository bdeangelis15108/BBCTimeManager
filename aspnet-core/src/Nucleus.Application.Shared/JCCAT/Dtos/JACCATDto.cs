
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.JCCAT.Dtos
{
    public class JACCATDto : EntityDto
    {
		public int SEQUENCE { get; set; }

		public string JOBNUM { get; set; }

		public string PHASENUM { get; set; }

		public string CATNUM { get; set; }

		public string NAME { get; set; }



    }
}
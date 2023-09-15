
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.JCJOBS.Dtos
{
    public class JCJOBDto : EntityDto
    {
		public string STATE { get; set; }

		public string LOCALITY { get; set; }

		public string CLASS { get; set; }

		public int CLOSED { get; set; }


		 public int JOBNUM { get; set; }

		 
    }
}

using System;
using Abp.Application.Services.Dto;

namespace Nucleus.JCUNIONS.Dtos
{
    public class JCUNIONDto : EntityDto
    {
		public string UNIONNUM { get; set; }

		public string UNIONLOCAL { get; set; }


		 public int JOBNUM { get; set; }

		 
    }
}

using System;
using Abp.Application.Services.Dto;

namespace Nucleus.UnionPayRate.Dtos
{
    public class UnionPayRatesDto : EntityDto
    {
		public string Class { get; set; }

		public string Dedtype { get; set; }

		public decimal Perhour { get; set; }


		 public int? UnionsId { get; set; }

		 
    }
}
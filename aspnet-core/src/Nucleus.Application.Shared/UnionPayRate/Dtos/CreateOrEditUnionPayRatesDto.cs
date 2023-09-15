
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.UnionPayRate.Dtos
{
    public class CreateOrEditUnionPayRatesDto : EntityDto<int?>
    {

		[StringLength(UnionPayRatesConsts.MaxClassLength, MinimumLength = UnionPayRatesConsts.MinClassLength)]
		public string Class { get; set; }
		
		
		[StringLength(UnionPayRatesConsts.MaxDedtypeLength, MinimumLength = UnionPayRatesConsts.MinDedtypeLength)]
		public string Dedtype { get; set; }
		
		
		[Range(UnionPayRatesConsts.MinPerhourValue, UnionPayRatesConsts.MaxPerhourValue)]
		public decimal Perhour { get; set; }
		
		
		 public int? UnionsId { get; set; }
		 
		 
    }
}
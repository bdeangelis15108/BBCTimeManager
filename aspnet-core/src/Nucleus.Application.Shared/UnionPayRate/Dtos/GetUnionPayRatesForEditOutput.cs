using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.UnionPayRate.Dtos
{
    public class GetUnionPayRatesForEditOutput
    {
		public CreateOrEditUnionPayRatesDto UnionPayRates { get; set; }

		public string UnionsNumber { get; set;}


    }
}
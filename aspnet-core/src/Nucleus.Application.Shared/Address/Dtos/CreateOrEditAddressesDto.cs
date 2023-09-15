
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Address.Dtos
{
    public class CreateOrEditAddressesDto : EntityDto<int?>
    {

		[StringLength(AddressesConsts.MaxLinne1Length, MinimumLength = AddressesConsts.MinLinne1Length)]
		public string Linne1 { get; set; }
		
		
		[StringLength(AddressesConsts.MaxLine2Length, MinimumLength = AddressesConsts.MinLine2Length)]
		public string Line2 { get; set; }
		
		
		[StringLength(AddressesConsts.MaxCityLength, MinimumLength = AddressesConsts.MinCityLength)]
		public string City { get; set; }
		
		
		[StringLength(AddressesConsts.MaxStateLength, MinimumLength = AddressesConsts.MinStateLength)]
		public string State { get; set; }
		
		
		[StringLength(AddressesConsts.MaxZipLength, MinimumLength = AddressesConsts.MinZipLength)]
		public string Zip { get; set; }
		
		
		[StringLength(AddressesConsts.MaxLanLength, MinimumLength = AddressesConsts.MinLanLength)]
		public string Lan { get; set; }
		
		
		[StringLength(AddressesConsts.MaxLatLength, MinimumLength = AddressesConsts.MinLatLength)]
		public string Lat { get; set; }
		
		

    }
}
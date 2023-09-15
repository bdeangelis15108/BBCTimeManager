using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.Address.Dtos
{
    public class GetAddressesForEditOutput
    {
		public CreateOrEditAddressesDto Addresses { get; set; }


    }
}
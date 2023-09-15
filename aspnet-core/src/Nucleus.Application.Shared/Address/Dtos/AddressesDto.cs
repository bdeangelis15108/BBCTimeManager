
using System;
using Abp.Application.Services.Dto;

namespace Nucleus.Address.Dtos
{
    public class AddressesDto : EntityDto
    {
		public string Linne1 { get; set; }

		public string Line2 { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Zip { get; set; }

		public string Lan { get; set; }

		public string Lat { get; set; }



    }
}
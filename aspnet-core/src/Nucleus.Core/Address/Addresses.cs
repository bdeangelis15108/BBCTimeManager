using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Nucleus.Address
{
	[Table("Addresseses")]
    [Audited]
    public class Addresses : Entity 
    {

		[StringLength(AddressesConsts.MaxLinne1Length, MinimumLength = AddressesConsts.MinLinne1Length)]
		public virtual string Linne1 { get; set; }
		
		[StringLength(AddressesConsts.MaxLine2Length, MinimumLength = AddressesConsts.MinLine2Length)]
		public virtual string Line2 { get; set; }
		
		[StringLength(AddressesConsts.MaxCityLength, MinimumLength = AddressesConsts.MinCityLength)]
		public virtual string City { get; set; }
		
		[StringLength(AddressesConsts.MaxStateLength, MinimumLength = AddressesConsts.MinStateLength)]
		public virtual string State { get; set; }
		
		[StringLength(AddressesConsts.MaxZipLength, MinimumLength = AddressesConsts.MinZipLength)]
		public virtual string Zip { get; set; }
		
		[StringLength(AddressesConsts.MaxLanLength, MinimumLength = AddressesConsts.MinLanLength)]
		public virtual string Lan { get; set; }
		
		[StringLength(AddressesConsts.MaxLatLength, MinimumLength = AddressesConsts.MinLatLength)]
		public virtual string Lat { get; set; }
		

    }
}
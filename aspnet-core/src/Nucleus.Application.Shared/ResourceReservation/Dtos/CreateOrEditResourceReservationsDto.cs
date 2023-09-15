
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ResourceReservation.Dtos
{
    public class CreateOrEditResourceReservationsDto : EntityDto<int?>
    {

		public DateTime? ReservedFrom { get; set; }
		
		
		public DateTime? ReservedUntil { get; set; }
		
		
		 public long? UserId { get; set; }
		 
		 		 public int? ResourcesId { get; set; }
		 
		 
    }
}
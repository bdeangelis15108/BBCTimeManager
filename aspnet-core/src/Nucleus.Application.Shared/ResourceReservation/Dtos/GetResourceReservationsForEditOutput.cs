using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ResourceReservation.Dtos
{
    public class GetResourceReservationsForEditOutput
    {
		public CreateOrEditResourceReservationsDto ResourceReservations { get; set; }

		public string UserName { get; set;}

		public string ResourcesName { get; set;}


    }
}
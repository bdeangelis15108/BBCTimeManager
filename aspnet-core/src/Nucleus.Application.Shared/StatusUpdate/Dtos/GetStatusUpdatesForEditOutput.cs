using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.StatusUpdate.Dtos
{
    public class GetStatusUpdatesForEditOutput
    {
		public CreateOrEditStatusUpdatesDto StatusUpdates { get; set; }

		public string TimesheetsName { get; set;}

		public string StatusesName { get; set;}

		public string JobsName { get; set;}

		public string UserName { get; set;}


    }
}
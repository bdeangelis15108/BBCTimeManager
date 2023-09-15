﻿using Abp.Application.Services.Dto;
using System;

namespace Nucleus.PayperiodHistory.Dtos
{
    public class GetAllPayperiodHistoriesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string periodFilter { get; set; }

		public int? activeFilter { get; set; }


		 public string PayPeriodsNameFilter { get; set; }

		 
    }
}
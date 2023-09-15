using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Nucleus.ResourceWorkerInfo.Dtos
{
    public class GetResourceWorkerInfosForEditOutput
    {
		public CreateOrEditResourceWorkerInfosDto ResourceWorkerInfos { get; set; }

		public string WorkerClaseesName { get; set;}

		public string ResourcesName { get; set;}


    }
}
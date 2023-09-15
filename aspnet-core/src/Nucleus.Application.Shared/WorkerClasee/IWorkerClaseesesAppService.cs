using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.WorkerClasee.Dtos;
using Nucleus.Dto;


namespace Nucleus.WorkerClasee
{
    public interface IWorkerClaseesesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetWorkerClaseesForViewDto>> GetAll(GetAllWorkerClaseesesInput input);

		Task<GetWorkerClaseesForEditOutput> GetWorkerClaseesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditWorkerClaseesDto input);

		Task Delete(EntityDto input);

		
    }
}
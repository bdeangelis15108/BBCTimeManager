using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Resource.Dtos;
using Nucleus.Dto;


namespace Nucleus.Resource
{
    public interface IResourcesesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetResourcesForViewDto>> GetAll(GetAllResourcesesInput input);

        Task<GetResourcesForViewDto> GetResourcesForView(int id);

        Task<GetResourcesForEditOutput> GetResourcesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditResourcesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetResourcesesToExcel(GetAllResourcesesForExcelInput input);

		
    }
}
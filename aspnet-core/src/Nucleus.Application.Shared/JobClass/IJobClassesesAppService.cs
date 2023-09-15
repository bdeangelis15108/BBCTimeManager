using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.JobClass.Dtos;
using Nucleus.Dto;


namespace Nucleus.JobClass
{
    public interface IJobClassesesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJobClassesForViewDto>> GetAll(GetAllJobClassesesInput input);

        Task<GetJobClassesForViewDto> GetJobClassesForView(int id);

		Task<GetJobClassesForEditOutput> GetJobClassesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJobClassesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJobClassesesToExcel(GetAllJobClassesesForExcelInput input);

		
    }
}
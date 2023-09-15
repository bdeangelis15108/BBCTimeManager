using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.JobCategory.Dtos;
using Nucleus.Dto;


namespace Nucleus.JobCategory
{
    public interface IJobCategoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetJobCategoriesForViewDto>> GetAll(GetAllJobCategoriesInput input);

        Task<GetJobCategoriesForViewDto> GetJobCategoriesForView(int id);

		Task<GetJobCategoriesForEditOutput> GetJobCategoriesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditJobCategoriesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetJobCategoriesToExcel(GetAllJobCategoriesForExcelInput input);

		
    }
}
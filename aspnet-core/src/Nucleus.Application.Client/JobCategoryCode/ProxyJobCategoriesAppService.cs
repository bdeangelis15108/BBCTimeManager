using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.JobCategory;
using Nucleus.JobCategory.Dtos;
using Nucleus.JobPhaseCode;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;

namespace Nucleus.JobPhaseCodes
{
    public class ProxyJobCategoriesAppService : ProxyAppServiceBase, IJobCategoriesAppService
    {
        public async Task CreateOrEdit(CreateOrEditJobCategoriesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditJobCategoriesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<PagedResultDto<GetJobCategoriesForViewDto>> GetAll(GetAllJobCategoriesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetJobCategoriesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<GetJobCategoriesForEditOutput> GetJobCategoriesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetJobCategoriesForEditOutput>(GetEndpoint(nameof(GetJobCategoriesForEdit)), input);
        }

        public async Task<GetJobCategoriesForViewDto> GetJobCategoriesForView(int id)
        {
            return await ApiClient.GetAsync<GetJobCategoriesForViewDto>(GetEndpoint(nameof(GetJobCategoriesForView)), id);
        }

        public async Task<FileDto> GetJobCategoriesToExcel(GetAllJobCategoriesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetJobCategoriesToExcel)), input);
        }
    }
}
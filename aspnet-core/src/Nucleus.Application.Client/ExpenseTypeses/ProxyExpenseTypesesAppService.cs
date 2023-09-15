using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Dto;
using Nucleus.ExpenseType;
using Nucleus.ExpenseType.Dtos;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;

namespace Nucleus.ExpenseTypeses
{
    public class ProxyExpenseTypesesAppService : ProxyAppServiceBase, IExpenseTypesesAppService
    {
        public async Task CreateOrEdit(CreateOrEditExpenseTypesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditExpenseTypesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task Delete(EntityDto input)
        {
            await ApiClient.PostAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async  Task<PagedResultDto<GetExpenseTypesForViewDto>> GetAll(GetAllExpenseTypesesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetExpenseTypesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }      
               
        public async Task<FileDto> GetExpenseTypesesToExcel(GetAllExpenseTypesesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetExpenseTypesesToExcel)), input);
        }      
               
        public async Task<GetExpenseTypesForEditOutput> GetExpenseTypesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetExpenseTypesForEditOutput>(GetEndpoint(nameof(GetExpenseTypesForEdit)), input);
        }      
               
        public async Task<GetExpenseTypesForViewDto> GetExpenseTypesForView(int id)
        {
            return await ApiClient.GetAsync <GetExpenseTypesForViewDto> (GetEndpoint(nameof(GetExpenseTypesForView)), id);
        }
    }
}
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Nucleus.Authorization.Users.Dto;
using Nucleus.Dto;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.JobUnion;
using Nucleus.JobUnion.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.ShiftExpense.Dtos;

namespace Nucleus.ShiftExpense
{
    public class ProxyShiftExpensesAppService : ProxyAppServiceBase, IShiftExpensesAppService
    {
        public async Task Delete(EntityDto input)
        {
            await ApiClient.DeleteAsync<EntityDto>(GetEndpoint(nameof(Delete)), input);
        }

        public async Task<PagedResultDto<GetShiftExpensesForViewDto>> GetAll(GetAllShiftExpensesInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<GetShiftExpensesForViewDto>>(GetEndpoint(nameof(GetAll)), input);
        }

        public async Task<GetShiftExpensesForViewDto> GetShiftExpensesForView(int id)
        {
            return await ApiClient.GetAsync<GetShiftExpensesForViewDto>(GetEndpoint(nameof(GetShiftExpensesForView)), id);
        }

        public async Task<GetShiftExpensesForEditOutput> GetShiftExpensesForEdit(EntityDto input)
        {
            return await ApiClient.GetAsync<GetShiftExpensesForEditOutput>(GetEndpoint(nameof(GetShiftExpensesForEdit)), input);
        }

        public async Task CreateOrEdit(CreateOrEditShiftExpensesDto input)
        {
            await ApiClient.PostAsync<CreateOrEditShiftExpensesDto>(GetEndpoint(nameof(CreateOrEdit)), input);
        }

        public async Task<FileDto> GetShiftExpensesToExcel(GetAllShiftExpensesForExcelInput input)
        {
            return await ApiClient.GetAsync<FileDto>(GetEndpoint(nameof(GetShiftExpensesToExcel)), input);
        }

        public async Task<PagedResultDto<ShiftExpensesShiftResourcesLookupTableDto>> GetAllShiftResourcesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftExpensesShiftResourcesLookupTableDto>>(GetEndpoint(nameof(GetAllShiftResourcesForLookupTable)), input);
        }

        public async Task<PagedResultDto<ShiftExpensesExpenseTypesLookupTableDto>> GetAllExpenseTypesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
            return await ApiClient.GetAsync<PagedResultDto<ShiftExpensesExpenseTypesLookupTableDto>>(GetEndpoint(nameof(GetAllExpenseTypesForLookupTable)), input);
        }
    }
}
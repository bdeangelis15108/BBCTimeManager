using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.ShiftExpense.Dtos;
using Nucleus.Dto;


namespace Nucleus.ShiftExpense
{
    public interface IShiftExpensesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShiftExpensesForViewDto>> GetAll(GetAllShiftExpensesInput input);

        Task<GetShiftExpensesForViewDto> GetShiftExpensesForView(int id);

		Task<GetShiftExpensesForEditOutput> GetShiftExpensesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditShiftExpensesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetShiftExpensesToExcel(GetAllShiftExpensesForExcelInput input);

		
		Task<PagedResultDto<ShiftExpensesShiftResourcesLookupTableDto>> GetAllShiftResourcesForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ShiftExpensesExpenseTypesLookupTableDto>> GetAllExpenseTypesForLookupTable(GetAllForLookupTableInput input);
		
    }
}
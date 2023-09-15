using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.ExpenseType.Dtos;
using Nucleus.Dto;


namespace Nucleus.ExpenseType
{
    public interface IExpenseTypesesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetExpenseTypesForViewDto>> GetAll(GetAllExpenseTypesesInput input);

        Task<GetExpenseTypesForViewDto> GetExpenseTypesForView(int id);

		Task<GetExpenseTypesForEditOutput> GetExpenseTypesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditExpenseTypesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetExpenseTypesesToExcel(GetAllExpenseTypesesForExcelInput input);

		
    }
}
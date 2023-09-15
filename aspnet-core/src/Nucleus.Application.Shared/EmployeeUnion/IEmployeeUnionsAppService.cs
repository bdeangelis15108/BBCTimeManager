using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.Dto;


namespace Nucleus.EmployeeUnion
{
    public interface IEmployeeUnionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeUnionsForViewDto>> GetAll(GetAllEmployeeUnionsInput input);

        Task<GetEmployeeUnionsForViewDto> GetEmployeeUnionsForView(int id);

		Task<GetEmployeeUnionsForEditOutput> GetEmployeeUnionsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeUnionsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeUnionsToExcel(GetAllEmployeeUnionsForExcelInput input);

		
		Task<PagedResultDto<EmployeeUnionsUnionsLookupTableDto>> GetAllUnionsForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<EmployeeUnionsResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input);
		
    }
}
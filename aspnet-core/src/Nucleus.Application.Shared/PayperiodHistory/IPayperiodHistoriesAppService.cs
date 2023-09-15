using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.PayperiodHistory.Dtos;
using Nucleus.Dto;


namespace Nucleus.PayperiodHistory
{
    public interface IPayperiodHistoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPayperiodHistoriesForViewDto>> GetAll(GetAllPayperiodHistoriesInput input);

        Task<GetPayperiodHistoriesForViewDto> GetPayperiodHistoriesForView(int id);

		Task<GetPayperiodHistoriesForEditOutput> GetPayperiodHistoriesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPayperiodHistoriesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPayperiodHistoriesToExcel(GetAllPayperiodHistoriesForExcelInput input);

		
		Task<PagedResultDto<PayperiodHistoriesPayPeriodsLookupTableDto>> GetAllPayPeriodsForLookupTable(GetAllForLookupTableInput input);
		
    }
}
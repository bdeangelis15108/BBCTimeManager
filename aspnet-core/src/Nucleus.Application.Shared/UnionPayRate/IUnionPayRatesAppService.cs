using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.UnionPayRate.Dtos;
using Nucleus.Dto;


namespace Nucleus.UnionPayRate
{
    public interface IUnionPayRatesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUnionPayRatesForViewDto>> GetAll(GetAllUnionPayRatesInput input);

        Task<GetUnionPayRatesForViewDto> GetUnionPayRatesForView(int id);

		Task<GetUnionPayRatesForEditOutput> GetUnionPayRatesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditUnionPayRatesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUnionPayRatesToExcel(GetAllUnionPayRatesForExcelInput input);

		
		Task<PagedResultDto<UnionPayRatesUnionsLookupTableDto>> GetAllUnionsForLookupTable(GetAllForLookupTableInput input);
		
    }
}
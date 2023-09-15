using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.CostType.Dtos;
using Nucleus.Dto;


namespace Nucleus.CostType
{
    public interface ICostTypeseAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCostTypesForViewDto>> GetAll(GetAllCostTypeseInput input);

        Task<GetCostTypesForViewDto> GetCostTypesForView(int id);

		Task<GetCostTypesForEditOutput> GetCostTypesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCostTypesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCostTypeseToExcel(GetAllCostTypeseForExcelInput input);

		
    }
}
using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.PayType.Dtos;
using Nucleus.Dto;


namespace Nucleus.PayType
{
    public interface IPayTypesesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPayTypesForViewDto>> GetAll(GetAllPayTypesesInput input);

        Task<GetPayTypesForViewDto> GetPayTypesForView(int id);

		Task<GetPayTypesForEditOutput> GetPayTypesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPayTypesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPayTypesesToExcel(GetAllPayTypesesForExcelInput input);

		
    }
}
using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.Address.Dtos;
using Nucleus.Dto;


namespace Nucleus.Address
{
    public interface IAddressesesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAddressesForViewDto>> GetAll(GetAllAddressesesInput input);

        Task<GetAddressesForViewDto> GetAddressesForView(int id);

		Task<GetAddressesForEditOutput> GetAddressesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAddressesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetAddressesesToExcel(GetAllAddressesesForExcelInput input);

		
    }
}
using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Nucleus.PayPeriod.Dtos;
using Nucleus.Dto;


namespace Nucleus.PayPeriod
{
    public interface IPayPeriodsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPayPeriodsForViewDto>> GetAll(GetAllPayPeriodsInput input);

        Task<GetPayPeriodsForViewDto> GetPayPeriodsForView(int id);

		Task<GetPayPeriodsForEditOutput> GetPayPeriodsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPayPeriodsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPayPeriodsToExcel(GetAllPayPeriodsForExcelInput input);

		
    }
}
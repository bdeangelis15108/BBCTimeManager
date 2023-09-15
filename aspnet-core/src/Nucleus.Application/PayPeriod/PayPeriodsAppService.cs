

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.PayPeriod.Exporting;
using Nucleus.PayPeriod.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.PayPeriod
{
	[AbpAuthorize(AppPermissions.Pages_PayPeriods)]
    public class PayPeriodsAppService : NucleusAppServiceBase, IPayPeriodsAppService
    {
		 private readonly IRepository<PayPeriods> _payPeriodsRepository;
		 private readonly IPayPeriodsExcelExporter _payPeriodsExcelExporter;
		 

		  public PayPeriodsAppService(IRepository<PayPeriods> payPeriodsRepository, IPayPeriodsExcelExporter payPeriodsExcelExporter ) 
		  {
			_payPeriodsRepository = payPeriodsRepository;
			_payPeriodsExcelExporter = payPeriodsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPayPeriodsForViewDto>> GetAll(GetAllPayPeriodsInput input)
         {

			var filteredPayPeriods = _payPeriodsRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);
						//.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

			var pagedAndFilteredPayPeriods = filteredPayPeriods
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var payPeriods = from o in pagedAndFilteredPayPeriods
                         select new GetPayPeriodsForViewDto() {
							PayPeriods = new PayPeriodsDto
							{
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                Name = o.Name,
                                IsActive = o.IsActive,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPayPeriods.CountAsync();

            return new PagedResultDto<GetPayPeriodsForViewDto>(
                totalCount,
                await payPeriods.ToListAsync()
            );
         }
		 
		 public async Task<GetPayPeriodsForViewDto> GetPayPeriodsForView(int id)
         {
            var payPeriods = await _payPeriodsRepository.GetAsync(id);

            var output = new GetPayPeriodsForViewDto { PayPeriods = ObjectMapper.Map<PayPeriodsDto>(payPeriods) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PayPeriods_Edit)]
		 public async Task<GetPayPeriodsForEditOutput> GetPayPeriodsForEdit(EntityDto input)
         {
            var payPeriods = await _payPeriodsRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPayPeriodsForEditOutput {PayPeriods = ObjectMapper.Map<CreateOrEditPayPeriodsDto>(payPeriods)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPayPeriodsDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PayPeriods_Create)]
		 protected virtual async Task Create(CreateOrEditPayPeriodsDto input)
         {
            var payPeriods = ObjectMapper.Map<PayPeriods>(input);

			

            await _payPeriodsRepository.InsertAsync(payPeriods);
         }

		 [AbpAuthorize(AppPermissions.Pages_PayPeriods_Edit)]
		 protected virtual async Task Update(CreateOrEditPayPeriodsDto input)
         {
            var payPeriods = await _payPeriodsRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, payPeriods);
         }

		 [AbpAuthorize(AppPermissions.Pages_PayPeriods_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _payPeriodsRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPayPeriodsToExcel(GetAllPayPeriodsForExcelInput input)
         {
			
			var filteredPayPeriods = _payPeriodsRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
						.WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
						.WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
						.WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.IsActiveFilter > -1,  e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive) );

			var query = (from o in filteredPayPeriods
                         select new GetPayPeriodsForViewDto() { 
							PayPeriods = new PayPeriodsDto
							{
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                Name = o.Name,
                                IsActive = o.IsActive,
                                Id = o.Id
							}
						 });


            var payPeriodsListDtos = await query.ToListAsync();

            return _payPeriodsExcelExporter.ExportToFile(payPeriodsListDtos);
         }


    }
}
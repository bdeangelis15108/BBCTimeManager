using Nucleus.PayPeriod;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.PayperiodHistory.Exporting;
using Nucleus.PayperiodHistory.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.PayperiodHistory
{
	[AbpAuthorize(AppPermissions.Pages_PayperiodHistories)]
    public class PayperiodHistoriesAppService : NucleusAppServiceBase, IPayperiodHistoriesAppService
    {
		 private readonly IRepository<PayperiodHistories> _payperiodHistoriesRepository;
		 private readonly IPayperiodHistoriesExcelExporter _payperiodHistoriesExcelExporter;
		 private readonly IRepository<PayPeriods,int> _lookup_payPeriodsRepository;
		 

		  public PayperiodHistoriesAppService(IRepository<PayperiodHistories> payperiodHistoriesRepository, IPayperiodHistoriesExcelExporter payperiodHistoriesExcelExporter , IRepository<PayPeriods, int> lookup_payPeriodsRepository) 
		  {
			_payperiodHistoriesRepository = payperiodHistoriesRepository;
			_payperiodHistoriesExcelExporter = payperiodHistoriesExcelExporter;
			_lookup_payPeriodsRepository = lookup_payPeriodsRepository;
		
		  }

		 public async Task<PagedResultDto<GetPayperiodHistoriesForViewDto>> GetAll(GetAllPayperiodHistoriesInput input)
         {
			
			var filteredPayperiodHistories = _payperiodHistoriesRepository.GetAll()
						.Include( e => e.PayPeriodsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.period.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.periodFilter),  e => e.period == input.periodFilter)
						.WhereIf(input.activeFilter.HasValue && input.activeFilter > -1,  e => (input.activeFilter == 1 && e.active) || (input.activeFilter == 0 && !e.active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.PayPeriodsNameFilter), e => e.PayPeriodsFk != null && e.PayPeriodsFk.Name == input.PayPeriodsNameFilter);

			var pagedAndFilteredPayperiodHistories = filteredPayperiodHistories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var payperiodHistories = from o in pagedAndFilteredPayperiodHistories
                         join o1 in _lookup_payPeriodsRepository.GetAll() on o.PayPeriodsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPayperiodHistoriesForViewDto() {
							PayperiodHistories = new PayperiodHistoriesDto
							{
                                period = o.period,
                                active = o.active,
                                Id = o.Id,
                                PayPeriodsId=o.PayPeriodsId,
                            },
                         	PayPeriodsName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredPayperiodHistories.CountAsync();

            return new PagedResultDto<GetPayperiodHistoriesForViewDto>(
                totalCount,
                await payperiodHistories.ToListAsync()
            );
         }
		 
		 public async Task<GetPayperiodHistoriesForViewDto> GetPayperiodHistoriesForView(int id)
         {
            var payperiodHistories = await _payperiodHistoriesRepository.GetAsync(id);

            var output = new GetPayperiodHistoriesForViewDto { PayperiodHistories = ObjectMapper.Map<PayperiodHistoriesDto>(payperiodHistories) };

		    if (output.PayperiodHistories.PayPeriodsId != null)
            {
                var _lookupPayPeriods = await _lookup_payPeriodsRepository.FirstOrDefaultAsync((int)output.PayperiodHistories.PayPeriodsId);
                output.PayPeriodsName = _lookupPayPeriods?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PayperiodHistories_Edit)]
		 public async Task<GetPayperiodHistoriesForEditOutput> GetPayperiodHistoriesForEdit(EntityDto input)
         {
            var payperiodHistories = await _payperiodHistoriesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPayperiodHistoriesForEditOutput {PayperiodHistories = ObjectMapper.Map<CreateOrEditPayperiodHistoriesDto>(payperiodHistories)};

		    if (output.PayperiodHistories.PayPeriodsId != null)
            {
                var _lookupPayPeriods = await _lookup_payPeriodsRepository.FirstOrDefaultAsync((int)output.PayperiodHistories.PayPeriodsId);
                output.PayPeriodsName = _lookupPayPeriods?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPayperiodHistoriesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PayperiodHistories_Create)]
		 protected virtual async Task Create(CreateOrEditPayperiodHistoriesDto input)
         {
            var payperiodHistories = ObjectMapper.Map<PayperiodHistories>(input);

			

            await _payperiodHistoriesRepository.InsertAsync(payperiodHistories);
         }

		 [AbpAuthorize(AppPermissions.Pages_PayperiodHistories_Edit)]
		 protected virtual async Task Update(CreateOrEditPayperiodHistoriesDto input)
         {
            var payperiodHistories = await _payperiodHistoriesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, payperiodHistories);
         }

		 [AbpAuthorize(AppPermissions.Pages_PayperiodHistories_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _payperiodHistoriesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPayperiodHistoriesToExcel(GetAllPayperiodHistoriesForExcelInput input)
         {
			
			var filteredPayperiodHistories = _payperiodHistoriesRepository.GetAll()
						.Include( e => e.PayPeriodsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.period.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.periodFilter),  e => e.period == input.periodFilter)
						.WhereIf(input.activeFilter.HasValue && input.activeFilter > -1,  e => (input.activeFilter == 1 && e.active) || (input.activeFilter == 0 && !e.active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.PayPeriodsNameFilter), e => e.PayPeriodsFk != null && e.PayPeriodsFk.Name == input.PayPeriodsNameFilter);

			var query = (from o in filteredPayperiodHistories
                         join o1 in _lookup_payPeriodsRepository.GetAll() on o.PayPeriodsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPayperiodHistoriesForViewDto() { 
							PayperiodHistories = new PayperiodHistoriesDto
							{
                                period = o.period,
                                active = o.active,
                                Id = o.Id
							},
                         	PayPeriodsName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						 });


            var payperiodHistoriesListDtos = await query.ToListAsync();

            return _payperiodHistoriesExcelExporter.ExportToFile(payperiodHistoriesListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_PayperiodHistories)]
         public async Task<PagedResultDto<PayperiodHistoriesPayPeriodsLookupTableDto>> GetAllPayPeriodsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_payPeriodsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var payPeriodsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PayperiodHistoriesPayPeriodsLookupTableDto>();
			foreach(var payPeriods in payPeriodsList){
				lookupTableDtoList.Add(new PayperiodHistoriesPayPeriodsLookupTableDto
				{
					Id = payPeriods.Id,
					DisplayName = payPeriods.Name?.ToString()
				});
			}

            return new PagedResultDto<PayperiodHistoriesPayPeriodsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
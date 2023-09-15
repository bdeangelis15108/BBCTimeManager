

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.PayType.Exporting;
using Nucleus.PayType.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.PayType
{
	[AbpAuthorize(AppPermissions.Pages_PayTypeses)]
    public class PayTypesesAppService : NucleusAppServiceBase, IPayTypesesAppService
    {
		 private readonly IRepository<PayTypes> _payTypesRepository;
		 private readonly IPayTypesesExcelExporter _payTypesesExcelExporter;
		 

		  public PayTypesesAppService(IRepository<PayTypes> payTypesRepository, IPayTypesesExcelExporter payTypesesExcelExporter ) 
		  {
			_payTypesRepository = payTypesRepository;
			_payTypesesExcelExporter = payTypesesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPayTypesForViewDto>> GetAll(GetAllPayTypesesInput input)
         {
			
			var filteredPayTypeses = _payTypesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinMultiplierFilter != null, e => e.Multiplier >= input.MinMultiplierFilter)
						.WhereIf(input.MaxMultiplierFilter != null, e => e.Multiplier <= input.MaxMultiplierFilter);

			var pagedAndFilteredPayTypeses = filteredPayTypeses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var payTypeses = from o in pagedAndFilteredPayTypeses
                         select new GetPayTypesForViewDto() {
							PayTypes = new PayTypesDto
							{
                                Code = o.Code,
                                Description = o.Description,
                                Multiplier = o.Multiplier,
                                Section1 = o.Section1,
                                Section2 = o.Section2,
                                Section3 = o.Section3,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPayTypeses.CountAsync();

            return new PagedResultDto<GetPayTypesForViewDto>(
                totalCount,
                await payTypeses.ToListAsync()
            );
         }
		 
		 public async Task<GetPayTypesForViewDto> GetPayTypesForView(int id)
         {
            var payTypes = await _payTypesRepository.GetAsync(id);

            var output = new GetPayTypesForViewDto { PayTypes = ObjectMapper.Map<PayTypesDto>(payTypes) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PayTypeses_Edit)]
		 public async Task<GetPayTypesForEditOutput> GetPayTypesForEdit(EntityDto input)
         {
            var payTypes = await _payTypesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPayTypesForEditOutput {PayTypes = ObjectMapper.Map<CreateOrEditPayTypesDto>(payTypes)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPayTypesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PayTypeses_Create)]
		 protected virtual async Task Create(CreateOrEditPayTypesDto input)
         {
            var payTypes = ObjectMapper.Map<PayTypes>(input);

			

            await _payTypesRepository.InsertAsync(payTypes);
         }

		 [AbpAuthorize(AppPermissions.Pages_PayTypeses_Edit)]
		 protected virtual async Task Update(CreateOrEditPayTypesDto input)
         {
            var payTypes = await _payTypesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, payTypes);
         }

		 [AbpAuthorize(AppPermissions.Pages_PayTypeses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _payTypesRepository.DeleteAsync(input.Id);
         }

		public async Task<FileDto> GetPayTypesesToExcel(GetAllPayTypesesForExcelInput input)
		{

			var filteredPayTypeses = _payTypesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinMultiplierFilter != null, e => e.Multiplier >= input.MinMultiplierFilter)
						.WhereIf(input.MaxMultiplierFilter != null, e => e.Multiplier <= input.MaxMultiplierFilter);
			var query = (from o in filteredPayTypeses
                         select new GetPayTypesForViewDto() { 
							PayTypes = new PayTypesDto
							{
                                Code = o.Code,
                                Description = o.Description,
                                Multiplier = o.Multiplier,
                                Section1 = o.Section1,
                                Section2 = o.Section2,
                                Section3 = o.Section3,
                                Id = o.Id
							}
						 });


            var payTypesListDtos = await query.ToListAsync();

            return _payTypesesExcelExporter.ExportToFile(payTypesListDtos);
         }


    }
}
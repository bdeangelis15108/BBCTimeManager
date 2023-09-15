

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.CostType.Exporting;
using Nucleus.CostType.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.CostType
{
	[AbpAuthorize(AppPermissions.Pages_CostTypese)]
    public class CostTypeseAppService : NucleusAppServiceBase, ICostTypeseAppService
    {
		 private readonly IRepository<CostTypes> _costTypesRepository;
		 private readonly ICostTypeseExcelExporter _costTypeseExcelExporter;
		 

		  public CostTypeseAppService(IRepository<CostTypes> costTypesRepository, ICostTypeseExcelExporter costTypeseExcelExporter ) 
		  {
			_costTypesRepository = costTypesRepository;
			_costTypeseExcelExporter = costTypeseExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetCostTypesForViewDto>> GetAll(GetAllCostTypeseInput input)
         {
			
			var filteredCostTypese = _costTypesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter);

			var pagedAndFilteredCostTypese = filteredCostTypese
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var costTypese = from o in pagedAndFilteredCostTypese
                         select new GetCostTypesForViewDto() {
							CostTypes = new CostTypesDto
							{
                                Name = o.Name,
                                Code = o.Code,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCostTypese.CountAsync();

            return new PagedResultDto<GetCostTypesForViewDto>(
                totalCount,
                await costTypese.ToListAsync()
            );
         }
		 
		 public async Task<GetCostTypesForViewDto> GetCostTypesForView(int id)
         {
            var costTypes = await _costTypesRepository.GetAsync(id);

            var output = new GetCostTypesForViewDto { CostTypes = ObjectMapper.Map<CostTypesDto>(costTypes) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CostTypese_Edit)]
		 public async Task<GetCostTypesForEditOutput> GetCostTypesForEdit(EntityDto input)
         {
            var costTypes = await _costTypesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCostTypesForEditOutput {CostTypes = ObjectMapper.Map<CreateOrEditCostTypesDto>(costTypes)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCostTypesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CostTypese_Create)]
		 protected virtual async Task Create(CreateOrEditCostTypesDto input)
         {
            var costTypes = ObjectMapper.Map<CostTypes>(input);

			

            await _costTypesRepository.InsertAsync(costTypes);
         }

		 [AbpAuthorize(AppPermissions.Pages_CostTypese_Edit)]
		 protected virtual async Task Update(CreateOrEditCostTypesDto input)
         {
            var costTypes = await _costTypesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, costTypes);
         }

		 [AbpAuthorize(AppPermissions.Pages_CostTypese_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _costTypesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCostTypeseToExcel(GetAllCostTypeseForExcelInput input)
         {
			
			var filteredCostTypese = _costTypesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter);

			var query = (from o in filteredCostTypese
                         select new GetCostTypesForViewDto() { 
							CostTypes = new CostTypesDto
							{
                                Name = o.Name,
                                Code = o.Code,
                                Id = o.Id
							}
						 });


            var costTypesListDtos = await query.ToListAsync();

            return _costTypeseExcelExporter.ExportToFile(costTypesListDtos);
         }


    }
}
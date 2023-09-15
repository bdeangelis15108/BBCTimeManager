

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Resource.Exporting;
using Nucleus.Resource.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Resource
{
	[AbpAuthorize(AppPermissions.Pages_Resourceses)]
    public class ResourcesesAppService : NucleusAppServiceBase, IResourcesesAppService
    {
		 private readonly IRepository<Resources> _resourcesRepository;
		 private readonly IResourcesesExcelExporter _resourcesesExcelExporter;
		 

		  public ResourcesesAppService(IRepository<Resources> resourcesRepository, IResourcesesExcelExporter resourcesesExcelExporter ) 
		  {
			_resourcesRepository = resourcesRepository;
			_resourcesesExcelExporter = resourcesesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetResourcesForViewDto>> GetAll(GetAllResourcesesInput input)
         {
			
			var filteredResourceses = _resourcesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Type.Contains(input.Filter) || e.ResourceNumber.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter),  e => e.Type == input.TypeFilter)
						.WhereIf(input.MinCostPerHourFilter != null, e => e.CostPerHour >= input.MinCostPerHourFilter)
						.WhereIf(input.MaxCostPerHourFilter != null, e => e.CostPerHour <= input.MaxCostPerHourFilter)
						.WhereIf(input.MinCostPerUserFilter != null, e => e.CostPerUser >= input.MinCostPerUserFilter)
						.WhereIf(input.MaxCostPerUserFilter != null, e => e.CostPerUser <= input.MaxCostPerUserFilter)
						.WhereIf(input.MinCostPerDayFilter != null, e => e.CostPerDay >= input.MinCostPerDayFilter)
						.WhereIf(input.MaxCostPerDayFilter != null, e => e.CostPerDay <= input.MaxCostPerDayFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourceNumberFilter),  e => e.ResourceNumber == input.ResourceNumberFilter);

			var pagedAndFilteredResourceses = filteredResourceses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var resourceses = from o in pagedAndFilteredResourceses
                         select new GetResourcesForViewDto() {
							Resources = new ResourcesDto
							{
                                Name = o.Name,
                                Type = o.Type,
                                CostPerHour = o.CostPerHour,
                                CostPerUser = o.CostPerUser,
                                CostPerDay = o.CostPerDay,
                                ResourceNumber = o.ResourceNumber,
                                Id = o.Id
							}
						};

            var totalCount = await filteredResourceses.CountAsync();

            return new PagedResultDto<GetResourcesForViewDto>(
                totalCount,
                await resourceses.ToListAsync()
            );
         }
		 
		 public async Task<GetResourcesForViewDto> GetResourcesForView(int id)
         {
            var resources = await _resourcesRepository.GetAsync(id);

            var output = new GetResourcesForViewDto { Resources = ObjectMapper.Map<ResourcesDto>(resources) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Resourceses_Edit)]
		 public async Task<GetResourcesForEditOutput> GetResourcesForEdit(EntityDto input)
         {
            var resources = await _resourcesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetResourcesForEditOutput {Resources = ObjectMapper.Map<CreateOrEditResourcesDto>(resources)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditResourcesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Resourceses_Create)]
		 protected virtual async Task Create(CreateOrEditResourcesDto input)
         {
            var resources = ObjectMapper.Map<Resources>(input);

			

            await _resourcesRepository.InsertAsync(resources);
         }

		 [AbpAuthorize(AppPermissions.Pages_Resourceses_Edit)]
		 protected virtual async Task Update(CreateOrEditResourcesDto input)
         {
            var resources = await _resourcesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, resources);
         }

		 [AbpAuthorize(AppPermissions.Pages_Resourceses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _resourcesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetResourcesesToExcel(GetAllResourcesesForExcelInput input)
         {
			
			var filteredResourceses = _resourcesRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Type.Contains(input.Filter) || e.ResourceNumber.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter),  e => e.Type == input.TypeFilter)
						.WhereIf(input.MinCostPerHourFilter != null, e => e.CostPerHour >= input.MinCostPerHourFilter)
						.WhereIf(input.MaxCostPerHourFilter != null, e => e.CostPerHour <= input.MaxCostPerHourFilter)
						.WhereIf(input.MinCostPerUserFilter != null, e => e.CostPerUser >= input.MinCostPerUserFilter)
						.WhereIf(input.MaxCostPerUserFilter != null, e => e.CostPerUser <= input.MaxCostPerUserFilter)
						.WhereIf(input.MinCostPerDayFilter != null, e => e.CostPerDay >= input.MinCostPerDayFilter)
						.WhereIf(input.MaxCostPerDayFilter != null, e => e.CostPerDay <= input.MaxCostPerDayFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourceNumberFilter),  e => e.ResourceNumber == input.ResourceNumberFilter);

			var query = (from o in filteredResourceses
                         select new GetResourcesForViewDto() { 
							Resources = new ResourcesDto
							{
                                Name = o.Name,
                                Type = o.Type,
                                CostPerHour = o.CostPerHour,
                                CostPerUser = o.CostPerUser,
                                CostPerDay = o.CostPerDay,
                                ResourceNumber = o.ResourceNumber,
                                Id = o.Id
							}
						 });


            var resourcesListDtos = await query.ToListAsync();

            return _resourcesesExcelExporter.ExportToFile(resourcesListDtos);
         }

		
	}
}
using Nucleus.WorkerClasee;
using Nucleus.Resource;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.ResourceWorkerInfo.Exporting;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.ResourceWorkerInfo
{
	[AbpAuthorize(AppPermissions.Pages_ResourceWorkerInfoses)]
    public class ResourceWorkerInfosesAppService : NucleusAppServiceBase, IResourceWorkerInfosesAppService
    {
		 private readonly IRepository<ResourceWorkerInfos> _resourceWorkerInfosRepository;
		 private readonly IResourceWorkerInfosesExcelExporter _resourceWorkerInfosesExcelExporter;
		 private readonly IRepository<WorkerClasees,int> _lookup_workerClaseesRepository;
		 private readonly IRepository<Resources,int> _lookup_resourcesRepository;
		 

		  public ResourceWorkerInfosesAppService(IRepository<ResourceWorkerInfos> resourceWorkerInfosRepository, IResourceWorkerInfosesExcelExporter resourceWorkerInfosesExcelExporter , IRepository<WorkerClasees, int> lookup_workerClaseesRepository, IRepository<Resources, int> lookup_resourcesRepository) 
		  {
			_resourceWorkerInfosRepository = resourceWorkerInfosRepository;
			_resourceWorkerInfosesExcelExporter = resourceWorkerInfosesExcelExporter;
			_lookup_workerClaseesRepository = lookup_workerClaseesRepository;
		_lookup_resourcesRepository = lookup_resourcesRepository;
		
		  }

		 public async Task<PagedResultDto<GetResourceWorkerInfosForViewDto>> GetAll(GetAllResourceWorkerInfosesInput input)
         {
			
			var filteredResourceWorkerInfoses = _resourceWorkerInfosRepository.GetAll()
                        .Include(e => e.WorkerClaseesFk)
                        .Include(e => e.ResourcesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.UnionNumber.Contains(input.Filter) || e.UnionLocal.Contains(input.Filter) || e.Wcomp1.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.ToLower() == input.FirstNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.ToLower() == input.LastNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnionNumberFilter), e => e.UnionNumber.ToLower() == input.UnionNumberFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnionLocalFilter), e => e.UnionLocal.ToLower() == input.UnionLocalFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Wcomp1Filter), e => e.Wcomp1.ToLower() == input.Wcomp1Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WorkerClaseesNameFilter), e => e.WorkerClaseesFk != null && e.WorkerClaseesFk.Name.ToLower() == input.WorkerClaseesNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name.ToLower() == input.ResourcesNameFilter.ToLower().Trim());
                        
                
            var pagedAndFilteredResourceWorkerInfoses = filteredResourceWorkerInfoses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var resourceWorkerInfoses = from o in pagedAndFilteredResourceWorkerInfoses
                         join o1 in _lookup_workerClaseesRepository.GetAll() on o.WorkerClaseesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetResourceWorkerInfosForViewDto() {
							ResourceWorkerInfos = new ResourceWorkerInfosDto
							{
                                FirstName = o.FirstName,
                                LastName = o.LastName,
                                UnionNumber = o.UnionNumber,
                                UnionLocal = o.UnionLocal,
                                Wcomp1 = o.Wcomp1,
                                RefNumber = o.ResourcesFk.ResourceNumber,
                                ResourcesId = o.ResourcesId,
                                Id = o.Id
							},
                             WorkerClaseesName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ResourcesName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         };

            var totalCount = await filteredResourceWorkerInfoses.CountAsync();

            return new PagedResultDto<GetResourceWorkerInfosForViewDto>(
                totalCount,
                await resourceWorkerInfoses.ToListAsync()
            );
         }
		 
		 public async Task<GetResourceWorkerInfosForViewDto> GetResourceWorkerInfosForView(int id)
         {
            var resourceWorkerInfos = await _resourceWorkerInfosRepository.GetAsync(id);

            var output = new GetResourceWorkerInfosForViewDto { ResourceWorkerInfos = ObjectMapper.Map<ResourceWorkerInfosDto>(resourceWorkerInfos) };

		    if (output.ResourceWorkerInfos.WorkerClaseesId != null)
            {
                var _lookupWorkerClasees = await _lookup_workerClaseesRepository.FirstOrDefaultAsync((int)output.ResourceWorkerInfos.WorkerClaseesId);
                output.WorkerClaseesName = _lookupWorkerClasees?.Name?.ToString();
            }

		    if (output.ResourceWorkerInfos.ResourcesId != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.ResourceWorkerInfos.ResourcesId);
                output.ResourcesName = _lookupResources?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ResourceWorkerInfoses_Edit)]
		 public async Task<GetResourceWorkerInfosForEditOutput> GetResourceWorkerInfosForEdit(EntityDto input)
         {
            var resourceWorkerInfos = await _resourceWorkerInfosRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetResourceWorkerInfosForEditOutput {ResourceWorkerInfos = ObjectMapper.Map<CreateOrEditResourceWorkerInfosDto>(resourceWorkerInfos)};

		    if (output.ResourceWorkerInfos.WorkerClaseesId != null)
            {
                var _lookupWorkerClasees = await _lookup_workerClaseesRepository.FirstOrDefaultAsync((int)output.ResourceWorkerInfos.WorkerClaseesId);
                output.WorkerClaseesName = _lookupWorkerClasees?.Name?.ToString();
            }

		    if (output.ResourceWorkerInfos.ResourcesId != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.ResourceWorkerInfos.ResourcesId);
                output.ResourcesName = _lookupResources?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditResourceWorkerInfosDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ResourceWorkerInfoses_Create)]
		 private async Task Create(CreateOrEditResourceWorkerInfosDto input)
         {
            var resourceWorkerInfos = ObjectMapper.Map<ResourceWorkerInfos>(input);

			

            await _resourceWorkerInfosRepository.InsertAsync(resourceWorkerInfos);
         }

		 [AbpAuthorize(AppPermissions.Pages_ResourceWorkerInfoses_Edit)]
		 private async Task Update(CreateOrEditResourceWorkerInfosDto input)
         {
            var resourceWorkerInfos = await _resourceWorkerInfosRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, resourceWorkerInfos);
         }

		 [AbpAuthorize(AppPermissions.Pages_ResourceWorkerInfoses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _resourceWorkerInfosRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetResourceWorkerInfosesToExcel(GetAllResourceWorkerInfosesForExcelInput input)
         {
			
			var filteredResourceWorkerInfoses = _resourceWorkerInfosRepository.GetAll()
						.Include( e => e.WorkerClaseesFk)
						.Include( e => e.ResourcesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.UnionNumber.Contains(input.Filter) || e.UnionLocal.Contains(input.Filter) || e.Wcomp1.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter),  e => e.FirstName.ToLower() == input.FirstNameFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter),  e => e.LastName.ToLower() == input.LastNameFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnionNumberFilter),  e => e.UnionNumber.ToLower() == input.UnionNumberFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnionLocalFilter),  e => e.UnionLocal.ToLower() == input.UnionLocalFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.Wcomp1Filter),  e => e.Wcomp1.ToLower() == input.Wcomp1Filter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.WorkerClaseesNameFilter), e => e.WorkerClaseesFk != null && e.WorkerClaseesFk.Name.ToLower() == input.WorkerClaseesNameFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name.ToLower() == input.ResourcesNameFilter.ToLower().Trim());

			var query = (from o in filteredResourceWorkerInfoses
                         join o1 in _lookup_workerClaseesRepository.GetAll() on o.WorkerClaseesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetResourceWorkerInfosForViewDto() { 
							ResourceWorkerInfos = new ResourceWorkerInfosDto
							{
                                FirstName = o.FirstName,
                                LastName = o.LastName,
                                UnionNumber = o.UnionNumber,
                                UnionLocal = o.UnionLocal,
                                Wcomp1 = o.Wcomp1,

                                Id = o.Id,
                                
							},
                             WorkerClaseesName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ResourcesName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });


            var resourceWorkerInfosListDtos = await query.ToListAsync();

            return _resourceWorkerInfosesExcelExporter.ExportToFile(resourceWorkerInfosListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ResourceWorkerInfoses)]
         public async Task<PagedResultDto<ResourceWorkerInfosWorkerClaseesLookupTableDto>> GetAllWorkerClaseesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_workerClaseesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var workerClaseesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ResourceWorkerInfosWorkerClaseesLookupTableDto>();
			foreach(var workerClasees in workerClaseesList){
				lookupTableDtoList.Add(new ResourceWorkerInfosWorkerClaseesLookupTableDto
				{
					Id = workerClasees.Id,
					DisplayName = workerClasees.Name?.ToString()
				});
			}

            return new PagedResultDto<ResourceWorkerInfosWorkerClaseesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ResourceWorkerInfoses)]
         public async Task<PagedResultDto<ResourceWorkerInfosResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_resourcesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var resourcesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ResourceWorkerInfosResourcesLookupTableDto>();
			foreach(var resources in resourcesList){
				lookupTableDtoList.Add(new ResourceWorkerInfosResourcesLookupTableDto
				{
					Id = resources.Id,
					DisplayName = resources.Name?.ToString()
				});
			}

            return new PagedResultDto<ResourceWorkerInfosResourcesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
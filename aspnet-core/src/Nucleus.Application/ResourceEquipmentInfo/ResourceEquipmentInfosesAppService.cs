

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.ResourceEquipmentInfo.Exporting;
using Nucleus.ResourceEquipmentInfo.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.ResourceEquipmentInfo
{
	[AbpAuthorize(AppPermissions.Pages_ResourceEquipmentInfoses)]
    public class ResourceEquipmentInfosesAppService : NucleusAppServiceBase, IResourceEquipmentInfosesAppService
    {
		 private readonly IRepository<ResourceEquipmentInfos> _resourceEquipmentInfosRepository;
		 private readonly IResourceEquipmentInfosesExcelExporter _resourceEquipmentInfosesExcelExporter;
		 

		  public ResourceEquipmentInfosesAppService(IRepository<ResourceEquipmentInfos> resourceEquipmentInfosRepository, IResourceEquipmentInfosesExcelExporter resourceEquipmentInfosesExcelExporter ) 
		  {
			_resourceEquipmentInfosRepository = resourceEquipmentInfosRepository;
			_resourceEquipmentInfosesExcelExporter = resourceEquipmentInfosesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetResourceEquipmentInfosForViewDto>> GetAll(GetAllResourceEquipmentInfosesInput input)
         {
			
			var filteredResourceEquipmentInfoses = _resourceEquipmentInfosRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var pagedAndFilteredResourceEquipmentInfoses = filteredResourceEquipmentInfoses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var resourceEquipmentInfoses = from o in pagedAndFilteredResourceEquipmentInfoses
                         select new GetResourceEquipmentInfosForViewDto() {
							ResourceEquipmentInfos = new ResourceEquipmentInfosDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredResourceEquipmentInfoses.CountAsync();

            return new PagedResultDto<GetResourceEquipmentInfosForViewDto>(
                totalCount,
                await resourceEquipmentInfoses.ToListAsync()
            );
         }
		 
		 public async Task<GetResourceEquipmentInfosForViewDto> GetResourceEquipmentInfosForView(int id)
         {
            var resourceEquipmentInfos = await _resourceEquipmentInfosRepository.GetAsync(id);

            var output = new GetResourceEquipmentInfosForViewDto { ResourceEquipmentInfos = ObjectMapper.Map<ResourceEquipmentInfosDto>(resourceEquipmentInfos) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ResourceEquipmentInfoses_Edit)]
		 public async Task<GetResourceEquipmentInfosForEditOutput> GetResourceEquipmentInfosForEdit(EntityDto input)
         {
            var resourceEquipmentInfos = await _resourceEquipmentInfosRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetResourceEquipmentInfosForEditOutput {ResourceEquipmentInfos = ObjectMapper.Map<CreateOrEditResourceEquipmentInfosDto>(resourceEquipmentInfos)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditResourceEquipmentInfosDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ResourceEquipmentInfoses_Create)]
		 protected virtual async Task Create(CreateOrEditResourceEquipmentInfosDto input)
         {
            var resourceEquipmentInfos = ObjectMapper.Map<ResourceEquipmentInfos>(input);

			

            await _resourceEquipmentInfosRepository.InsertAsync(resourceEquipmentInfos);
         }

		 [AbpAuthorize(AppPermissions.Pages_ResourceEquipmentInfoses_Edit)]
		 protected virtual async Task Update(CreateOrEditResourceEquipmentInfosDto input)
         {
            var resourceEquipmentInfos = await _resourceEquipmentInfosRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, resourceEquipmentInfos);
         }

		 [AbpAuthorize(AppPermissions.Pages_ResourceEquipmentInfoses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _resourceEquipmentInfosRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetResourceEquipmentInfosesToExcel(GetAllResourceEquipmentInfosesForExcelInput input)
         {
			
			var filteredResourceEquipmentInfoses = _resourceEquipmentInfosRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredResourceEquipmentInfoses
                         select new GetResourceEquipmentInfosForViewDto() { 
							ResourceEquipmentInfos = new ResourceEquipmentInfosDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var resourceEquipmentInfosListDtos = await query.ToListAsync();

            return _resourceEquipmentInfosesExcelExporter.ExportToFile(resourceEquipmentInfosListDtos);
         }


    }
}
using Nucleus.Union;
using Nucleus.Resource;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.EmployeeUnion.Exporting;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.EmployeeUnion
{
	[AbpAuthorize(AppPermissions.Pages_EmployeeUnions)]
    public class EmployeeUnionsAppService : NucleusAppServiceBase, IEmployeeUnionsAppService
    {
		 private readonly IRepository<EmployeeUnions> _employeeUnionsRepository;
		 private readonly IEmployeeUnionsExcelExporter _employeeUnionsExcelExporter;
		 private readonly IRepository<Unions,int> _lookup_unionsRepository;
		 private readonly IRepository<Resources,int> _lookup_resourcesRepository;
		 

		  public EmployeeUnionsAppService(IRepository<EmployeeUnions> employeeUnionsRepository, IEmployeeUnionsExcelExporter employeeUnionsExcelExporter , IRepository<Unions, int> lookup_unionsRepository, IRepository<Resources, int> lookup_resourcesRepository) 
		  {
			_employeeUnionsRepository = employeeUnionsRepository;
			_employeeUnionsExcelExporter = employeeUnionsExcelExporter;
			_lookup_unionsRepository = lookup_unionsRepository;
		_lookup_resourcesRepository = lookup_resourcesRepository;
		
		  }

		 public async Task<PagedResultDto<GetEmployeeUnionsForViewDto>> GetAll(GetAllEmployeeUnionsInput input)
         {
			
			var filteredEmployeeUnions = _employeeUnionsRepository.GetAll()
						.Include( e => e.UnionsFk)
						.Include( e => e.ResourcesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.LocalNumber.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocalNumberFilter),  e => e.LocalNumber == input.LocalNumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnionsNumberFilter), e => e.UnionsFk != null && e.UnionsFk.Number == input.UnionsNumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name == input.ResourcesNameFilter);

			var pagedAndFilteredEmployeeUnions = filteredEmployeeUnions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var employeeUnions = from o in pagedAndFilteredEmployeeUnions
                         join o1 in _lookup_unionsRepository.GetAll() on o.UnionsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeeUnionsForViewDto() {
							EmployeeUnions = new EmployeeUnionsDto
							{
                                LocalNumber = o.LocalNumber,
                                Id = o.Id,
                                UnionsId=o.UnionsId
							},
                         	UnionsNumber = s1 == null || s1.Number == null ? "" : s1.Number.ToString(),
                         	ResourcesName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredEmployeeUnions.CountAsync();

            return new PagedResultDto<GetEmployeeUnionsForViewDto>(
                totalCount,
                await employeeUnions.ToListAsync()
            );
         }
		 
		 public async Task<GetEmployeeUnionsForViewDto> GetEmployeeUnionsForView(int id)
         {
            var employeeUnions = await _employeeUnionsRepository.GetAsync(id);

            var output = new GetEmployeeUnionsForViewDto { EmployeeUnions = ObjectMapper.Map<EmployeeUnionsDto>(employeeUnions) };

		    if (output.EmployeeUnions.UnionsId != null)
            {
                var _lookupUnions = await _lookup_unionsRepository.FirstOrDefaultAsync((int)output.EmployeeUnions.UnionsId);
                output.UnionsNumber = _lookupUnions?.Number?.ToString();
            }

		    if (output.EmployeeUnions.ResourcesId != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.EmployeeUnions.ResourcesId);
                output.ResourcesName = _lookupResources?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_EmployeeUnions_Edit)]
		 public async Task<GetEmployeeUnionsForEditOutput> GetEmployeeUnionsForEdit(EntityDto input)
         {
            var employeeUnions = await _employeeUnionsRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmployeeUnionsForEditOutput {EmployeeUnions = ObjectMapper.Map<CreateOrEditEmployeeUnionsDto>(employeeUnions)};

		    if (output.EmployeeUnions.UnionsId != null)
            {
                var _lookupUnions = await _lookup_unionsRepository.FirstOrDefaultAsync((int)output.EmployeeUnions.UnionsId);
                output.UnionsNumber = _lookupUnions?.Number?.ToString();
            }

		    if (output.EmployeeUnions.ResourcesId != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.EmployeeUnions.ResourcesId);
                output.ResourcesName = _lookupResources?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEmployeeUnionsDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeUnions_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeeUnionsDto input)
         {
            var employeeUnions = ObjectMapper.Map<EmployeeUnions>(input);

			

            await _employeeUnionsRepository.InsertAsync(employeeUnions);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeUnions_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeeUnionsDto input)
         {
            var employeeUnions = await _employeeUnionsRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, employeeUnions);
         }

		 [AbpAuthorize(AppPermissions.Pages_EmployeeUnions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeeUnionsRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeeUnionsToExcel(GetAllEmployeeUnionsForExcelInput input)
         {
			
			var filteredEmployeeUnions = _employeeUnionsRepository.GetAll()
						.Include( e => e.UnionsFk)
						.Include( e => e.ResourcesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.LocalNumber.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocalNumberFilter),  e => e.LocalNumber == input.LocalNumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnionsNumberFilter), e => e.UnionsFk != null && e.UnionsFk.Number == input.UnionsNumberFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name == input.ResourcesNameFilter);

			var query = (from o in filteredEmployeeUnions
                         join o1 in _lookup_unionsRepository.GetAll() on o.UnionsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetEmployeeUnionsForViewDto() { 
							EmployeeUnions = new EmployeeUnionsDto
							{
                                LocalNumber = o.LocalNumber,
                                Id = o.Id
							},
                         	UnionsNumber = s1 == null || s1.Number == null ? "" : s1.Number.ToString(),
                         	ResourcesName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var employeeUnionsListDtos = await query.ToListAsync();

            return _employeeUnionsExcelExporter.ExportToFile(employeeUnionsListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_EmployeeUnions)]
         public async Task<PagedResultDto<EmployeeUnionsUnionsLookupTableDto>> GetAllUnionsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_unionsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Number != null && e.Number.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var unionsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeUnionsUnionsLookupTableDto>();
			foreach(var unions in unionsList){
				lookupTableDtoList.Add(new EmployeeUnionsUnionsLookupTableDto
				{
					Id = unions.Id,
					DisplayName = unions.Number?.ToString()
				});
			}

            return new PagedResultDto<EmployeeUnionsUnionsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_EmployeeUnions)]
         public async Task<PagedResultDto<EmployeeUnionsResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_resourcesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var resourcesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<EmployeeUnionsResourcesLookupTableDto>();
			foreach(var resources in resourcesList){
				lookupTableDtoList.Add(new EmployeeUnionsResourcesLookupTableDto
				{
					Id = resources.Id,
					DisplayName = resources.Name?.ToString()
				});
			}

            return new PagedResultDto<EmployeeUnionsResourcesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
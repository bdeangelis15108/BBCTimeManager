using Nucleus.Union;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.UnionPayRate.Exporting;
using Nucleus.UnionPayRate.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.UnionPayRate
{
	[AbpAuthorize(AppPermissions.Pages_UnionPayRates)]
    public class UnionPayRatesAppService : NucleusAppServiceBase, IUnionPayRatesAppService
    {
		 private readonly IRepository<UnionPayRates> _unionPayRatesRepository;
		 private readonly IUnionPayRatesExcelExporter _unionPayRatesExcelExporter;
		 private readonly IRepository<Unions,int> _lookup_unionsRepository;
		 

		  public UnionPayRatesAppService(IRepository<UnionPayRates> unionPayRatesRepository, IUnionPayRatesExcelExporter unionPayRatesExcelExporter , IRepository<Unions, int> lookup_unionsRepository) 
		  {
			_unionPayRatesRepository = unionPayRatesRepository;
			_unionPayRatesExcelExporter = unionPayRatesExcelExporter;
			_lookup_unionsRepository = lookup_unionsRepository;
		
		  }

		 public async Task<PagedResultDto<GetUnionPayRatesForViewDto>> GetAll(GetAllUnionPayRatesInput input)
         {
			
			var filteredUnionPayRates = _unionPayRatesRepository.GetAll()
						.Include( e => e.UnionsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Class.Contains(input.Filter) || e.Dedtype.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ClassFilter),  e => e.Class == input.ClassFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DedtypeFilter),  e => e.Dedtype == input.DedtypeFilter)
						.WhereIf(input.MinPerhourFilter != null, e => e.Perhour >= input.MinPerhourFilter)
						.WhereIf(input.MaxPerhourFilter != null, e => e.Perhour <= input.MaxPerhourFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnionsNumberFilter), e => e.UnionsFk != null && e.UnionsFk.Number == input.UnionsNumberFilter);

			var pagedAndFilteredUnionPayRates = filteredUnionPayRates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var unionPayRates = from o in pagedAndFilteredUnionPayRates
                         join o1 in _lookup_unionsRepository.GetAll() on o.UnionsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetUnionPayRatesForViewDto() {
							UnionPayRates = new UnionPayRatesDto
							{
                                Class = o.Class,
                                Dedtype = o.Dedtype,
                                Perhour = o.Perhour,
                                Id = o.Id
							},
                         	UnionsNumber = s1 == null || s1.Number == null ? "" : s1.Number.ToString()
						};

            var totalCount = await filteredUnionPayRates.CountAsync();

            return new PagedResultDto<GetUnionPayRatesForViewDto>(
                totalCount,
                await unionPayRates.ToListAsync()
            );
         }
		 
		 public async Task<GetUnionPayRatesForViewDto> GetUnionPayRatesForView(int id)
         {
            var unionPayRates = await _unionPayRatesRepository.GetAsync(id);

            var output = new GetUnionPayRatesForViewDto { UnionPayRates = ObjectMapper.Map<UnionPayRatesDto>(unionPayRates) };

		    if (output.UnionPayRates.UnionsId != null)
            {
                var _lookupUnions = await _lookup_unionsRepository.FirstOrDefaultAsync((int)output.UnionPayRates.UnionsId);
                output.UnionsNumber = _lookupUnions?.Number?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_UnionPayRates_Edit)]
		 public async Task<GetUnionPayRatesForEditOutput> GetUnionPayRatesForEdit(EntityDto input)
         {
            var unionPayRates = await _unionPayRatesRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUnionPayRatesForEditOutput {UnionPayRates = ObjectMapper.Map<CreateOrEditUnionPayRatesDto>(unionPayRates)};

		    if (output.UnionPayRates.UnionsId != null)
            {
                var _lookupUnions = await _lookup_unionsRepository.FirstOrDefaultAsync((int)output.UnionPayRates.UnionsId);
                output.UnionsNumber = _lookupUnions?.Number?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUnionPayRatesDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_UnionPayRates_Create)]
		 protected virtual async Task Create(CreateOrEditUnionPayRatesDto input)
         {
            var unionPayRates = ObjectMapper.Map<UnionPayRates>(input);

			

            await _unionPayRatesRepository.InsertAsync(unionPayRates);
         }

		 [AbpAuthorize(AppPermissions.Pages_UnionPayRates_Edit)]
		 protected virtual async Task Update(CreateOrEditUnionPayRatesDto input)
         {
            var unionPayRates = await _unionPayRatesRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, unionPayRates);
         }

		 [AbpAuthorize(AppPermissions.Pages_UnionPayRates_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _unionPayRatesRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUnionPayRatesToExcel(GetAllUnionPayRatesForExcelInput input)
         {
			
			var filteredUnionPayRates = _unionPayRatesRepository.GetAll()
						.Include( e => e.UnionsFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Class.Contains(input.Filter) || e.Dedtype.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ClassFilter),  e => e.Class == input.ClassFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DedtypeFilter),  e => e.Dedtype == input.DedtypeFilter)
						.WhereIf(input.MinPerhourFilter != null, e => e.Perhour >= input.MinPerhourFilter)
						.WhereIf(input.MaxPerhourFilter != null, e => e.Perhour <= input.MaxPerhourFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UnionsNumberFilter), e => e.UnionsFk != null && e.UnionsFk.Number == input.UnionsNumberFilter);

			var query = (from o in filteredUnionPayRates
                         join o1 in _lookup_unionsRepository.GetAll() on o.UnionsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetUnionPayRatesForViewDto() { 
							UnionPayRates = new UnionPayRatesDto
							{
                                Class = o.Class,
                                Dedtype = o.Dedtype,
                                Perhour = o.Perhour,
                                Id = o.Id
							},
                         	UnionsNumber = s1 == null || s1.Number == null ? "" : s1.Number.ToString()
						 });


            var unionPayRatesListDtos = await query.ToListAsync();

            return _unionPayRatesExcelExporter.ExportToFile(unionPayRatesListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_UnionPayRates)]
         public async Task<PagedResultDto<UnionPayRatesUnionsLookupTableDto>> GetAllUnionsForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_unionsRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Number != null && e.Number.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var unionsList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UnionPayRatesUnionsLookupTableDto>();
			foreach(var unions in unionsList){
				lookupTableDtoList.Add(new UnionPayRatesUnionsLookupTableDto
				{
					Id = unions.Id,
					DisplayName = unions.Number?.ToString()
				});
			}

            return new PagedResultDto<UnionPayRatesUnionsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
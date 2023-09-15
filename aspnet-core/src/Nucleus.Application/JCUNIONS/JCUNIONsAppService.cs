using Nucleus.JCCAT;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.JCUNIONS.Exporting;
using Nucleus.JCUNIONS.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.JCUNIONS
{
	[AbpAuthorize(AppPermissions.Pages_JCUNIONs)]
    public class JCUNIONsAppService : NucleusAppServiceBase, IJCUNIONsAppService
    {
		 private readonly IRepository<JCUNION> _jcunionRepository;
		 private readonly IJCUNIONsExcelExporter _jcunioNsExcelExporter;
		 private readonly IRepository<JACCAT,int> _lookup_jaccatRepository;
		 

		  public JCUNIONsAppService(IRepository<JCUNION> jcunionRepository, IJCUNIONsExcelExporter jcunioNsExcelExporter , IRepository<JACCAT, int> lookup_jaccatRepository) 
		  {
			_jcunionRepository = jcunionRepository;
			_jcunioNsExcelExporter = jcunioNsExcelExporter;
			_lookup_jaccatRepository = lookup_jaccatRepository;
		
		  }

		 public async Task<PagedResultDto<GetJCUNIONForViewDto>> GetAll(GetAllJCUNIONsInput input)
         {
			
			var filteredJCUNIONs = _jcunionRepository.GetAll()
						.Include( e => e.JOBNFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.UNIONNUM.Contains(input.Filter) || e.UNIONLOCAL.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONNUMFilter),  e => e.UNIONNUM == input.UNIONNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONLOCALFilter),  e => e.UNIONLOCAL == input.UNIONLOCALFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JACCATJOBNUMFilter), e => e.JOBNFk != null && e.JOBNFk.JOBNUM == input.JACCATJOBNUMFilter);

			var pagedAndFilteredJCUNIONs = filteredJCUNIONs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var jcunioNs = from o in pagedAndFilteredJCUNIONs
                         join o1 in _lookup_jaccatRepository.GetAll() on o.JOBNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetJCUNIONForViewDto() {
							JCUNION = new JCUNIONDto
							{
                                UNIONNUM = o.UNIONNUM,
                                UNIONLOCAL = o.UNIONLOCAL,
                                Id = o.Id
							},
                         	JACCATJOBNUM = s1 == null ? "" : s1.JOBNUM.ToString()
						};

            var totalCount = await filteredJCUNIONs.CountAsync();

            return new PagedResultDto<GetJCUNIONForViewDto>(
                totalCount,
                await jcunioNs.ToListAsync()
            );
         }
		 
		 public async Task<GetJCUNIONForViewDto> GetJCUNIONForView(int id)
         {
            var jcunion = await _jcunionRepository.GetAsync(id);

            var output = new GetJCUNIONForViewDto { JCUNION = ObjectMapper.Map<JCUNIONDto>(jcunion) };

		    if (output.JCUNION.JOBNUM != null)
            {
                var _lookupJACCAT = await _lookup_jaccatRepository.FirstOrDefaultAsync((int)output.JCUNION.JOBNUM);
                output.JACCATJOBNUM = _lookupJACCAT.JOBNUM.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_JCUNIONs_Edit)]
		 public async Task<GetJCUNIONForEditOutput> GetJCUNIONForEdit(EntityDto input)
         {
            var jcunion = await _jcunionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetJCUNIONForEditOutput {JCUNION = ObjectMapper.Map<CreateOrEditJCUNIONDto>(jcunion)};

		    if (output.JCUNION.JOBNUM != null)
            {
                var _lookupJACCAT = await _lookup_jaccatRepository.FirstOrDefaultAsync((int)output.JCUNION.JOBNUM);
                output.JACCATJOBNUM = _lookupJACCAT.JOBNUM.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditJCUNIONDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_JCUNIONs_Create)]
		 protected virtual async Task Create(CreateOrEditJCUNIONDto input)
         {
            var jcunion = ObjectMapper.Map<JCUNION>(input);

			
			if (AbpSession.TenantId != null)
			{
				jcunion.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _jcunionRepository.InsertAsync(jcunion);
         }

		 [AbpAuthorize(AppPermissions.Pages_JCUNIONs_Edit)]
		 protected virtual async Task Update(CreateOrEditJCUNIONDto input)
         {
            var jcunion = await _jcunionRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, jcunion);
         }

		 [AbpAuthorize(AppPermissions.Pages_JCUNIONs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _jcunionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetJCUNIONsToExcel(GetAllJCUNIONsForExcelInput input)
         {
			
			var filteredJCUNIONs = _jcunionRepository.GetAll()
						.Include( e => e.JOBNFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.UNIONNUM.Contains(input.Filter) || e.UNIONLOCAL.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONNUMFilter),  e => e.UNIONNUM == input.UNIONNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONLOCALFilter),  e => e.UNIONLOCAL == input.UNIONLOCALFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JACCATJOBNUMFilter), e => e.JOBNFk != null && e.JOBNFk.JOBNUM == input.JACCATJOBNUMFilter);

			var query = (from o in filteredJCUNIONs
                         join o1 in _lookup_jaccatRepository.GetAll() on o.JOBNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetJCUNIONForViewDto() { 
							JCUNION = new JCUNIONDto
							{
                                UNIONNUM = o.UNIONNUM,
                                UNIONLOCAL = o.UNIONLOCAL,
                                Id = o.Id
							},
                         	JACCATJOBNUM = s1 == null ? "" : s1.JOBNUM.ToString()
						 });


            var jcunionListDtos = await query.ToListAsync();

            return _jcunioNsExcelExporter.ExportToFile(jcunionListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_JCUNIONs)]
         public async Task<PagedResultDto<JCUNIONJACCATLookupTableDto>> GetAllJACCATForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_jaccatRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.JOBNUM != null ? e.JOBNUM.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var jaccatList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<JCUNIONJACCATLookupTableDto>();
			foreach(var jaccat in jaccatList){
				lookupTableDtoList.Add(new JCUNIONJACCATLookupTableDto
				{
					Id = jaccat.Id,
					DisplayName = jaccat.JOBNUM?.ToString()
				});
			}

            return new PagedResultDto<JCUNIONJACCATLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
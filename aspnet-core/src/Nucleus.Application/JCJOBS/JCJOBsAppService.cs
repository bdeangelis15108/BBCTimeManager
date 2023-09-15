using Nucleus.JCCAT;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.JCJOBS.Exporting;
using Nucleus.JCJOBS.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.JCJOBS
{
	[AbpAuthorize(AppPermissions.Pages_JCJOBs)]
    public class JCJOBsAppService : NucleusAppServiceBase, IJCJOBsAppService
    {
		 private readonly IRepository<JCJOB> _jcjobRepository;
		 private readonly IJCJOBsExcelExporter _jcjoBsExcelExporter;
		 private readonly IRepository<JACCAT,int> _lookup_jaccatRepository;
		 

		  public JCJOBsAppService(IRepository<JCJOB> jcjobRepository, IJCJOBsExcelExporter jcjoBsExcelExporter , IRepository<JACCAT, int> lookup_jaccatRepository) 
		  {
			_jcjobRepository = jcjobRepository;
			_jcjoBsExcelExporter = jcjoBsExcelExporter;
			_lookup_jaccatRepository = lookup_jaccatRepository;
		
		  }

		 public async Task<PagedResultDto<GetJCJOBForViewDto>> GetAll(GetAllJCJOBsInput input)
         {
			
			var filteredJCJOBs = _jcjobRepository.GetAll()
						.Include( e => e.JOBNFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.STATE.Contains(input.Filter) || e.LOCALITY.Contains(input.Filter) || e.CLASS.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.STATEFilter),  e => e.STATE == input.STATEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LOCALITYFilter),  e => e.LOCALITY == input.LOCALITYFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CLASSFilter),  e => e.CLASS == input.CLASSFilter)
						.WhereIf(input.MinCLOSEDFilter != null, e => e.CLOSED >= input.MinCLOSEDFilter)
						.WhereIf(input.MaxCLOSEDFilter != null, e => e.CLOSED <= input.MaxCLOSEDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JACCATJOBNUMFilter), e => e.JOBNFk != null && e.JOBNFk.JOBNUM == input.JACCATJOBNUMFilter);

			var pagedAndFilteredJCJOBs = filteredJCJOBs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var jcjoBs = from o in pagedAndFilteredJCJOBs
                         join o1 in _lookup_jaccatRepository.GetAll() on o.JOBNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetJCJOBForViewDto() {
							JCJOB = new JCJOBDto
							{
                                STATE = o.STATE,
                                LOCALITY = o.LOCALITY,
                                CLASS = o.CLASS,
                                CLOSED = o.CLOSED,
                                Id = o.Id
							},
                         	JACCATJOBNUM = s1 == null ? "" : s1.JOBNUM.ToString()
						};

            var totalCount = await filteredJCJOBs.CountAsync();

            return new PagedResultDto<GetJCJOBForViewDto>(
                totalCount,
                await jcjoBs.ToListAsync()
            );
         }
		 
		 public async Task<GetJCJOBForViewDto> GetJCJOBForView(int id)
         {
            var jcjob = await _jcjobRepository.GetAsync(id);

            var output = new GetJCJOBForViewDto { JCJOB = ObjectMapper.Map<JCJOBDto>(jcjob) };

		    if (output.JCJOB.JOBNUM != null)
            {
                var _lookupJACCAT = await _lookup_jaccatRepository.FirstOrDefaultAsync((int)output.JCJOB.JOBNUM);
                output.JACCATJOBNUM = _lookupJACCAT.JOBNUM.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_JCJOBs_Edit)]
		 public async Task<GetJCJOBForEditOutput> GetJCJOBForEdit(EntityDto input)
         {
            var jcjob = await _jcjobRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetJCJOBForEditOutput {JCJOB = ObjectMapper.Map<CreateOrEditJCJOBDto>(jcjob)};

		    if (output.JCJOB.JOBNUM != null)
            {
                var _lookupJACCAT = await _lookup_jaccatRepository.FirstOrDefaultAsync((int)output.JCJOB.JOBNUM);
                output.JACCATJOBNUM = _lookupJACCAT.JOBNUM.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditJCJOBDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_JCJOBs_Create)]
		 protected virtual async Task Create(CreateOrEditJCJOBDto input)
         {
            var jcjob = ObjectMapper.Map<JCJOB>(input);

			
			if (AbpSession.TenantId != null)
			{
				jcjob.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _jcjobRepository.InsertAsync(jcjob);
         }

		 [AbpAuthorize(AppPermissions.Pages_JCJOBs_Edit)]
		 protected virtual async Task Update(CreateOrEditJCJOBDto input)
         {
            var jcjob = await _jcjobRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, jcjob);
         }

		 [AbpAuthorize(AppPermissions.Pages_JCJOBs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _jcjobRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetJCJOBsToExcel(GetAllJCJOBsForExcelInput input)
         {
			
			var filteredJCJOBs = _jcjobRepository.GetAll()
						.Include( e => e.JOBNFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.STATE.Contains(input.Filter) || e.LOCALITY.Contains(input.Filter) || e.CLASS.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.STATEFilter),  e => e.STATE == input.STATEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LOCALITYFilter),  e => e.LOCALITY == input.LOCALITYFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CLASSFilter),  e => e.CLASS == input.CLASSFilter)
						.WhereIf(input.MinCLOSEDFilter != null, e => e.CLOSED >= input.MinCLOSEDFilter)
						.WhereIf(input.MaxCLOSEDFilter != null, e => e.CLOSED <= input.MaxCLOSEDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JACCATJOBNUMFilter), e => e.JOBNFk != null && e.JOBNFk.JOBNUM == input.JACCATJOBNUMFilter);

			var query = (from o in filteredJCJOBs
                         join o1 in _lookup_jaccatRepository.GetAll() on o.JOBNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetJCJOBForViewDto() { 
							JCJOB = new JCJOBDto
							{
                                STATE = o.STATE,
                                LOCALITY = o.LOCALITY,
                                CLASS = o.CLASS,
                                CLOSED = o.CLOSED,
                                Id = o.Id
							},
                         	JACCATJOBNUM = s1 == null ? "" : s1.JOBNUM.ToString()
						 });


            var jcjobListDtos = await query.ToListAsync();

            return _jcjoBsExcelExporter.ExportToFile(jcjobListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_JCJOBs)]
         public async Task<PagedResultDto<JCJOBJACCATLookupTableDto>> GetAllJACCATForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_jaccatRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.JOBNUM != null ? e.JOBNUM.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var jaccatList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<JCJOBJACCATLookupTableDto>();
			foreach(var jaccat in jaccatList){
				lookupTableDtoList.Add(new JCJOBJACCATLookupTableDto
				{
					Id = jaccat.Id,
					DisplayName = jaccat.JOBNUM?.ToString()
				});
			}

            return new PagedResultDto<JCJOBJACCATLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
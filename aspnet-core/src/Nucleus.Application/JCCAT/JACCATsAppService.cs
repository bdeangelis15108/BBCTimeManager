

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.JCCAT.Exporting;
using Nucleus.JCCAT.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.JCCAT
{
	[AbpAuthorize(AppPermissions.Pages_JACCATs)]
    public class JACCATsAppService : NucleusAppServiceBase, IJACCATsAppService
    {
		 private readonly IRepository<JACCAT> _jaccatRepository;
		 private readonly IJACCATsExcelExporter _jaccaTsExcelExporter;
		 

		  public JACCATsAppService(IRepository<JACCAT> jaccatRepository, IJACCATsExcelExporter jaccaTsExcelExporter ) 
		  {
			_jaccatRepository = jaccatRepository;
			_jaccaTsExcelExporter = jaccaTsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetJACCATForViewDto>> GetAll(GetAllJACCATsInput input)
         {
			
			var filteredJACCATs = _jaccatRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.JOBNUM.Contains(input.Filter) || e.PHASENUM.Contains(input.Filter) || e.CATNUM.Contains(input.Filter) || e.NAME.Contains(input.Filter))
						.WhereIf(input.MinSEQUENCEFilter != null, e => e.SEQUENCE >= input.MinSEQUENCEFilter)
						.WhereIf(input.MaxSEQUENCEFilter != null, e => e.SEQUENCE <= input.MaxSEQUENCEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JOBNUMFilter),  e => e.JOBNUM == input.JOBNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PHASENUMFilter),  e => e.PHASENUM == input.PHASENUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CATNUMFilter),  e => e.CATNUM == input.CATNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NAMEFilter),  e => e.NAME == input.NAMEFilter);

			var pagedAndFilteredJACCATs = filteredJACCATs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var jaccaTs = from o in pagedAndFilteredJACCATs
                         select new GetJACCATForViewDto() {
							JACCAT = new JACCATDto
							{
                                SEQUENCE = o.SEQUENCE,
                                JOBNUM = o.JOBNUM,
                                PHASENUM = o.PHASENUM,
                                CATNUM = o.CATNUM,
                                NAME = o.NAME,
                                Id = o.Id
							}
						};

            var totalCount = await filteredJACCATs.CountAsync();

            return new PagedResultDto<GetJACCATForViewDto>(
                totalCount,
                await jaccaTs.ToListAsync()
            );
         }
		 
		 public async Task<GetJACCATForViewDto> GetJACCATForView(int id)
         {
            var jaccat = await _jaccatRepository.GetAsync(id);

            var output = new GetJACCATForViewDto { JACCAT = ObjectMapper.Map<JACCATDto>(jaccat) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_JACCATs_Edit)]
		 public async Task<GetJACCATForEditOutput> GetJACCATForEdit(EntityDto input)
         {
            var jaccat = await _jaccatRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetJACCATForEditOutput {JACCAT = ObjectMapper.Map<CreateOrEditJACCATDto>(jaccat)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditJACCATDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_JACCATs_Create)]
		 protected virtual async Task Create(CreateOrEditJACCATDto input)
         {
            var jaccat = ObjectMapper.Map<JACCAT>(input);

			
			if (AbpSession.TenantId != null)
			{
				jaccat.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _jaccatRepository.InsertAsync(jaccat);
         }

		 [AbpAuthorize(AppPermissions.Pages_JACCATs_Edit)]
		 protected virtual async Task Update(CreateOrEditJACCATDto input)
         {
            var jaccat = await _jaccatRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, jaccat);
         }

		 [AbpAuthorize(AppPermissions.Pages_JACCATs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _jaccatRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetJACCATsToExcel(GetAllJACCATsForExcelInput input)
         {
			
			var filteredJACCATs = _jaccatRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.JOBNUM.Contains(input.Filter) || e.PHASENUM.Contains(input.Filter) || e.CATNUM.Contains(input.Filter) || e.NAME.Contains(input.Filter))
						.WhereIf(input.MinSEQUENCEFilter != null, e => e.SEQUENCE >= input.MinSEQUENCEFilter)
						.WhereIf(input.MaxSEQUENCEFilter != null, e => e.SEQUENCE <= input.MaxSEQUENCEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JOBNUMFilter),  e => e.JOBNUM == input.JOBNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PHASENUMFilter),  e => e.PHASENUM == input.PHASENUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CATNUMFilter),  e => e.CATNUM == input.CATNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NAMEFilter),  e => e.NAME == input.NAMEFilter);

			var query = (from o in filteredJACCATs
                         select new GetJACCATForViewDto() { 
							JACCAT = new JACCATDto
							{
                                SEQUENCE = o.SEQUENCE,
                                JOBNUM = o.JOBNUM,
                                PHASENUM = o.PHASENUM,
                                CATNUM = o.CATNUM,
                                NAME = o.NAME,
                                Id = o.Id
							}
						 });


            var jaccatListDtos = await query.ToListAsync();

            return _jaccaTsExcelExporter.ExportToFile(jaccatListDtos);
         }


    }
}
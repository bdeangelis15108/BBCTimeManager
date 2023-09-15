using Nucleus.JCUNIONS;
using Nucleus.PREMPLOYEES;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.PRCLASSES.Exporting;
using Nucleus.PRCLASSES.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.PRCLASSES
{
	[AbpAuthorize(AppPermissions.Pages_PRCLASSs)]
    public class PRCLASSsAppService : NucleusAppServiceBase, IPRCLASSsAppService
    {
		 private readonly IRepository<PRCLASS> _prclassRepository;
		 private readonly IPRCLASSsExcelExporter _prclasSsExcelExporter;
		 private readonly IRepository<JCUNION,int> _lookup_jcunionRepository;
		 private readonly IRepository<PREMPLOYEE,int> _lookup_premployeeRepository;
		 

		  public PRCLASSsAppService(IRepository<PRCLASS> prclassRepository, IPRCLASSsExcelExporter prclasSsExcelExporter , IRepository<JCUNION, int> lookup_jcunionRepository, IRepository<PREMPLOYEE, int> lookup_premployeeRepository) 
		  {
			_prclassRepository = prclassRepository;
			_prclasSsExcelExporter = prclasSsExcelExporter;
			_lookup_jcunionRepository = lookup_jcunionRepository;
		_lookup_premployeeRepository = lookup_premployeeRepository;
		
		  }

		 public async Task<PagedResultDto<GetPRCLASSForViewDto>> GetAll(GetAllPRCLASSsInput input)
         {
			
			var filteredPRCLASSs = _prclassRepository.GetAll()
						.Include( e => e.UNIONNFk)
						.Include( e => e.CLAFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NAME.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NAMEFilter),  e => e.NAME == input.NAMEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JCUNIONUNIONNUMFilter), e => e.UNIONNFk != null && e.UNIONNFk.UNIONNUM == input.JCUNIONUNIONNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PREMPLOYEECLASSFilter), e => e.CLAFk != null && e.CLAFk.CLASS == input.PREMPLOYEECLASSFilter);

			var pagedAndFilteredPRCLASSs = filteredPRCLASSs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var prclasSs = from o in pagedAndFilteredPRCLASSs
                         join o1 in _lookup_jcunionRepository.GetAll() on o.UNIONNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_premployeeRepository.GetAll() on o.CLASS equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetPRCLASSForViewDto() {
							PRCLASS = new PRCLASSDto
							{
                                NAME = o.NAME,
                                Id = o.Id
							},
                         	JCUNIONUNIONNUM = s1 == null ? "" : s1.UNIONNUM.ToString(),
                         	PREMPLOYEECLASS = s2 == null ? "" : s2.CLASS.ToString()
						};

            var totalCount = await filteredPRCLASSs.CountAsync();

            return new PagedResultDto<GetPRCLASSForViewDto>(
                totalCount,
                await prclasSs.ToListAsync()
            );
         }
		 
		 public async Task<GetPRCLASSForViewDto> GetPRCLASSForView(int id)
         {
            var prclass = await _prclassRepository.GetAsync(id);

            var output = new GetPRCLASSForViewDto { PRCLASS = ObjectMapper.Map<PRCLASSDto>(prclass) };

		    if (output.PRCLASS.UNIONNUM != null)
            {
                var _lookupJCUNION = await _lookup_jcunionRepository.FirstOrDefaultAsync((int)output.PRCLASS.UNIONNUM);
                output.JCUNIONUNIONNUM = _lookupJCUNION.UNIONNUM.ToString();
            }

		    if (output.PRCLASS.CLASS != null)
            {
                var _lookupPREMPLOYEE = await _lookup_premployeeRepository.FirstOrDefaultAsync((int)output.PRCLASS.CLASS);
                output.PREMPLOYEECLASS = _lookupPREMPLOYEE.CLASS.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PRCLASSs_Edit)]
		 public async Task<GetPRCLASSForEditOutput> GetPRCLASSForEdit(EntityDto input)
         {
            var prclass = await _prclassRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPRCLASSForEditOutput {PRCLASS = ObjectMapper.Map<CreateOrEditPRCLASSDto>(prclass)};

		    if (output.PRCLASS.UNIONNUM != null)
            {
                var _lookupJCUNION = await _lookup_jcunionRepository.FirstOrDefaultAsync((int)output.PRCLASS.UNIONNUM);
                output.JCUNIONUNIONNUM = _lookupJCUNION.UNIONNUM.ToString();
            }

		    if (output.PRCLASS.CLASS != null)
            {
                var _lookupPREMPLOYEE = await _lookup_premployeeRepository.FirstOrDefaultAsync((int)output.PRCLASS.CLASS);
                output.PREMPLOYEECLASS = _lookupPREMPLOYEE.CLASS.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPRCLASSDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PRCLASSs_Create)]
		 protected virtual async Task Create(CreateOrEditPRCLASSDto input)
         {
            var prclass = ObjectMapper.Map<PRCLASS>(input);

			
			if (AbpSession.TenantId != null)
			{
				prclass.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _prclassRepository.InsertAsync(prclass);
         }

		 [AbpAuthorize(AppPermissions.Pages_PRCLASSs_Edit)]
		 protected virtual async Task Update(CreateOrEditPRCLASSDto input)
         {
            var prclass = await _prclassRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, prclass);
         }

		 [AbpAuthorize(AppPermissions.Pages_PRCLASSs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _prclassRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPRCLASSsToExcel(GetAllPRCLASSsForExcelInput input)
         {
			
			var filteredPRCLASSs = _prclassRepository.GetAll()
						.Include( e => e.UNIONNFk)
						.Include( e => e.CLAFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NAME.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NAMEFilter),  e => e.NAME == input.NAMEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.JCUNIONUNIONNUMFilter), e => e.UNIONNFk != null && e.UNIONNFk.UNIONNUM == input.JCUNIONUNIONNUMFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PREMPLOYEECLASSFilter), e => e.CLAFk != null && e.CLAFk.CLASS == input.PREMPLOYEECLASSFilter);

			var query = (from o in filteredPRCLASSs
                         join o1 in _lookup_jcunionRepository.GetAll() on o.UNIONNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_premployeeRepository.GetAll() on o.CLASS equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetPRCLASSForViewDto() { 
							PRCLASS = new PRCLASSDto
							{
                                NAME = o.NAME,
                                Id = o.Id
							},
                         	JCUNIONUNIONNUM = s1 == null ? "" : s1.UNIONNUM.ToString(),
                         	PREMPLOYEECLASS = s2 == null ? "" : s2.CLASS.ToString()
						 });


            var prclassListDtos = await query.ToListAsync();

            return _prclasSsExcelExporter.ExportToFile(prclassListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_PRCLASSs)]
         public async Task<PagedResultDto<PRCLASSJCUNIONLookupTableDto>> GetAllJCUNIONForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_jcunionRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.UNIONNUM != null ? e.UNIONNUM.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var jcunionList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PRCLASSJCUNIONLookupTableDto>();
			foreach(var jcunion in jcunionList){
				lookupTableDtoList.Add(new PRCLASSJCUNIONLookupTableDto
				{
					Id = jcunion.Id,
					DisplayName = jcunion.UNIONNUM?.ToString()
				});
			}

            return new PagedResultDto<PRCLASSJCUNIONLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_PRCLASSs)]
         public async Task<PagedResultDto<PRCLASSPREMPLOYEELookupTableDto>> GetAllPREMPLOYEEForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_premployeeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.CLASS != null ? e.CLASS.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var premployeeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PRCLASSPREMPLOYEELookupTableDto>();
			foreach(var premployee in premployeeList){
				lookupTableDtoList.Add(new PRCLASSPREMPLOYEELookupTableDto
				{
					Id = premployee.Id,
					DisplayName = premployee.CLASS?.ToString()
				});
			}

            return new PagedResultDto<PRCLASSPREMPLOYEELookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
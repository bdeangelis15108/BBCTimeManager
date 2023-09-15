using Nucleus.PRCLASSES;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.PRDEDRATES.Exporting;
using Nucleus.PRDEDRATES.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.PRDEDRATES
{
	[AbpAuthorize(AppPermissions.Pages_PRDEDRATES)]
    public class PRDEDRATESAppService : NucleusAppServiceBase, IPRDEDRATESAppService
    {
		 private readonly IRepository<PRDEDRATE> _prdedrateRepository;
		 private readonly IPRDEDRATESExcelExporter _prdedratesExcelExporter;
		 private readonly IRepository<PRCLASS,int> _lookup_prclassRepository;
		 

		  public PRDEDRATESAppService(IRepository<PRDEDRATE> prdedrateRepository, IPRDEDRATESExcelExporter prdedratesExcelExporter , IRepository<PRCLASS, int> lookup_prclassRepository) 
		  {
			_prdedrateRepository = prdedrateRepository;
			_prdedratesExcelExporter = prdedratesExcelExporter;
			_lookup_prclassRepository = lookup_prclassRepository;
		
		  }

		 public async Task<PagedResultDto<GetPRDEDRATEForViewDto>> GetAll(GetAllPRDEDRATESInput input)
         {
			
			var filteredPRDEDRATES = _prdedrateRepository.GetAll()
						.Include( e => e.UNIONNFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.UNIONLOCAL.Contains(input.Filter) || e.CLASS.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONLOCALFilter),  e => e.UNIONLOCAL == input.UNIONLOCALFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CLASSFilter),  e => e.CLASS == input.CLASSFilter)
						.WhereIf(input.MinDEDTYPEFilter != null, e => e.DEDTYPE >= input.MinDEDTYPEFilter)
						.WhereIf(input.MaxDEDTYPEFilter != null, e => e.DEDTYPE <= input.MaxDEDTYPEFilter)
						.WhereIf(input.MinPERHRFilter != null, e => e.PERHR >= input.MinPERHRFilter)
						.WhereIf(input.MaxPERHRFilter != null, e => e.PERHR <= input.MaxPERHRFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PRCLASSUNIONNUMFilter), e => e.UNIONNFk != null && e.UNIONNFk.UNIONNUM.Equals(input.PRCLASSUNIONNUMFilter));

			var pagedAndFilteredPRDEDRATES = filteredPRDEDRATES
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var prdedrates = from o in pagedAndFilteredPRDEDRATES
                         join o1 in _lookup_prclassRepository.GetAll() on o.UNIONNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPRDEDRATEForViewDto() {
							PRDEDRATE = new PRDEDRATEDto
							{
                                UNIONLOCAL = o.UNIONLOCAL,
                                CLASS = o.CLASS,
                                DEDTYPE = o.DEDTYPE,
                                PERHR = o.PERHR,
                                Id = o.Id
							},
                         	PRCLASSUNIONNUM = s1 == null ? "" : s1.UNIONNUM.ToString()
						};

            var totalCount = await filteredPRDEDRATES.CountAsync();

            return new PagedResultDto<GetPRDEDRATEForViewDto>(
                totalCount,
                await prdedrates.ToListAsync()
            );
         }
		 
		 public async Task<GetPRDEDRATEForViewDto> GetPRDEDRATEForView(int id)
         {
            var prdedrate = await _prdedrateRepository.GetAsync(id);

            var output = new GetPRDEDRATEForViewDto { PRDEDRATE = ObjectMapper.Map<PRDEDRATEDto>(prdedrate) };

		    if (output.PRDEDRATE.UNIONNUM != null)
            {
                var _lookupPRCLASS = await _lookup_prclassRepository.FirstOrDefaultAsync((int)output.PRDEDRATE.UNIONNUM);
                output.PRCLASSUNIONNUM = _lookupPRCLASS.UNIONNUM.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PRDEDRATES_Edit)]
		 public async Task<GetPRDEDRATEForEditOutput> GetPRDEDRATEForEdit(EntityDto input)
         {
            var prdedrate = await _prdedrateRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPRDEDRATEForEditOutput {PRDEDRATE = ObjectMapper.Map<CreateOrEditPRDEDRATEDto>(prdedrate)};

		    if (output.PRDEDRATE.UNIONNUM != null)
            {
                var _lookupPRCLASS = await _lookup_prclassRepository.FirstOrDefaultAsync((int)output.PRDEDRATE.UNIONNUM);
                output.PRCLASSUNIONNUM = _lookupPRCLASS.UNIONNUM.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPRDEDRATEDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PRDEDRATES_Create)]
		 protected virtual async Task Create(CreateOrEditPRDEDRATEDto input)
         {
            var prdedrate = ObjectMapper.Map<PRDEDRATE>(input);

			
			if (AbpSession.TenantId != null)
			{
				prdedrate.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _prdedrateRepository.InsertAsync(prdedrate);
         }

		 [AbpAuthorize(AppPermissions.Pages_PRDEDRATES_Edit)]
		 protected virtual async Task Update(CreateOrEditPRDEDRATEDto input)
         {
            var prdedrate = await _prdedrateRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, prdedrate);
         }

		 [AbpAuthorize(AppPermissions.Pages_PRDEDRATES_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _prdedrateRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPRDEDRATESToExcel(GetAllPRDEDRATESForExcelInput input)
         {
			
			var filteredPRDEDRATES = _prdedrateRepository.GetAll()
						.Include( e => e.UNIONNFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.UNIONLOCAL.Contains(input.Filter) || e.CLASS.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UNIONLOCALFilter),  e => e.UNIONLOCAL == input.UNIONLOCALFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CLASSFilter),  e => e.CLASS == input.CLASSFilter)
						.WhereIf(input.MinDEDTYPEFilter != null, e => e.DEDTYPE >= input.MinDEDTYPEFilter)
						.WhereIf(input.MaxDEDTYPEFilter != null, e => e.DEDTYPE <= input.MaxDEDTYPEFilter)
						.WhereIf(input.MinPERHRFilter != null, e => e.PERHR >= input.MinPERHRFilter)
						.WhereIf(input.MaxPERHRFilter != null, e => e.PERHR <= input.MaxPERHRFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PRCLASSUNIONNUMFilter), e => e.UNIONNFk != null && e.UNIONNFk.UNIONNUM.Equals(input.PRCLASSUNIONNUMFilter));

			var query = (from o in filteredPRDEDRATES
                         join o1 in _lookup_prclassRepository.GetAll() on o.UNIONNUM equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetPRDEDRATEForViewDto() { 
							PRDEDRATE = new PRDEDRATEDto
							{
                                UNIONLOCAL = o.UNIONLOCAL,
                                CLASS = o.CLASS,
                                DEDTYPE = o.DEDTYPE,
                                PERHR = o.PERHR,
                                Id = o.Id
							},
                         	PRCLASSUNIONNUM = s1 == null ? "" : s1.UNIONNUM.ToString()
						 });


            var prdedrateListDtos = await query.ToListAsync();

            return _prdedratesExcelExporter.ExportToFile(prdedrateListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_PRDEDRATES)]
         public async Task<PagedResultDto<PRDEDRATEPRCLASSLookupTableDto>> GetAllPRCLASSForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_prclassRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> (e.UNIONNUM != null ? e.UNIONNUM.ToString():"").Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var prclassList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PRDEDRATEPRCLASSLookupTableDto>();
			foreach(var prclass in prclassList){
                lookupTableDtoList.Add(new PRDEDRATEPRCLASSLookupTableDto
                {
                    Id = prclass.Id,
                    DisplayName = prclass.UNIONNUM.ToString()

                }) ;
			}

            return new PagedResultDto<PRDEDRATEPRCLASSLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}
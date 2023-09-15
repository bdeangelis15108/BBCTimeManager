using Nucleus.Authorization.Users;
using Nucleus.Resource;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.ResourceReservation.Exporting;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Nucleus.Resource.Dtos;

namespace Nucleus.ResourceReservation
{
	[AbpAuthorize(AppPermissions.Pages_ResourceReservationses)]
    public class ResourceReservationsesAppService : NucleusAppServiceBase, IResourceReservationsesAppService
    {
		 private readonly IRepository<ResourceReservations> _resourceReservationsRepository;
		 private readonly IResourceReservationsesExcelExporter _resourceReservationsesExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Resources,int> _lookup_resourcesRepository;
		 

		  public ResourceReservationsesAppService(IRepository<ResourceReservations> resourceReservationsRepository, IResourceReservationsesExcelExporter resourceReservationsesExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<Resources, int> lookup_resourcesRepository) 
		  {
			_resourceReservationsRepository = resourceReservationsRepository;
			_resourceReservationsesExcelExporter = resourceReservationsesExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_resourcesRepository = lookup_resourcesRepository;
		
		  }

		 public async Task<PagedResultDto<GetResourceReservationsForViewDto>> GetAll(GetAllResourceReservationsesInput input)
         {
			
			var filteredResourceReservationses = _resourceReservationsRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ResourcesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinReservedFromFilter != null, e => e.ReservedFrom >= input.MinReservedFromFilter)
						.WhereIf(input.MaxReservedFromFilter != null, e => e.ReservedFrom <= input.MaxReservedFromFilter)
						.WhereIf(input.MinReservedUntilFilter != null, e => e.ReservedUntil >= input.MinReservedUntilFilter)
						.WhereIf(input.MaxReservedUntilFilter != null, e => e.ReservedUntil <= input.MaxReservedUntilFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name == input.ResourcesNameFilter);

			var pagedAndFilteredResourceReservationses = filteredResourceReservationses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var resourceReservationses = from o in pagedAndFilteredResourceReservationses
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetResourceReservationsForViewDto() {
							ResourceReservations = new ResourceReservationsDto
							{
                                ReservedFrom = o.ReservedFrom,
                                ReservedUntil = o.ReservedUntil,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	ResourcesName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredResourceReservationses.CountAsync();

            return new PagedResultDto<GetResourceReservationsForViewDto>(
                totalCount,
                await resourceReservationses.ToListAsync()
            );
         }
		 
		 public async Task<GetResourceReservationsForViewDto> GetResourceReservationsForView(int id)
         {
            var resourceReservations = await _resourceReservationsRepository.GetAsync(id);

            var output = new GetResourceReservationsForViewDto { ResourceReservations = ObjectMapper.Map<ResourceReservationsDto>(resourceReservations) };

		    if (output.ResourceReservations.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ResourceReservations.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.ResourceReservations.ResourcesId != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.ResourceReservations.ResourcesId);
                output.ResourcesName = _lookupResources.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ResourceReservationses_Edit)]
		 public async Task<GetResourceReservationsForEditOutput> GetResourceReservationsForEdit(EntityDto input)
         {
            var resourceReservations = await _resourceReservationsRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetResourceReservationsForEditOutput {ResourceReservations = ObjectMapper.Map<CreateOrEditResourceReservationsDto>(resourceReservations)};

		    if (output.ResourceReservations.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ResourceReservations.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.ResourceReservations.ResourcesId != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.ResourceReservations.ResourcesId);
                output.ResourcesName = _lookupResources.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditResourceReservationsDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ResourceReservationses_Create)]
		 protected virtual async Task Create(CreateOrEditResourceReservationsDto input)
         {
            var resourceReservations = ObjectMapper.Map<ResourceReservations>(input);

			

            await _resourceReservationsRepository.InsertAsync(resourceReservations);
         }

		 [AbpAuthorize(AppPermissions.Pages_ResourceReservationses_Edit)]
		 protected virtual async Task Update(CreateOrEditResourceReservationsDto input)
         {
            var resourceReservations = await _resourceReservationsRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, resourceReservations);
         }

		[AbpAuthorize(AppPermissions.Pages_ResourceReservationses_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _resourceReservationsRepository.DeleteAsync(input.Id);
         }
        public async Task DeleteByResourceId(int id)
        {
            await _resourceReservationsRepository.DeleteAsync(id);
        }
       
        public async Task<FileDto> GetResourceReservationsesToExcel(GetAllResourceReservationsesForExcelInput input)
         {
			
			var filteredResourceReservationses = _resourceReservationsRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ResourcesFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinReservedFromFilter != null, e => e.ReservedFrom >= input.MinReservedFromFilter)
						.WhereIf(input.MaxReservedFromFilter != null, e => e.ReservedFrom <= input.MaxReservedFromFilter)
						.WhereIf(input.MinReservedUntilFilter != null, e => e.ReservedUntil >= input.MinReservedUntilFilter)
						.WhereIf(input.MaxReservedUntilFilter != null, e => e.ReservedUntil <= input.MaxReservedUntilFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name == input.ResourcesNameFilter);

			var query = (from o in filteredResourceReservationses
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetResourceReservationsForViewDto() { 
							ResourceReservations = new ResourceReservationsDto
							{
                                ReservedFrom = o.ReservedFrom,
                                ReservedUntil = o.ReservedUntil,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	ResourcesName = s2 == null ? "" : s2.Name.ToString()
						 });


            var resourceReservationsListDtos = await query.ToListAsync();

            return _resourceReservationsesExcelExporter.ExportToFile(resourceReservationsListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ResourceReservationses)]
        public async Task<PagedResultDto<ResourceReservationsUserLookupTableDto>> GetAllUserForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ResourceReservationsUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new ResourceReservationsUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<ResourceReservationsUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ResourceReservationses)]
        public async Task<PagedResultDto<ResourceReservationsResourcesLookupTableDto>> GetAllResourcesForLookupTable(Dtos.GetAllForLookupTableInput input)
        {
             var query = _lookup_resourcesRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var resourcesList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ResourceReservationsResourcesLookupTableDto>();
			foreach(var resources in resourcesList){
				lookupTableDtoList.Add(new ResourceReservationsResourcesLookupTableDto
				{
					Id = resources.Id,
					DisplayName = resources.Name?.ToString()
				});
			}

            return new PagedResultDto<ResourceReservationsResourcesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
       public  async Task<PagedResultDto<GetMyResourceReservationsDto>> GetMyReservedResource(GetAllResourceReservationsesInput input)
        {
            var filteredResourceReservationses = _resourceReservationsRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.ResourcesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinReservedFromFilter != null, e => e.ReservedFrom >= input.MinReservedFromFilter)
                        .WhereIf(input.MaxReservedFromFilter != null, e => e.ReservedFrom <= input.MaxReservedFromFilter)
                        .WhereIf(input.MinReservedUntilFilter != null, e => e.ReservedUntil >= input.MinReservedUntilFilter)
                        .WhereIf(input.MaxReservedUntilFilter != null, e => e.ReservedUntil <= input.MaxReservedUntilFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesFk != null && e.ResourcesFk.Name.Contains( input.ResourcesNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesTypeFilter), e => e.ResourcesFk != null && e.ResourcesFk.Type == input.ResourcesTypeFilter);

            var pagedAndFilteredResourceReservationses = filteredResourceReservationses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var resourceReservationses = from o in pagedAndFilteredResourceReservationses
                                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new GetMyResourceReservationsDto()
                                         {
                                             Resources = new ResourcesDto
                                             {
                                                ResourceNumber=o.ResourcesFk.ResourceNumber,
                                                Name= o.ResourcesFk.Name,
                                                Type=o.ResourcesFk.Type,
                                                Id=o.ResourcesFk.Id
                                             },
                                             UserName = s1 == null ? "" : s1.Name.ToString(),
                                             ResourcesName = s2 == null ? "" : s2.Name.ToString(),
                                             ReservedFrom=o.ReservedFrom,
                                             ReservedUntil=o.ReservedUntil,
                                             Id=o.Id
                                         };

            var totalCount = await filteredResourceReservationses.CountAsync();

            return new PagedResultDto<GetMyResourceReservationsDto>(
                totalCount,
                await resourceReservationses.ToListAsync()
            );
        }

       
    }
}
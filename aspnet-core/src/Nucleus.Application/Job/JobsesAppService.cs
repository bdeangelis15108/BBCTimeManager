using Nucleus.Address;
using Nucleus.JobClass;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Job.Exporting;
using Nucleus.Job.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Job
{
    [AbpAuthorize(AppPermissions.Pages_Jobses)]
    public class JobsesAppService : NucleusAppServiceBase, IJobsesAppService
    {
        private readonly IRepository<Jobs> _jobsRepository;
        private readonly IJobsesExcelExporter _jobsesExcelExporter;
        private readonly IRepository<Addresses, int> _lookup_addressesRepository;
        private readonly IRepository<JobClasses, int> _lookup_jobClassesRepository;


        public JobsesAppService(IRepository<Jobs> jobsRepository, IJobsesExcelExporter jobsesExcelExporter, IRepository<Addresses, int> lookup_addressesRepository, IRepository<JobClasses, int> lookup_jobClassesRepository)
        {
            _jobsRepository = jobsRepository;
            _jobsesExcelExporter = jobsesExcelExporter;
            _lookup_addressesRepository = lookup_addressesRepository;
            _lookup_jobClassesRepository = lookup_jobClassesRepository;

        }
        // CostCode
        public async Task<PagedResultDto<GetJobsForViewDto>> GetAll(GetAllJobsesInput input)
        {

            try
            {
                var filteredJobses = _jobsRepository.GetAll()
                       .Include(e => e.AddressesFk)
                       .Include(e => e.JobClassesFk)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                       .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                       .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                       .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                       .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                       .WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
                       .WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.AddressesLinne1Filter), e => e.AddressesFk != null && e.AddressesFk.Linne1 == input.AddressesLinne1Filter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.JobClassesNameFilter), e => e.JobClassesFk != null && e.JobClassesFk.Name == input.JobClassesNameFilter);

                var pagedAndFilteredJobses = filteredJobses
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var jobses = from o in pagedAndFilteredJobses
                             join o1 in _lookup_addressesRepository.GetAll() on o.AddressesId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_jobClassesRepository.GetAll() on o.JobClassesId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new GetJobsForViewDto()
                             {
                                 Jobs = new JobsDto
                                 {
                                     Code = o.Code,
                                     Name = o.Name,
                                     StartDate = o.StartDate,
                                     EndDate = o.EndDate,
                                     Status = o.Status,
                                     Id = o.Id
                                 },
                                 AddressesLinne1 = s1 == null ? "" : s1.Linne1,
                                 JobClassesName = s2 == null ? "" : s2.Name
                             };

                var totalCount = await filteredJobses.CountAsync();

                return new PagedResultDto<GetJobsForViewDto>(
                    totalCount,
                    await jobses.ToListAsync()
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetJobsForViewDto> GetJobsForView(int id)
        {
            var jobs = await _jobsRepository.GetAsync(id);

            var output = new GetJobsForViewDto { Jobs = ObjectMapper.Map<JobsDto>(jobs) };

            if (output.Jobs.AddressesId != null)
            {
                var _lookupAddresses = await _lookup_addressesRepository.FirstOrDefaultAsync((int)output.Jobs.AddressesId);
                output.AddressesLinne1 = _lookupAddresses.Linne1.ToString();
            }

            if (output.Jobs.JobClassesId != null)
            {
                var _lookupJobClasses = await _lookup_jobClassesRepository.FirstOrDefaultAsync((int)output.Jobs.JobClassesId);
                output.JobClassesName = _lookupJobClasses.Name.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Jobses_Edit)]
        public async Task<GetJobsForEditOutput> GetJobsForEdit(EntityDto input)
        {
            var jobs = await _jobsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetJobsForEditOutput { Jobs = ObjectMapper.Map<CreateOrEditJobsDto>(jobs) };

            if (output.Jobs.AddressesId != null)
            {
                var _lookupAddresses = await _lookup_addressesRepository.FirstOrDefaultAsync((int)output.Jobs.AddressesId);
                output.AddressesLinne1 = _lookupAddresses.Linne1.ToString();
            }

            if (output.Jobs.JobClassesId != null)
            {
                var _lookupJobClasses = await _lookup_jobClassesRepository.FirstOrDefaultAsync((int)output.Jobs.JobClassesId);
                output.JobClassesName = _lookupJobClasses.Name.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditJobsDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Jobses_Create)]
        protected virtual async Task Create(CreateOrEditJobsDto input)
        {
            var jobs = ObjectMapper.Map<Jobs>(input);



            await _jobsRepository.InsertAsync(jobs);
        }

        [AbpAuthorize(AppPermissions.Pages_Jobses_Edit)]
        protected virtual async Task Update(CreateOrEditJobsDto input)
        {
            var jobs = await _jobsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, jobs);
        }

        [AbpAuthorize(AppPermissions.Pages_Jobses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _jobsRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetJobsesToExcel(GetAllJobsesForExcelInput input)
        {

            var filteredJobses = _jobsRepository.GetAll()
                        .Include(e => e.AddressesFk)
                        .Include(e => e.JobClassesFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code == input.CodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
                        .WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressesLinne1Filter), e => e.AddressesFk != null && e.AddressesFk.Linne1 == input.AddressesLinne1Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobClassesNameFilter), e => e.JobClassesFk != null && e.JobClassesFk.Name == input.JobClassesNameFilter);

            var query = (from o in filteredJobses
                         join o1 in _lookup_addressesRepository.GetAll() on o.AddressesId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_jobClassesRepository.GetAll() on o.JobClassesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetJobsForViewDto()
                         {
                             Jobs = new JobsDto
                             {
                                 Code = o.Code,
                                 Name = o.Name,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 Status = o.Status,
                                 Id = o.Id
                             },
                             AddressesLinne1 = s1 == null ? "" : s1.Linne1.ToString(),
                             JobClassesName = s2 == null ? "" : s2.Name.ToString()
                         });


            var jobsListDtos = await query.ToListAsync();

            return _jobsesExcelExporter.ExportToFile(jobsListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_Jobses)]
        public async Task<PagedResultDto<JobsAddressesLookupTableDto>> GetAllAddressesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_addressesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Linne1 != null && e.Linne1.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var addressesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobsAddressesLookupTableDto>();
            foreach (var addresses in addressesList)
            {
                lookupTableDtoList.Add(new JobsAddressesLookupTableDto
                {
                    Id = addresses.Id,
                    DisplayName = addresses.Linne1?.ToString()
                });
            }

            return new PagedResultDto<JobsAddressesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobses)]
        public async Task<PagedResultDto<JobsJobClassesLookupTableDto>> GetAllJobClassesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobClassesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var jobClassesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobsJobClassesLookupTableDto>();
            foreach (var jobClasses in jobClassesList)
            {
                lookupTableDtoList.Add(new JobsJobClassesLookupTableDto
                {
                    Id = jobClasses.Id,
                    DisplayName = jobClasses.Name?.ToString()
                });
            }

            return new PagedResultDto<JobsJobClassesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
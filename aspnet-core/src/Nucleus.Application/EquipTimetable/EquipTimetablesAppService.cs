using Nucleus.PayPeriod;
using Nucleus.Resource;
using Nucleus.JobPhaseCode;
using Nucleus.JobCategory;
using Nucleus.Job;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.EquipTimetable.Exporting;
using Nucleus.EquipTimetable.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.EquipTimetable
{
    [AbpAuthorize(AppPermissions.Pages_EquipTimetables)]
    public class EquipTimetablesAppService : NucleusAppServiceBase, IEquipTimetablesAppService
    {
        private readonly IRepository<EquipTimetables> _equipTimetablesRepository;
        private readonly IEquipTimetablesExcelExporter _equipTimetablesExcelExporter;
        private readonly IRepository<PayPeriods, int> _lookup_payPeriodsRepository;
        private readonly IRepository<Resources, int> _lookup_resourcesRepository;
        private readonly IRepository<JobPhaseCodes, int> _lookup_jobPhaseCodesRepository;
        private readonly IRepository<JobCategories, int> _lookup_jobCategoriesRepository;
        private readonly IRepository<Jobs, int> _lookup_jobsRepository;

        public EquipTimetablesAppService(IRepository<EquipTimetables> equipTimetablesRepository, IEquipTimetablesExcelExporter equipTimetablesExcelExporter, IRepository<PayPeriods, int> lookup_payPeriodsRepository, IRepository<Resources, int> lookup_resourcesRepository, IRepository<JobPhaseCodes, int> lookup_jobPhaseCodesRepository, IRepository<JobCategories, int> lookup_jobCategoriesRepository, IRepository<Jobs, int> lookup_jobsRepository)
        {
            _equipTimetablesRepository = equipTimetablesRepository;
            _equipTimetablesExcelExporter = equipTimetablesExcelExporter;
            _lookup_payPeriodsRepository = lookup_payPeriodsRepository;
            _lookup_resourcesRepository = lookup_resourcesRepository;
            _lookup_jobPhaseCodesRepository = lookup_jobPhaseCodesRepository;
            _lookup_jobCategoriesRepository = lookup_jobCategoriesRepository;
            _lookup_jobsRepository = lookup_jobsRepository;

        }

        public async Task<PagedResultDto<GetEquipTimetablesForViewDto>> GetAll(GetAllEquipTimetablesInput input)
        {
            // todo: Get Job ID on the basis of jobcode
            var jobCode = _lookup_jobsRepository.GetAll().Where(x => x.Code == input.JobCode).FirstOrDefault(); // -1; // input.JobCode 
            if (jobCode == null)
            {
                return new PagedResultDto<GetEquipTimetablesForViewDto>(
                    0,
                    new List<GetEquipTimetablesForViewDto>()
                );
            }
            var filteredEquipTimetables = _equipTimetablesRepository.GetAll()
                        .Include(e => e.PeriodDateFk)
                        .Include(e => e.ResourcesCodeFk)
                        .Include(e => e.PhaseCodeFk)
                        .Include(e => e.CategoryCodeFk)
                        .Include(e => e.JobCodeFk)
                        .Where(x => x.JobCode == jobCode.Id && x.PeriodDate == input.PayPeriodId /*&& x.IsActive*/
                            && ((x.Day1 != null && x.Day1.HasValue && x.Day1.Value > 0)
                            || (x.Day2 != null && x.Day2.HasValue && x.Day2.Value > 0)
                            || (x.Day3 != null && x.Day3.HasValue && x.Day3.Value > 0)
                            || (x.Day4 != null && x.Day4.HasValue && x.Day4.Value > 0)
                            || (x.Day5 != null && x.Day5.HasValue && x.Day5.Value > 0)
                            || (x.Day6 != null && x.Day6.HasValue && x.Day6.Value > 0)
                            || (x.Day7 != null && x.Day7.HasValue && x.Day7.Value > 0)))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDay1Filter != null, e => e.Day1 >= input.MinDay1Filter)
                        .WhereIf(input.MaxDay1Filter != null, e => e.Day1 <= input.MaxDay1Filter)
                        .WhereIf(input.MinDay2Filter != null, e => e.Day2 >= input.MinDay2Filter)
                        .WhereIf(input.MaxDay2Filter != null, e => e.Day2 <= input.MaxDay2Filter)
                        .WhereIf(input.MinDay3Filter != null, e => e.Day3 >= input.MinDay3Filter)
                        .WhereIf(input.MaxDay3Filter != null, e => e.Day3 <= input.MaxDay3Filter)
                        .WhereIf(input.MinDay4Filter != null, e => e.Day4 >= input.MinDay4Filter)
                        .WhereIf(input.MaxDay4Filter != null, e => e.Day4 <= input.MaxDay4Filter)
                        .WhereIf(input.MinDay5Filter != null, e => e.Day5 >= input.MinDay5Filter)
                        .WhereIf(input.MaxDay5Filter != null, e => e.Day5 <= input.MaxDay5Filter)
                        .WhereIf(input.MinDay6Filter != null, e => e.Day6 >= input.MinDay6Filter)
                        .WhereIf(input.MaxDay6Filter != null, e => e.Day6 <= input.MaxDay6Filter)
                        .WhereIf(input.MinDay7Filter != null, e => e.Day7 >= input.MinDay7Filter)
                        .WhereIf(input.MaxDay7Filter != null, e => e.Day7 <= input.MaxDay7Filter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinCreatedOnFilter != null, e => e.CreatedOn >= input.MinCreatedOnFilter)
                        .WhereIf(input.MaxCreatedOnFilter != null, e => e.CreatedOn <= input.MaxCreatedOnFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayPeriodsNameFilter), e => e.PeriodDateFk != null && e.PeriodDateFk.Name == input.PayPeriodsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesResourceNumberFilter), e => e.ResourcesCodeFk != null && e.ResourcesCodeFk.ResourceNumber == input.ResourcesResourceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobPhaseCodesNameFilter), e => e.PhaseCodeFk != null && e.PhaseCodeFk.Name == input.JobPhaseCodesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobCategoriesNameFilter), e => e.CategoryCodeFk != null && e.CategoryCodeFk.Name == input.JobCategoriesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobCodeFk != null && e.JobCodeFk.Name == input.JobsNameFilter);

            var pagedAndFilteredEquipTimetables = filteredEquipTimetables
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var equipTimetables = from o in pagedAndFilteredEquipTimetables
                                  join o1 in _lookup_payPeriodsRepository.GetAll() on o.PeriodDate equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesCode equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  join o3 in _lookup_jobPhaseCodesRepository.GetAll() on o.PhaseCode equals o3.Id into j3
                                  from s3 in j3.DefaultIfEmpty()

                                  join o4 in _lookup_jobCategoriesRepository.GetAll() on o.CategoryCode equals o4.Id into j4
                                  from s4 in j4.DefaultIfEmpty()

                                  join o5 in _lookup_jobsRepository.GetAll() on o.JobCode equals o5.Id into j5
                                  from s5 in j5.DefaultIfEmpty()

                                  select new GetEquipTimetablesForViewDto()
                                  {
                                      EquipTimetables = new EquipTimetablesDto
                                      {
                                          Day1 = o.Day1,
                                          Day2 = o.Day2,
                                          Day3 = o.Day3,
                                          Day4 = o.Day4,
                                          Day5 = o.Day5,
                                          Day6 = o.Day6,
                                          Day7 = o.Day7,
                                          Amount = o.Amount,
                                          CreatedOn = o.CreatedOn,
                                          IsActive = o.IsActive,
                                          Id = o.Id
                                      },
                                      PayPeriodsName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      ResourcesResourceNumber = s2 == null || s2.ResourceNumber == null ? "" : s2.ResourceNumber.ToString(),
                                      ResourcesResourceName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                      JobPhaseCodesName = s3 == null || s3.Code == null ? "" : s3.Code.ToString(),
                                      JobCategoriesName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                                      JobsName = s5 == null || s5.Code == null ? "" : s5.Code.ToString(),
                                      CostPerHour = s2 == null || s2.CostPerHour == null ? 0.00M : s2.CostPerHour.Value
                                  };

            var totalCount = await filteredEquipTimetables.CountAsync();

            return new PagedResultDto<GetEquipTimetablesForViewDto>(
                totalCount,
                await equipTimetables.ToListAsync()
            );
        }

        public async Task<GetEquipTimetablesForViewDto> GetEquipTimetablesForView(int id)
        {
            var equipTimetables = await _equipTimetablesRepository.GetAsync(id);

            var output = new GetEquipTimetablesForViewDto { EquipTimetables = ObjectMapper.Map<EquipTimetablesDto>(equipTimetables) };

            if (output.EquipTimetables.PeriodDate != null)
            {
                var _lookupPayPeriods = await _lookup_payPeriodsRepository.FirstOrDefaultAsync((int)output.EquipTimetables.PeriodDate);
                output.PayPeriodsName = _lookupPayPeriods?.Name?.ToString();
            }

            if (output.EquipTimetables.ResourcesCode != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.EquipTimetables.ResourcesCode);
                output.ResourcesResourceNumber = _lookupResources?.ResourceNumber?.ToString();
            }

            if (output.EquipTimetables.PhaseCode != null)
            {
                var _lookupJobPhaseCodes = await _lookup_jobPhaseCodesRepository.FirstOrDefaultAsync((int)output.EquipTimetables.PhaseCode);
                output.JobPhaseCodesName = _lookupJobPhaseCodes?.Name?.ToString();
            }

            if (output.EquipTimetables.CategoryCode != null)
            {
                var _lookupJobCategories = await _lookup_jobCategoriesRepository.FirstOrDefaultAsync((int)output.EquipTimetables.CategoryCode);
                output.JobCategoriesName = _lookupJobCategories?.Name?.ToString();
            }

            if (output.EquipTimetables.JobCode != null)
            {
                var _lookupJobs = await _lookup_jobsRepository.FirstOrDefaultAsync((int)output.EquipTimetables.JobCode);
                output.JobsName = _lookupJobs?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables_Edit)]
        public async Task<GetEquipTimetablesForEditOutput> GetEquipTimetablesForEdit(EntityDto input)
        {
            var equipTimetables = await _equipTimetablesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEquipTimetablesForEditOutput { EquipTimetables = ObjectMapper.Map<CreateOrEditEquipTimetablesDto>(equipTimetables) };

            if (output.EquipTimetables.PeriodDate != null)
            {
                var _lookupPayPeriods = await _lookup_payPeriodsRepository.FirstOrDefaultAsync((int)output.EquipTimetables.PeriodDate);
                output.PayPeriodsName = _lookupPayPeriods?.Name?.ToString();
            }

            if (output.EquipTimetables.ResourcesCode != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.EquipTimetables.ResourcesCode);
                output.ResourcesResourceNumber = _lookupResources?.ResourceNumber?.ToString();
            }

            if (output.EquipTimetables.PhaseCode != null)
            {
                var _lookupJobPhaseCodes = await _lookup_jobPhaseCodesRepository.FirstOrDefaultAsync((int)output.EquipTimetables.PhaseCode);
                output.JobPhaseCodesName = _lookupJobPhaseCodes?.Name?.ToString();
            }

            if (output.EquipTimetables.CategoryCode != null)
            {
                var _lookupJobCategories = await _lookup_jobCategoriesRepository.FirstOrDefaultAsync((int)output.EquipTimetables.CategoryCode);
                output.JobCategoriesName = _lookupJobCategories?.Name?.ToString();
            }

            if (output.EquipTimetables.JobCode != null)
            {
                var _lookupJobs = await _lookup_jobsRepository.FirstOrDefaultAsync((int)output.EquipTimetables.JobCode);
                output.JobsName = _lookupJobs?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEquipTimetablesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables_Create)]
        protected virtual async Task Create(CreateOrEditEquipTimetablesDto input)
        {
            var equipTimetables = ObjectMapper.Map<EquipTimetables>(input);

            var jobCode = _lookup_jobsRepository.GetAll().Where(x => x.Code == input.JobCodeString).FirstOrDefault();

            if (jobCode == null) throw new Exception("Job Code is Null.");

            decimal costPerHour = 0;

            var resource = _lookup_resourcesRepository.GetAll().Where(x => x.Id == equipTimetables.Id).FirstOrDefault();

            if (resource != null && resource.CostPerHour != null && resource.CostPerHour.HasValue)
            {
                costPerHour = resource.CostPerHour.Value;
            }

            decimal totalHours = 0;

            if (equipTimetables.Day1 != null && equipTimetables.Day1.HasValue)
            {
                totalHours += equipTimetables.Day1.Value;
            }
            if (equipTimetables.Day2 != null && equipTimetables.Day2.HasValue)
            {
                totalHours += equipTimetables.Day2.Value;
            }
            if (equipTimetables.Day3 != null && equipTimetables.Day3.HasValue)
            {
                totalHours += equipTimetables.Day3.Value;
            }
            if (equipTimetables.Day4 != null && equipTimetables.Day4.HasValue)
            {
                totalHours += equipTimetables.Day4.Value;
            }
            if (equipTimetables.Day5 != null && equipTimetables.Day5.HasValue)
            {
                totalHours += equipTimetables.Day5.Value;
            }
            if (equipTimetables.Day6 != null && equipTimetables.Day6.HasValue)
            {
                totalHours += equipTimetables.Day6.Value;
            }
            if (equipTimetables.Day7 != null && equipTimetables.Day7.HasValue)
            {
                totalHours += equipTimetables.Day7.Value;
            }
            equipTimetables.JobCode = jobCode.Id;
            decimal amount = costPerHour * totalHours;
            equipTimetables.Amount = amount;
            equipTimetables.CreatedOn = DateTime.Now;
            equipTimetables.IsActive = true;
            await _equipTimetablesRepository.InsertAsync(equipTimetables);
        }

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables_Edit)]
        protected virtual async Task Update(CreateOrEditEquipTimetablesDto input)
        {
            var equipTimetables = await _equipTimetablesRepository.FirstOrDefaultAsync((int)input.Id);
            // todo: Calculate Amount, CostPerHour * Days

            decimal costPerHour = 0;

            var resource = _lookup_resourcesRepository.GetAll().Where(x => x.Id == equipTimetables.Id).FirstOrDefault();

            if (resource != null && resource.CostPerHour != null && resource.CostPerHour.HasValue)
            {
                costPerHour = resource.CostPerHour.Value;
            }

            decimal totalHours = 0;

            if (equipTimetables.Day1 != null && equipTimetables.Day1.HasValue)
            {
                totalHours += equipTimetables.Day1.Value;
            }
            if (equipTimetables.Day2 != null && equipTimetables.Day2.HasValue)
            {
                totalHours += equipTimetables.Day2.Value;
            }
            if (equipTimetables.Day3 != null && equipTimetables.Day3.HasValue)
            {
                totalHours += equipTimetables.Day3.Value;
            }
            if (equipTimetables.Day4 != null && equipTimetables.Day4.HasValue)
            {
                totalHours += equipTimetables.Day4.Value;
            }
            if (equipTimetables.Day5 != null && equipTimetables.Day5.HasValue)
            {
                totalHours += equipTimetables.Day5.Value;
            }
            if (equipTimetables.Day6 != null && equipTimetables.Day6.HasValue)
            {
                totalHours += equipTimetables.Day6.Value;
            }
            if (equipTimetables.Day7 != null && equipTimetables.Day7.HasValue)
            {
                totalHours += equipTimetables.Day7.Value;
            }

            decimal amount = costPerHour * totalHours;
            equipTimetables.Amount = amount;
            equipTimetables.CreatedOn = DateTime.Now;
            
            ObjectMapper.Map(input, equipTimetables);
        }

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _equipTimetablesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEquipTimetablesToExcel(GetAllEquipTimetablesForExcelInput input)
        {
            var jobCode = _lookup_jobsRepository.GetAll().Where(x => x.Code == input.JobCode).FirstOrDefault(); // -1; // input.JobCode 
            if (jobCode == null)
            {
                return new FileDto 
                {
                    FileName = "",
                    FileToken = "",
                    FileType = ""
                };
            }
            var payPeriod = _lookup_payPeriodsRepository.GetAll().Where(x => x.Id == input.PayPeriodId).FirstOrDefault();
            if(payPeriod == null)
            {
                return new FileDto
                {
                    FileName = "",
                    FileToken = "",
                    FileType = ""
                };
            }
            var filteredEquipTimetables = _equipTimetablesRepository.GetAll()
                        .Include(e => e.PeriodDateFk)
                        .Include(e => e.ResourcesCodeFk)
                        .Include(e => e.PhaseCodeFk)
                        .Include(e => e.CategoryCodeFk)
                        .Include(e => e.JobCodeFk)
                        .Where(x => x.JobCode == jobCode.Id && x.PeriodDate == input.PayPeriodId && x.IsActive
                            && ((x.Day1 != null && x.Day1.HasValue && x.Day1.Value > 0)
                            || (x.Day2 != null && x.Day2.HasValue && x.Day2.Value > 0)
                            || (x.Day3 != null && x.Day3.HasValue && x.Day3.Value > 0)
                            || (x.Day4 != null && x.Day4.HasValue && x.Day4.Value > 0)
                            || (x.Day5 != null && x.Day5.HasValue && x.Day5.Value > 0)
                            || (x.Day6 != null && x.Day6.HasValue && x.Day6.Value > 0)
                            || (x.Day7 != null && x.Day7.HasValue && x.Day7.Value > 0)))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDay1Filter != null, e => e.Day1 >= input.MinDay1Filter)
                        .WhereIf(input.MaxDay1Filter != null, e => e.Day1 <= input.MaxDay1Filter)
                        .WhereIf(input.MinDay2Filter != null, e => e.Day2 >= input.MinDay2Filter)
                        .WhereIf(input.MaxDay2Filter != null, e => e.Day2 <= input.MaxDay2Filter)
                        .WhereIf(input.MinDay3Filter != null, e => e.Day3 >= input.MinDay3Filter)
                        .WhereIf(input.MaxDay3Filter != null, e => e.Day3 <= input.MaxDay3Filter)
                        .WhereIf(input.MinDay4Filter != null, e => e.Day4 >= input.MinDay4Filter)
                        .WhereIf(input.MaxDay4Filter != null, e => e.Day4 <= input.MaxDay4Filter)
                        .WhereIf(input.MinDay5Filter != null, e => e.Day5 >= input.MinDay5Filter)
                        .WhereIf(input.MaxDay5Filter != null, e => e.Day5 <= input.MaxDay5Filter)
                        .WhereIf(input.MinDay6Filter != null, e => e.Day6 >= input.MinDay6Filter)
                        .WhereIf(input.MaxDay6Filter != null, e => e.Day6 <= input.MaxDay6Filter)
                        .WhereIf(input.MinDay7Filter != null, e => e.Day7 >= input.MinDay7Filter)
                        .WhereIf(input.MaxDay7Filter != null, e => e.Day7 <= input.MaxDay7Filter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinCreatedOnFilter != null, e => e.CreatedOn >= input.MinCreatedOnFilter)
                        .WhereIf(input.MaxCreatedOnFilter != null, e => e.CreatedOn <= input.MaxCreatedOnFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayPeriodsNameFilter), e => e.PeriodDateFk != null && e.PeriodDateFk.Name == input.PayPeriodsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesResourceNumberFilter), e => e.ResourcesCodeFk != null && e.ResourcesCodeFk.ResourceNumber == input.ResourcesResourceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobPhaseCodesNameFilter), e => e.PhaseCodeFk != null && e.PhaseCodeFk.Name == input.JobPhaseCodesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobCategoriesNameFilter), e => e.CategoryCodeFk != null && e.CategoryCodeFk.Name == input.JobCategoriesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobCodeFk != null && e.JobCodeFk.Name == input.JobsNameFilter);

            var query = (from o in filteredEquipTimetables
                         join o1 in _lookup_payPeriodsRepository.GetAll() on o.PeriodDate equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesCode equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_jobPhaseCodesRepository.GetAll() on o.PhaseCode equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_jobCategoriesRepository.GetAll() on o.CategoryCode equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_jobsRepository.GetAll() on o.JobCode equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetEquipTimetablesForViewDto()
                         {
                             EquipTimetables = new EquipTimetablesDto
                             {
                                 Day1 = o.Day1,
                                 Day2 = o.Day2,
                                 Day3 = o.Day3,
                                 Day4 = o.Day4,
                                 Day5 = o.Day5,
                                 Day6 = o.Day6,
                                 Day7 = o.Day7,
                                 Amount = o.Amount,
                                 CreatedOn = o.CreatedOn,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             },
                             PayPeriodsName = s1 == null || s1.Name == null ? "N/A" : s1.Name.ToString(),
                             ResourcesResourceNumber = s2 == null || s2.ResourceNumber == null ? "N/A" : s2.ResourceNumber.ToString(),
                             ResourcesResourceName = s2 == null || string.IsNullOrWhiteSpace(s2.Name) ? "N/A" : s2.Name,
                             JobPhaseCodesName = s3 == null || s3.Name == null ? "N/A" : s3.Name.ToString(),
                             PhaseCode = s3 == null || s3.Code == null ? "N/A" : s3.Code.ToString(),
                             JobCategoriesName = s4 == null || s4.Name == null ? "N/A" : s4.Name.ToString(),
                             CategoryCode = s4 == null || s4.Code == null ? "N/A" : s4.Code.ToString(),
                             JobsName = s5 == null || s5.Name == null ? "N/A" : s5.Name.ToString(),
                             JobCode = s5 == null || s5.Code == null ? "N/A" : s5.Code.ToString(),
                             CostPerHour = s2 == null || s2.CostPerHour == null ? 0.00M : s2.CostPerHour.Value
                         });

            var equipTimetablesListDtos = await query.ToListAsync();


            
            var days = new string[7];

            if (payPeriod != null)
            {


                var day1 = payPeriod.StartDate.ToString("MM/dd/yyyy");
                var day2 = payPeriod.StartDate.AddDays(1).ToString("MM/dd/yyyy");
                var day3 = payPeriod.StartDate.AddDays(2).ToString("MM/dd/yyyy");
                var day4 = payPeriod.StartDate.AddDays(3).ToString("MM/dd/yyyy");
                var day5 = payPeriod.StartDate.AddDays(4).ToString("MM/dd/yyyy");
                var day6 = payPeriod.StartDate.AddDays(5).ToString("MM/dd/yyyy");
                var day7 = payPeriod.StartDate.AddDays(6).ToString("MM/dd/yyyy");
                //var month = payPeriod.StartDate.ToString("MMM");
                //days[0] = $"{month}-{day1}";
                //days[1] = $"{month}-{day2}";
                //days[2] = $"{month}-{day3}";
                //days[3] = $"{month}-{day4}";
                //days[4] = $"{month}-{day5}";
                //days[5] = $"{month}-{day6}";
                //days[6] = $"{month}-{day7}";
                days[0] = day1;
                days[1] = day2;
                days[2] = day3;
                days[3] = day4;
                days[4] = day5;
                days[5] = day6;
                days[6] = day7;
            }
            else
            {
                days[0] = $"Day1";
                days[1] = $"Day2";
                days[2] = $"Day3";
                days[3] = $"Day4";
                days[4] = $"Day5";
                days[5] = $"Day6";
                days[6] = $"Day7";
            }
            var response = _equipTimetablesExcelExporter.ExportToFile(equipTimetablesListDtos, days);
            await MarkInActive(equipTimetablesListDtos);
            return response;
        }
        private async Task MarkInActive(List<GetEquipTimetablesForViewDto> timetables)
        {
            foreach (var timetable in timetables)
            {
                var timetableFromRepo = _equipTimetablesRepository.Get(timetable.EquipTimetables.Id);
                if (timetableFromRepo == null) continue;


                timetableFromRepo.IsActive = false;

                await _equipTimetablesRepository.UpdateAsync(timetableFromRepo);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables)]
        public async Task<PagedResultDto<EquipTimetablesPayPeriodsLookupTableDto>> GetAllPayPeriodsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_payPeriodsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Id.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var payPeriodsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EquipTimetablesPayPeriodsLookupTableDto>();
            foreach (var payPeriods in payPeriodsList)
            {
                lookupTableDtoList.Add(new EquipTimetablesPayPeriodsLookupTableDto
                {
                    Id = payPeriods.Id,
                    DisplayName = payPeriods.StartDate.ToString("MMMM dd, yyyy") + " - " + payPeriods.EndDate.ToString("MMMM dd, yyyy")
                });
            }

            return new PagedResultDto<EquipTimetablesPayPeriodsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables)]
        public async Task<PagedResultDto<EquipTimetablesResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_resourcesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.ResourceNumber != null && e.ResourceNumber.Contains(input.Filter))
                    || (e.Name != null && e.Name.Contains(input.Filter))
               ).Where(x => x.Type.ToLower() == "equipment");


            var totalCount = await query.CountAsync();

            var resourcesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EquipTimetablesResourcesLookupTableDto>();
            foreach (var resources in resourcesList)
            {
                lookupTableDtoList.Add(new EquipTimetablesResourcesLookupTableDto
                {
                    Id = resources.Id,
                    DisplayName = resources.ResourceNumber?.ToString()
                });
            }

            return new PagedResultDto<EquipTimetablesResourcesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables)]
        public async Task<PagedResultDto<EquipTimetablesJobPhaseCodesLookupTableDto>> GetAllJobPhaseCodesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobPhaseCodesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.Name != null && e.Name.Contains(input.Filter))
                    || (e.Code != null && e.Code.Contains(input.Filter))
               );

            var totalCount = await query.CountAsync();
            var jobPhaseCodesList = await query
                   .PageBy(input)
                   .ToListAsync();

            var lookupTableDtoList = new List<EquipTimetablesJobPhaseCodesLookupTableDto>();
            foreach (var jobPhaseCodes in jobPhaseCodesList)
            {
                lookupTableDtoList.Add(new EquipTimetablesJobPhaseCodesLookupTableDto
                {
                    Id = jobPhaseCodes.Id,
                    DisplayName = jobPhaseCodes.Code?.ToString()
                });
            }

            return new PagedResultDto<EquipTimetablesJobPhaseCodesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables)]
        public async Task<PagedResultDto<EquipTimetablesJobCategoriesLookupTableDto>> GetAllJobCategoriesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobCategoriesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => (e.Name != null && e.Name.Contains(input.Filter))
                    || (e.Code != null && e.Code.Contains(input.Filter))
               );

            var totalCount = await query.CountAsync();

            var jobCategoriesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EquipTimetablesJobCategoriesLookupTableDto>();
            foreach (var jobCategories in jobCategoriesList)
            {
                lookupTableDtoList.Add(new EquipTimetablesJobCategoriesLookupTableDto
                {
                    Id = jobCategories.Id,
                    DisplayName = jobCategories.Code?.ToString()
                });
            }

            return new PagedResultDto<EquipTimetablesJobCategoriesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_EquipTimetables)]
        public async Task<PagedResultDto<EquipTimetablesJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var jobsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EquipTimetablesJobsLookupTableDto>();
            foreach (var jobs in jobsList)
            {
                lookupTableDtoList.Add(new EquipTimetablesJobsLookupTableDto
                {
                    Id = jobs.Id,
                    DisplayName = jobs.Name?.ToString()
                });
            }

            return new PagedResultDto<EquipTimetablesJobsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
using Nucleus.PayPeriod;
using Nucleus.Resource;
using Nucleus.UnionPayRate;
using Nucleus.Union;
using Nucleus.Address;
using Nucleus.ExpenseType;
using Nucleus.CostType;
using Nucleus.Account;
using Nucleus.Authorization.Users;
using Nucleus.PayType;
using Nucleus.WorkerClasee;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.Timetable.Exporting;
using Nucleus.Timetable.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Nucleus.Timetable
{
    [AbpAuthorize(AppPermissions.Pages_Timetables)]
    public class TimetablesAppService : NucleusAppServiceBase, ITimetablesAppService
    {
        private readonly IRepository<Timetables> _timetablesRepository;
        private readonly ITimetablesExcelExporter _timetablesExcelExporter;
        private readonly IRepository<PayPeriods, int> _lookup_payPeriodsRepository;
        private readonly IRepository<Resources, int> _lookup_resourcesRepository;
        private readonly IRepository<UnionPayRates, int> _lookup_unionPayRatesRepository;
        private readonly IRepository<Unions, int> _lookup_unionsRepository;
        private readonly IRepository<Addresses, int> _lookup_addressesRepository;
        private readonly IRepository<ExpenseTypes, int> _lookup_expenseTypesRepository;
        private readonly IRepository<CostTypes, int> _lookup_costTypesRepository;
        private readonly IRepository<Accounts, int> _lookup_accountsRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<PayTypes, int> _lookup_payTypesRepository;
        private readonly IRepository<WorkerClasees, int> _lookup_workerClaseesRepository;

        public TimetablesAppService(IRepository<Timetables> timetablesRepository, ITimetablesExcelExporter timetablesExcelExporter, IRepository<PayPeriods, int> lookup_payPeriodsRepository, IRepository<Resources, int> lookup_resourcesRepository, IRepository<UnionPayRates, int> lookup_unionPayRatesRepository, IRepository<Unions, int> lookup_unionsRepository, IRepository<Addresses, int> lookup_addressesRepository, IRepository<ExpenseTypes, int> lookup_expenseTypesRepository, IRepository<CostTypes, int> lookup_costTypesRepository, IRepository<Accounts, int> lookup_accountsRepository, IRepository<User, long> lookup_userRepository, IRepository<PayTypes, int> lookup_payTypesRepository, IRepository<WorkerClasees, int> lookup_workerClaseesRepository)
        {
            _timetablesRepository = timetablesRepository;
            _timetablesExcelExporter = timetablesExcelExporter;
            _lookup_payPeriodsRepository = lookup_payPeriodsRepository;
            _lookup_resourcesRepository = lookup_resourcesRepository;
            _lookup_unionPayRatesRepository = lookup_unionPayRatesRepository;
            _lookup_unionsRepository = lookup_unionsRepository;
            _lookup_addressesRepository = lookup_addressesRepository;
            _lookup_expenseTypesRepository = lookup_expenseTypesRepository;
            _lookup_costTypesRepository = lookup_costTypesRepository;
            _lookup_accountsRepository = lookup_accountsRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_payTypesRepository = lookup_payTypesRepository;
            _lookup_workerClaseesRepository = lookup_workerClaseesRepository;

        }
        public async Task<PagedResultDto<dynamic>> GetAllForJob(TimetableFilterRequestDto request)
        {
            var timetables = _timetablesRepository.GetAll()
                            .Where(x => x.CostCode == request.CostCode && x.PeriodDate == request.PayPeriodId)
                            .Include(x => x.AccountsFk)
                            .Include(x => x.CostTypesFk)
                            .Include(x => x.CreatedByFk)
                            .Include(x => x.DescriptionFk)
                            .Include(x => x.PayTypesFk)
                            .Include(x => x.PeriodDateFk)
                            .Include(x => x.RateFk)
                                .ThenInclude(y => y.UnionsFk)
                            .Include(x => x.ResourcesCodeFk)
                            .Include(x => x.StateFk)
                            .Include(x => x.UnionlocalFk);

            var totalCount = await timetables.CountAsync();

            return new PagedResultDto<dynamic>(
                totalCount,
                await timetables.OrderBy(request.Sorting ?? "Id asc")
                            .PageBy(request)
                            .ToListAsync()
            );
        }
        public async Task<PagedResultDto<GetTimetablesForViewDto>> GetAll(GetAllTimetablesInput input)
        {
            try
            {
                var filteredTimetables = _timetablesRepository.GetAll()
                        .Include(e => e.PeriodDateFk)
                        .Include(e => e.ResourcesCodeFk)
                        .Include(e => e.RateFk)
                            .ThenInclude(y => y.UnionsFk)
                        .Include(e => e.UnionlocalFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.DescriptionFk)
                        .Include(e => e.CostTypesFk)
                        .Include(e => e.AccountsFk)
                        .Include(e => e.CreatedByFk)
                        .Include(e => e.PayTypesFk)
                        .Include(e => e.WorkerClaseesFk)
                        .Where(x => x.CostCode.ToLower().StartsWith(input.JobCode) && x.PeriodDate == input.PayPeriodId 
                            /* && x.IsActive */ // Uncomment if only the non-exported data is required. 
                            && ((x.Day1 != null && x.Day1.HasValue && x.Day1.Value > 0)
                            || (x.Day2 != null && x.Day2.HasValue && x.Day2.Value > 0)
                            || (x.Day3 != null && x.Day3.HasValue && x.Day3.Value > 0)
                            || (x.Day4 != null && x.Day4.HasValue && x.Day4.Value > 0)
                            || (x.Day5 != null && x.Day5.HasValue && x.Day5.Value > 0)
                            || (x.Day6 != null && x.Day6.HasValue && x.Day6.Value > 0)
                            || (x.Day7 != null && x.Day7.HasValue && x.Day7.Value > 0)))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CostCode.Contains(input.Filter))
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CostCodeFilter), e => e.CostCode == input.CostCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Wcomp1Filter), e => e.Wcomp1 == input.Wcomp1Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayPeriodsNameFilter), e => e.PeriodDateFk != null && e.PeriodDateFk.Name == input.PayPeriodsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesCodeFk != null && e.ResourcesCodeFk.Name == input.ResourcesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnionPayRatesClassFilter), e => e.RateFk != null && e.RateFk.Class == input.UnionPayRatesClassFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnionsNumberFilter), e => e.UnionlocalFk != null && e.UnionlocalFk.Number == input.UnionsNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressesStateFilter), e => e.StateFk != null && e.StateFk.State == input.AddressesStateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExpenseTypesDescriptionFilter), e => e.DescriptionFk != null && e.DescriptionFk.Description == input.ExpenseTypesDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CostTypesNameFilter), e => e.CostTypesFk != null && e.CostTypesFk.Name == input.CostTypesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountsNameFilter), e => e.AccountsFk != null && e.AccountsFk.Name == input.AccountsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.CreatedByFk != null && e.CreatedByFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayTypesCodeFilter), e => e.PayTypesFk != null && e.PayTypesFk.Code == input.PayTypesCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WorkerClaseesNameFilter), e => e.WorkerClaseesFk != null && e.WorkerClaseesFk.Name == input.WorkerClaseesNameFilter);
                var pagedAndFilteredTimetables = filteredTimetables
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var timetables = from o in pagedAndFilteredTimetables
                                 join o1 in _lookup_payPeriodsRepository.GetAll() on o.PeriodDate equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesCode equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 join o3 in _lookup_unionPayRatesRepository.GetAll() on o.Rate equals o3.Id into j3
                                 from s3 in j3.DefaultIfEmpty()

                                 join o4 in _lookup_unionsRepository.GetAll() on o.Unionlocal equals o4.Id into j4
                                 from s4 in j4.DefaultIfEmpty()

                                 join o5 in _lookup_addressesRepository.GetAll() on o.State equals o5.Id into j5
                                 from s5 in j5.DefaultIfEmpty()

                                 join o6 in _lookup_expenseTypesRepository.GetAll() on o.Description equals o6.Id into j6
                                 from s6 in j6.DefaultIfEmpty()

                                 join o7 in _lookup_costTypesRepository.GetAll() on o.CostTypesId equals o7.Id into j7
                                 from s7 in j7.DefaultIfEmpty()

                                 join o8 in _lookup_accountsRepository.GetAll() on o.AccountsId equals o8.Id into j8
                                 from s8 in j8.DefaultIfEmpty()

                                 join o9 in _lookup_userRepository.GetAll() on o.CreatedBy equals o9.Id into j9
                                 from s9 in j9.DefaultIfEmpty()

                                 join o10 in _lookup_payTypesRepository.GetAll() on o.PayTypesId equals o10.Id into j10
                                 from s10 in j10.DefaultIfEmpty()

                                 join o11 in _lookup_workerClaseesRepository.GetAll() on o.WorkerClaseesId equals o11.Id into j11
                                 from s11 in j11.DefaultIfEmpty()

                                 select new GetTimetablesForViewDto()
                                 {
                                     Timetables = new TimetablesDto
                                     {
                                         Day1 = o.Day1,
                                         Day2 = o.Day2,
                                         Day3 = o.Day3,
                                         Day4 = o.Day4,
                                         Day5 = o.Day5,
                                         Day6 = o.Day6,
                                         Day7 = o.Day7,
                                         Amount = o.Amount,
                                         Multiplier = o.Multiplier,
                                         CreatedOn = o.CreatedOn,
                                         IsActive = o.IsActive,
                                         CostCode = o.CostCode,
                                         Wcomp1 = o.Wcomp1,
                                         Id = o.Id
                                     },
                                     PayPeriodsName = s1 == null || s1.Name == null ? "" : s1.Name,
                                     ResourcesName = s2 == null || s2.ResourceNumber == null ? "" : s2.ResourceNumber,
                                     UnionPayRatesClass = s3 == null || s3.Class == null ? "" : s3.Class,
                                     UnionPayRatesPerHour = s3 == null ? 0 : s3.Perhour,
                                     UnionsNumber = s4 == null || s4.LocalNumber == null ? "" : s4.LocalNumber, // s4 == null || s4.Number == null ? "" : s4.Number,
                                     UnionsLocalNumber = s4 == null || s4.LocalNumber == null ? "" : s4.LocalNumber,
                                     AddressesState = s5 == null || s5.State == null ? "" : s5.State,
                                     ExpenseTypesDescription = s6 == null || s6.Description == null ? "" : s6.Description,
                                     CostTypesName = s7 == null || s7.Name == null ? "" : s7.Code == null ? s7.Name : s7.Name + " - " + s7.Code,
                                     AccountsName = s8 == null || s8.Name == null ? "" : s8.Name,
                                     UserName = s9 == null || s9.Name == null ? "" : s9.Name,
                                     PayTypesCode = s10 == null || s10.Code == null ? "" : s10.Code,
                                     WorkerClaseesName = s11 == null || s11.Name == null ? "" : s11.Name
                                 };

                var totalCount = await filteredTimetables.CountAsync();

                return new PagedResultDto<GetTimetablesForViewDto>(
                    totalCount,
                    await timetables
                    .OrderByDescending(x=>x.Timetables.IsActive)
                        .ThenBy(x => x.ResourcesName)
                            .ThenBy(x => x.PayTypesCode)
                    .ToListAsync()
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetTimetablesForViewDto> GetTimetablesForView(int id)
        {
            var timetables = await _timetablesRepository.GetAsync(id);

            var output = new GetTimetablesForViewDto { Timetables = ObjectMapper.Map<TimetablesDto>(timetables) };

            if (output.Timetables.PeriodDate != null)
            {
                var _lookupPayPeriods = await _lookup_payPeriodsRepository.FirstOrDefaultAsync((int)output.Timetables.PeriodDate);
                output.PayPeriodsName = _lookupPayPeriods?.Name?.ToString();
            }

            if (output.Timetables.ResourcesCode != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.Timetables.ResourcesCode);
                output.ResourcesName = _lookupResources?.Name?.ToString();
            }

            if (output.Timetables.Rate != null)
            {
                var _lookupUnionPayRates = await _lookup_unionPayRatesRepository.FirstOrDefaultAsync((int)output.Timetables.Rate);
                output.UnionPayRatesClass = _lookupUnionPayRates?.Class?.ToString();
            }

            if (output.Timetables.Unionlocal != null)
            {
                var _lookupUnions = await _lookup_unionsRepository.FirstOrDefaultAsync((int)output.Timetables.Unionlocal);
                output.UnionsNumber = _lookupUnions?.Number?.ToString();
            }

            if (output.Timetables.State != null)
            {
                var _lookupAddresses = await _lookup_addressesRepository.FirstOrDefaultAsync((int)output.Timetables.State);
                output.AddressesState = _lookupAddresses?.State?.ToString();
            }

            if (output.Timetables.Description != null)
            {
                var _lookupExpenseTypes = await _lookup_expenseTypesRepository.FirstOrDefaultAsync((int)output.Timetables.Description);
                output.ExpenseTypesDescription = _lookupExpenseTypes?.Description?.ToString();
            }

            if (output.Timetables.CostTypesId != null)
            {
                var _lookupCostTypes = await _lookup_costTypesRepository.FirstOrDefaultAsync((int)output.Timetables.CostTypesId);
                output.CostTypesName = _lookupCostTypes?.Name?.ToString();
            }

            if (output.Timetables.AccountsId != null)
            {
                var _lookupAccounts = await _lookup_accountsRepository.FirstOrDefaultAsync((int)output.Timetables.AccountsId);
                output.AccountsName = _lookupAccounts?.Name?.ToString();
            }

            if (output.Timetables.CreatedBy != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Timetables.CreatedBy);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Timetables.PayTypesId != null)
            {
                var _lookupPayTypes = await _lookup_payTypesRepository.FirstOrDefaultAsync((int)output.Timetables.PayTypesId);
                output.PayTypesCode = _lookupPayTypes?.Code?.ToString();
            }

            if (output.Timetables.WorkerClaseesId != null)
            {
                var _lookupWorkerClasees = await _lookup_workerClaseesRepository.FirstOrDefaultAsync((int)output.Timetables.WorkerClaseesId);
                output.WorkerClaseesName = _lookupWorkerClasees?.Name?.ToString();
            }

            return output;
        }
        

        [AbpAuthorize(AppPermissions.Pages_Timetables_Edit)]
        public async Task<GetTimetablesForEditOutput> GetTimetablesForEdit(EntityDto input)
        {
            var timetables = await _timetablesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTimetablesForEditOutput { Timetables = ObjectMapper.Map<CreateOrEditTimetablesDto>(timetables) };

            if (output.Timetables.PeriodDate != null)
            {
                var _lookupPayPeriods = await _lookup_payPeriodsRepository.FirstOrDefaultAsync((int)output.Timetables.PeriodDate);
                output.PayPeriodsName = _lookupPayPeriods?.Name?.ToString();
            }

            if (output.Timetables.ResourcesCode != null)
            {
                var _lookupResources = await _lookup_resourcesRepository.FirstOrDefaultAsync((int)output.Timetables.ResourcesCode);
                output.ResourcesName = _lookupResources?.Name?.ToString();
            }

            if (output.Timetables.Rate != null)
            {
                var _lookupUnionPayRates = await _lookup_unionPayRatesRepository.FirstOrDefaultAsync((int)output.Timetables.Rate);
                if (_lookupUnionPayRates != null)
                    output.UnionPayRatesClass = $"{_lookupUnionPayRates.Perhour}";
                else
                    output.UnionPayRatesClass = "";
            }

            if (output.Timetables.Unionlocal != null)
            {
                var _lookupUnions = await _lookup_unionsRepository.FirstOrDefaultAsync((int)output.Timetables.Unionlocal);
                output.UnionsNumber = _lookupUnions?.LocalNumber?.ToString();
            }

            if (output.Timetables.State != null)
            {
                var _lookupAddresses = await _lookup_addressesRepository.FirstOrDefaultAsync((int)output.Timetables.State);
                output.AddressesState = _lookupAddresses?.State?.ToString();
            }

            if (output.Timetables.Description != null)
            {
                var _lookupExpenseTypes = await _lookup_expenseTypesRepository.FirstOrDefaultAsync((int)output.Timetables.Description);
                output.ExpenseTypesDescription = _lookupExpenseTypes?.Description?.ToString();
            }

            if (output.Timetables.CostTypesId != null)
            {
                var _lookupCostTypes = await _lookup_costTypesRepository.FirstOrDefaultAsync((int)output.Timetables.CostTypesId);
                output.CostTypesName = _lookupCostTypes?.Name?.ToString();
            }

            if (output.Timetables.AccountsId != null)
            {
                var _lookupAccounts = await _lookup_accountsRepository.FirstOrDefaultAsync((int)output.Timetables.AccountsId);
                output.AccountsName = _lookupAccounts?.Name?.ToString();
            }

            if (output.Timetables.CreatedBy != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Timetables.CreatedBy);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Timetables.PayTypesId != null)
            {
                var _lookupPayTypes = await _lookup_payTypesRepository.FirstOrDefaultAsync((int)output.Timetables.PayTypesId);
                output.PayTypesCode = _lookupPayTypes?.Code?.ToString();
            }

            if (output.Timetables.WorkerClaseesId != null)
            {
                var _lookupWorkerClasees = await _lookup_workerClaseesRepository.FirstOrDefaultAsync((int)output.Timetables.WorkerClaseesId);
                output.WorkerClaseesName = _lookupWorkerClasees?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTimetablesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Timetables_Create)]
        protected virtual async Task Create(CreateOrEditTimetablesDto input)
        {
            var timetables = ObjectMapper.Map<Timetables>(input);

            await _timetablesRepository.InsertAsync(timetables);
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables_Edit)]
        protected virtual async Task Update(CreateOrEditTimetablesDto input)
        {
            var timetables = await _timetablesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, timetables);
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _timetablesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTimetablesToExcel(GetAllTimetablesForExcelInput input)
        {
            //input.JobCode = "AEP190013";
            //input.PayPeriodId = 3;
            var filteredTimetables = _timetablesRepository.GetAll()
                        .Include(e => e.PeriodDateFk)
                        .Include(e => e.ResourcesCodeFk)
                        .Include(e => e.RateFk)
                        .Include(e => e.UnionlocalFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.DescriptionFk)
                        .Include(e => e.CostTypesFk)
                        .Include(e => e.AccountsFk)
                        .Include(e => e.CreatedByFk)
                        .Include(e => e.PayTypesFk)
                        .Include(e => e.WorkerClaseesFk)
                        .Where(x => x.CostCode.ToLower().StartsWith(input.JobCode) && x.PeriodDate == input.PayPeriodId /*&& x.IsActive*/
                            && ((x.Day1 != null && x.Day1.HasValue && x.Day1.Value > 0)
                            || (x.Day2 != null && x.Day2.HasValue && x.Day2.Value > 0)
                            || (x.Day3 != null && x.Day3.HasValue && x.Day3.Value > 0)
                            || (x.Day4 != null && x.Day4.HasValue && x.Day4.Value > 0)
                            || (x.Day5 != null && x.Day5.HasValue && x.Day5.Value > 0)
                            || (x.Day6 != null && x.Day6.HasValue && x.Day6.Value > 0)
                            || (x.Day7 != null && x.Day7.HasValue && x.Day7.Value > 0)))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CostCode.Contains(input.Filter))
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CostCodeFilter), e => e.CostCode == input.CostCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayPeriodsNameFilter), e => e.PeriodDateFk != null && e.PeriodDateFk.Name == input.PayPeriodsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ResourcesNameFilter), e => e.ResourcesCodeFk != null && e.ResourcesCodeFk.Name == input.ResourcesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnionPayRatesClassFilter), e => e.RateFk != null && e.RateFk.Class == input.UnionPayRatesClassFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UnionsNumberFilter), e => e.UnionlocalFk != null && e.UnionlocalFk.Number == input.UnionsNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressesStateFilter), e => e.StateFk != null && e.StateFk.State == input.AddressesStateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExpenseTypesDescriptionFilter), e => e.DescriptionFk != null && e.DescriptionFk.Description == input.ExpenseTypesDescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CostTypesNameFilter), e => e.CostTypesFk != null && e.CostTypesFk.Name == input.CostTypesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountsNameFilter), e => e.AccountsFk != null && e.AccountsFk.Name == input.AccountsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.CreatedByFk != null && e.CreatedByFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayTypesCodeFilter), e => e.PayTypesFk != null && e.PayTypesFk.Code == input.PayTypesCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WorkerClaseesNameFilter), e => e.WorkerClaseesFk != null && e.WorkerClaseesFk.Name == input.WorkerClaseesNameFilter);

            var query = (from o in filteredTimetables
                         join o1 in _lookup_payPeriodsRepository.GetAll() on o.PeriodDate equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_resourcesRepository.GetAll() on o.ResourcesCode equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_unionPayRatesRepository.GetAll() on o.Rate equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_unionsRepository.GetAll() on o.Unionlocal equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_addressesRepository.GetAll() on o.State equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_expenseTypesRepository.GetAll() on o.Description equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_costTypesRepository.GetAll() on o.CostTypesId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         join o8 in _lookup_accountsRepository.GetAll() on o.AccountsId equals o8.Id into j8
                         from s8 in j8.DefaultIfEmpty()

                         join o9 in _lookup_userRepository.GetAll() on o.CreatedBy equals o9.Id into j9
                         from s9 in j9.DefaultIfEmpty()

                         join o10 in _lookup_payTypesRepository.GetAll() on o.PayTypesId equals o10.Id into j10
                         from s10 in j10.DefaultIfEmpty()

                         join o11 in _lookup_workerClaseesRepository.GetAll() on o.WorkerClaseesId equals o11.Id into j11
                         from s11 in j11.DefaultIfEmpty()

                         select new GetTimetablesForViewDto()
                         {
                             Timetables = new TimetablesDto
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
                                 CostCode = o.CostCode,
                                 Multiplier = o.Multiplier,
                                 Wcomp1 = o.Wcomp1,
                                 Id = o.Id
                             },

                             PayPeriodsName = s1 == null || s1.Name == null ? "" : s1.Name,
                             ResourcesName = s2 == null || s2.ResourceNumber == null ? "" : s2.ResourceNumber,
                             UnionPayRatesClass = s3 == null || s3.Class == null ? "" : s3.Class,
                             UnionPayRatesPerHour = s3 == null ? 0 : s3.Perhour,
                             UnionsNumber = s4 == null || s4.LocalNumber == null ? "" : s4.LocalNumber, // s4 == null || s4.Number == null ? "" : s4.Number,
                             UnionsLocalNumber = s4 == null || s4.LocalNumber == null ? "" : s4.LocalNumber,
                             AddressesState = s5 == null || s5.State == null ? "" : s5.State,
                             ExpenseTypesDescription = s6 == null || s6.Description == null ? "" : s6.Description,
                             CostTypesName = s7 == null || s7.Name == null ? "" : s7.Code == null ? s7.Name : s7.Name + " - " + s7.Code,
                             AccountsName = s8 == null || s8.Name == null ? "" : s8.Name,
                             UserName = s9 == null || s9.Name == null ? "" : s9.Name,
                             PayTypesCode = s10 == null || s10.Code == null ? "" : s10.Code,
                             WorkerClaseesName = s11 == null || s11.Name == null ? "" : s11.Name
                         });

            var timetablesListDtos = await query
                    .OrderBy(x => x.ResourcesName)
                        .ThenBy(x => x.PayTypesCode)
                    .ToListAsync();

            var payPeriod = _lookup_payPeriodsRepository.GetAll().Where(x => x.Id == input.PayPeriodId).FirstOrDefault();
            var days = new string[7];

            if (payPeriod != null)
            {


                var day1 = payPeriod.StartDate.ToString("dd");
                var day2 = payPeriod.StartDate.AddDays(1).ToString("dd");
                var day3 = payPeriod.StartDate.AddDays(2).ToString("dd");
                var day4 = payPeriod.StartDate.AddDays(3).ToString("dd");
                var day5 = payPeriod.StartDate.AddDays(4).ToString("dd");
                var day6 = payPeriod.StartDate.AddDays(5).ToString("dd");
                var day7 = payPeriod.StartDate.AddDays(6).ToString("dd");
                var month = payPeriod.StartDate.ToString("MMM");
                days[0] = $"{month}-{day1}";
                days[1] = $"{month}-{day2}";
                days[2] = $"{month}-{day3}";
                days[3] = $"{month}-{day4}";
                days[4] = $"{month}-{day5}";
                days[5] = $"{month}-{day6}";
                days[6] = $"{month}-{day7}";


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

            var file = _timetablesExcelExporter.ExportToFile(timetablesListDtos, days);

            await MarkInActive(timetablesListDtos);

            return file;
        }

        private async Task MarkInActive(List<GetTimetablesForViewDto> timetables)
        {
            foreach (var timetable in timetables)
            {
                var timetableFromRepo = _timetablesRepository.Get(timetable.Timetables.Id);
                if (timetableFromRepo == null) continue;


                timetableFromRepo.IsActive = false;

                await _timetablesRepository.UpdateAsync(timetableFromRepo);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesPayPeriodsLookupTableDto>> GetAllPayPeriodsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_payPeriodsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var payPeriodsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesPayPeriodsLookupTableDto>();
            foreach (var payPeriods in payPeriodsList)
            {
                lookupTableDtoList.Add(new TimetablesPayPeriodsLookupTableDto
                {
                    Id = payPeriods.Id,
                    DisplayName = payPeriods.Name?.ToString()
                });
            }

            return new PagedResultDto<TimetablesPayPeriodsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesResourcesLookupTableDto>> GetAllResourcesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_resourcesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var resourcesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesResourcesLookupTableDto>();
            foreach (var resources in resourcesList)
            {
                lookupTableDtoList.Add(new TimetablesResourcesLookupTableDto
                {
                    Id = resources.Id,
                    DisplayName = resources.Name?.ToString()
                });
            }

            return new PagedResultDto<TimetablesResourcesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<UnionPayRate.Dtos.UnionPayRatesDto> GetUnionPayRate(int unionsId, int classId)
        {
            var workerClass = _lookup_workerClaseesRepository.GetAll().Where(x => x.Id == classId).FirstOrDefault();
            
            if(workerClass == null)
            {
                return null;
            }
            var className = workerClass.Name;
            if (className != null) className = className.Trim();
            else return null;
            try
            {
                var payRate = await _lookup_unionPayRatesRepository.GetAll()
                   .Where(x => 
                        x.UnionsId == unionsId && x.Class != null && x.Class.Trim() == className)
                   .FirstOrDefaultAsync();

                if (payRate == null) return null;

                var unionPayRate = new UnionPayRate.Dtos.UnionPayRatesDto
                {
                    Class = payRate.Class,
                    Dedtype = payRate.Dedtype,
                    Id = payRate.Id,
                    Perhour = payRate.Perhour,
                    UnionsId = payRate.UnionsId
                };


                return unionPayRate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesUnionPayRatesLookupTableDto>> GetAllUnionPayRatesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_unionPayRatesRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Class != null && e.Class.Contains(input.Filter));

            var totalCount = await query.CountAsync();

            var unionPayRatesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesUnionPayRatesLookupTableDto>();
            foreach (var unionPayRates in unionPayRatesList)
            {
                lookupTableDtoList.Add(new TimetablesUnionPayRatesLookupTableDto
                {
                    Id = unionPayRates.Id,
                    DisplayName = $"{unionPayRates.Perhour}"
                });
            }

            return new PagedResultDto<TimetablesUnionPayRatesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesUnionsLookupTableDto>> GetAllUnionsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_unionsRepository.GetAll().Where(x=>x.Number.ToLower() != "exempt")
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => (e.LocalNumber != null && e.LocalNumber.Contains(input.Filter))
                || (e.Number != null && e.Number.Contains(input.Filter)));

            var totalCount = await query.CountAsync();

            var unionsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesUnionsLookupTableDto>();
            foreach (var unions in unionsList)
            {
                lookupTableDtoList.Add(new TimetablesUnionsLookupTableDto
                {
                    Id = unions.Id,
                    DisplayName = $"{unions.LocalNumber}"
                });
            }

            return new PagedResultDto<TimetablesUnionsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesAddressesLookupTableDto>> GetAllAddressesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_addressesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.State != null && e.State.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var addressesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesAddressesLookupTableDto>();
            foreach (var addresses in addressesList)
            {
                lookupTableDtoList.Add(new TimetablesAddressesLookupTableDto
                {
                    Id = addresses.Id,
                    DisplayName = addresses.State?.ToString()
                });
            }

            return new PagedResultDto<TimetablesAddressesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesExpenseTypesLookupTableDto>> GetAllExpenseTypesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_expenseTypesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Description != null && e.Description.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var expenseTypesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesExpenseTypesLookupTableDto>();
            foreach (var expenseTypes in expenseTypesList)
            {
                lookupTableDtoList.Add(new TimetablesExpenseTypesLookupTableDto
                {
                    Id = expenseTypes.Id,
                    DisplayName = expenseTypes.Description?.ToString()
                });
            }

            return new PagedResultDto<TimetablesExpenseTypesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesCostTypesLookupTableDto>> GetAllCostTypesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_costTypesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var costTypesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesCostTypesLookupTableDto>();
            foreach (var costTypes in costTypesList)
            {
                lookupTableDtoList.Add(new TimetablesCostTypesLookupTableDto
                {
                    Id = costTypes.Id,
                    DisplayName = costTypes.Name?.ToString()
                });
            }

            return new PagedResultDto<TimetablesCostTypesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesAccountsLookupTableDto>> GetAllAccountsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_accountsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var accountsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesAccountsLookupTableDto>();
            foreach (var accounts in accountsList)
            {
                lookupTableDtoList.Add(new TimetablesAccountsLookupTableDto
                {
                    Id = accounts.Id,
                    DisplayName = accounts.Name?.ToString()
                });
            }

            return new PagedResultDto<TimetablesAccountsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new TimetablesUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<TimetablesUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesPayTypesLookupTableDto>> GetAllPayTypesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_payTypesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code != null && e.Code.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var payTypesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesPayTypesLookupTableDto>();
            foreach (var payTypes in payTypesList)
            {
                lookupTableDtoList.Add(new TimetablesPayTypesLookupTableDto
                {
                    Id = payTypes.Id,
                    DisplayName = $"{payTypes.Code} | {payTypes.Multiplier.ToString().Trim()}"
                });
            }

            return new PagedResultDto<TimetablesPayTypesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Timetables)]
        public async Task<PagedResultDto<TimetablesWorkerClaseesLookupTableDto>> GetAllWorkerClaseesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_workerClaseesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var workerClaseesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TimetablesWorkerClaseesLookupTableDto>();
            foreach (var workerClasees in workerClaseesList)
            {
                lookupTableDtoList.Add(new TimetablesWorkerClaseesLookupTableDto
                {
                    Id = workerClasees.Id,
                    DisplayName = workerClasees.Name?.ToString()
                });
            }

            return new PagedResultDto<TimetablesWorkerClaseesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
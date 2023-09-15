using Nucleus.Timesheet;
using Nucleus.Status;
using Nucleus.Job;
using Nucleus.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Nucleus.StatusUpdate.Exporting;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.Dto;
using Abp.Application.Services.Dto;
using Nucleus.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Nucleus.PayPeriod;
using Microsoft.AspNetCore.Mvc;
using Abp.Runtime.Session;
using Nucleus.Resource;
using Nucleus.ShiftResource;
using Nucleus.UnionPayRate;
using Nucleus.Union;
using Nucleus.Timetable;
using Nucleus.CostType;
using Nucleus.Account;
using Nucleus.ShiftExpense;
using Nucleus.JobUnion;
using Nucleus.Shift;
using Nucleus.EmployeeUnion;
using System.Data.Odbc;
using System.Text;
using Microsoft.Data.SqlClient;
using Nucleus.ResourceWorkerInfo;
using Nucleus.PayperiodHistory;
using Nucleus.EquipTimetable;

namespace Nucleus.StatusUpdate
{
    [AbpAuthorize(AppPermissions.Pages_StatusUpdates)]
    public class StatusUpdatesAppService : NucleusAppServiceBase, IStatusUpdatesAppService
    {
        private string OdbcConnection = @"Driver={ComputerEase};DBQ=C:\Users\Administrator\Downloads\ComputerEase(1)\ComputerEase(1)\data\0;uid=STSHULL;pwd=Lineman1;";
        private string SqlConnectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=BBCNucleus;Data Source=.";

        private readonly IRepository<StatusUpdates> _statusUpdatesRepository;
        private readonly IStatusUpdatesExcelExporter _statusUpdatesExcelExporter;
        private readonly IRepository<Timesheets, int> _lookup_timesheetsRepository;
        private readonly IRepository<Statuses, int> _statusesRepository;
        private readonly IRepository<Jobs, int> _jobsRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<PayPeriods, int> _payPeriodsRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<CostTypes, int> _costTypesRepository;
        private readonly IRepository<Resources, int> _resourcesRepository;
        private readonly IRepository<ShiftResources, int> _shiftResourcesRepository;
        private readonly IRepository<UnionPayRates, int> _unionPayRatesRepository;
        private readonly IRepository<Unions, int> _unionsRepository;
        private readonly IRepository<Timetables> _timetablesRepository;
        private readonly IRepository<Accounts, int> _accountsRepository;
        private readonly IRepository<ShiftExpenses, int> _shiftExpenseRepository;
        private readonly IRepository<JobUnions, int> _jobsUnioinsRepository;
        private readonly IRepository<WorkerClasee.WorkerClasees, int> _workerClassesRepository;
        private readonly IRepository<Shifts, int> _shiftsRepository;
        private readonly IRepository<PayType.PayTypes, int> _payTypesRepository;
        private readonly IRepository<EmployeeUnions, int> _employeeUnionsRepository;
        private readonly IRepository<ResourceWorkerInfos, int> _resourceWorkerInfosRepository;
        private readonly IRepository<PayperiodHistories, int> _payPeriodHistoryRepository;
        private readonly IRepository<EquipTimetables, int> _equipTimeTablesRepository;
        public StatusUpdatesAppService(IRepository<PayPeriods, int> lookup_payPeriodsRepository, IRepository<StatusUpdates> statusUpdatesRepository, IStatusUpdatesExcelExporter statusUpdatesExcelExporter, IRepository<Timesheets, int> lookup_timesheetsRepository, IRepository<Statuses, int> lookup_statusesRepository, IRepository<Jobs, int> lookup_jobsRepository, IRepository<User, long> lookup_userRepository,
            IAbpSession abpSession,
            IRepository<Unions, int> lookup_unionsRepository,
            IRepository<UnionPayRates, int> lookup_unionPayRatesRepository,
            IRepository<ShiftResources, int> lookup_shiftResourcesRepository,
            IRepository<Resources, int> lookup_resourcesRepository,
            IRepository<Timetables> timetablesRepository,
            IRepository<CostTypes, int> costTypesRepository,
            IRepository<Accounts, int> accountsRepository,
            IRepository<ShiftExpenses, int> shiftExpenseRepository,
            IRepository<JobUnions, int> jobUnioinsRepository,
            IRepository<Shifts, int> shiftsRepository,
            IRepository<PayType.PayTypes, int> payTypesRepository,
            IRepository<EmployeeUnions, int> employeeUnionsRepository,
            IRepository<WorkerClasee.WorkerClasees, int> workerClassesRepository,
            IRepository<ResourceWorkerInfos, int> resourceWorkerInfosRepository,
            IRepository<PayperiodHistories, int> payPeriodHistoryRepository,
            IRepository<EquipTimetables, int> equipTimeTablesRepository
            )
        {
            _statusUpdatesRepository = statusUpdatesRepository;
            _statusUpdatesExcelExporter = statusUpdatesExcelExporter;
            _lookup_timesheetsRepository = lookup_timesheetsRepository;
            _statusesRepository = lookup_statusesRepository;
            _jobsRepository = lookup_jobsRepository;
            _userRepository = lookup_userRepository;
            _payPeriodsRepository = lookup_payPeriodsRepository;
            _abpSession = abpSession;
            _shiftsRepository = shiftsRepository;
            _resourcesRepository = lookup_resourcesRepository;
            _shiftResourcesRepository = lookup_shiftResourcesRepository;
            _unionPayRatesRepository = lookup_unionPayRatesRepository;
            _unionsRepository = lookup_unionsRepository;
            _timetablesRepository = timetablesRepository;
            _costTypesRepository = costTypesRepository;
            _accountsRepository = accountsRepository;
            _shiftExpenseRepository = shiftExpenseRepository;
            _jobsUnioinsRepository = jobUnioinsRepository;
            _employeeUnionsRepository = employeeUnionsRepository;
            _payTypesRepository = payTypesRepository;
            _workerClassesRepository = workerClassesRepository;
            _resourceWorkerInfosRepository = resourceWorkerInfosRepository;
            _payPeriodHistoryRepository = payPeriodHistoryRepository;
            _equipTimeTablesRepository = equipTimeTablesRepository;
        }

        public async Task<dynamic> CeDataRefresh()
        {
            var startTime = DateTime.Now;

             var rAJCCAT = await GetFromTableAndInsert("JCCAT", new string[] { "SEQUENCE", "JOBNUM", "PHASENUM", "CATNUM", "NAME" });
             var rAJCUNION = await GetFromTableAndInsert("JCUNION", new string[] { "JOBNUM", "UNIONNUM", "UNIONLOCAL" });
            var rAPRCLASS = await GetFromTableAndInsert("PRCLASS", new string[] { "UNIONNUM", "CLASS", "NAME" });
            var rAJCJOB = await GetFromTableAndInsert("JCJOB", new string[] { "JOBNUM", "CLASS", "STATE", "LOCALITY", "CLOSED", "NAME" });
            var rAPREMPLOYEE = await GetFromTableAndInsert("PREMPLOYEE", new string[] { "EMPNUM", "NAME", "UNIONNUM", "UNIONLOCAL", "CLASS",
                "WCOMPNUM1", "LASTNAME", "FIRSTNAME", "STATUS", "PAYRATE", "SERIALNUM" });

            var rAPRDEDRATE = await GetFromTableAndInsert("PRDEDRATE", new string[] { "UNIONLOCAL", "UNIONNUM", "CLASS", "DEDTYPE", "PERHR" });

            var rAEQUIPMENT = await GetFromTableAndInsert("EQUIPMENT", new string[] { "EQUIPNUM", "NAME" });
            var rAECCOST = await GetFromTableAndInsert("ECCOST", new string[] { "EQUIPNUM", "CODENUM", "ESTHOURLY" });

            var executionTime = (DateTime.Now - startTime).Seconds;

            return new
            {
                Status = "Success",
                Seconds = executionTime,
                JCCAT = $"{rAJCCAT} rows inserted.",
                JCUNION = $"{rAJCUNION} rows inserted.",
                PRCLASS = $"{rAPRCLASS} rows inserted.",
                JCJOB = $"{rAJCJOB} rows inserted.",
                PREMPLOYEE = $"{rAPREMPLOYEE} rows inserted.",
                PRDEDRATE = $"{rAPRDEDRATE} rows inserted.",
                EQUIPMENT = $"{rAEQUIPMENT} rows inserted.",
                ECCOST = $"{rAECCOST} rows inserted."
            };
        }

        public async Task<int> GetFromTableAndInsert(string tableName, string[] columnNames)
        {
            var rA = 0;
            using (var con = new OdbcConnection(OdbcConnection))
            {
                var query = "SELECT * FROM " + tableName;

                using (var cmd = new OdbcCommand(query, con))
                {
                    await con.OpenAsync();

                    var reader = await cmd.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var values = new string[columnNames.Length];
                        var count = 0;
                        foreach (var column in columnNames)
                        {
                            values[count++] = reader[column].ToString();
                        }

                        rA += await InsertIfNotExist(tableName, columnNames, values);
                    }

                    await con.CloseAsync();
                }
            }
            return rA;
        }

        public string GenerateSelectCommand(string tableName, string[] columnNames, string[] columnValues)
        {
            var sb = new StringBuilder($"SELECT * FROM {tableName} ");
            if (columnNames.Length > 0) sb.Append(" WHERE ");
            for (int i = 0; i < columnNames.Length; i++)
            {
                if (!string.IsNullOrEmpty(columnValues[i]))
                {
                    sb.Append($" {columnNames[i]} = '{columnValues[i].Replace("'", "''")}' ");
                }
                else
                {
                    sb.Append($" {columnNames[i]} is NULL ");
                }
                if (i + 1 < columnNames.Length)
                {
                    sb.Append(" AND ");
                }
            }
            return sb.ToString();
        }
        public string GenerateInsertCommand(string tableName, string[] columnNames, string[] columnValues)
        {
            var sb = new StringBuilder($"INSERT INTO {tableName} (");
            // if (columnNames.Length > 0) sb.Append(" WHERE ");
            for (int i = 0; i < columnNames.Length; i++)
            {

                sb.Append($"{columnNames[i]}");
                if (i + 1 < columnNames.Length)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(") VALUES (");

            for (int i = 0; i < columnNames.Length; i++)
            {
                sb.Append($"@{columnNames[i]}");
                if (i + 1 < columnNames.Length)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(")");

            return sb.ToString();
        }
        public SqlParameter[] GenerateSqlParameters(string[] columnNames, string[] values)
        {
            var sqlParams = new SqlParameter[columnNames.Length];

            for (int i = 0; i < columnNames.Length; i++)
            {
                if (string.IsNullOrEmpty(values[i]))
                {
                    var sqlParam = new SqlParameter($"@{columnNames[i]}", DBNull.Value);
                    sqlParams[i] = sqlParam;
                }
                else
                {
                    var sqlParam = new SqlParameter($"@{columnNames[i]}", values[i]);
                    sqlParams[i] = sqlParam;
                }
            }

            return sqlParams;
        }
        public async Task<int> InsertIfNotExist(string tableName, string[] columnNames, string[] values)
        {
            var selectCommand = GenerateSelectCommand(tableName, columnNames, values);

            if (!(await HasRows(selectCommand)))
            {
                var insertCommand = GenerateInsertCommand(tableName, columnNames, values);

                var sqlParams = GenerateSqlParameters(columnNames, values);
                return await RunCommandWithParameters(insertCommand, sqlParams);
            }
            return 0;
        }
        public async Task<int> RunCommandWithParameters(string command, SqlParameter[] sqlParams)
        {
            int rA = 0;
            try
            {
                using (var con = new SqlConnection(SqlConnectionString))
                {
                    using (var cmd = new SqlCommand(command, con))
                    {
                        cmd.Parameters.AddRange(sqlParams);

                        await con.OpenAsync();

                        rA = await cmd.ExecuteNonQueryAsync();

                        await con.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return rA;
        }
        private async Task<bool> HasRows(string command)
        {
            bool hasRows = false;
            try
            {
                using (var con = new SqlConnection(SqlConnectionString))
                {
                    using (var cmd = new SqlCommand(command, con))
                    {
                        await con.OpenAsync();

                        hasRows = (await cmd.ExecuteReaderAsync()).HasRows;

                        await con.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hasRows;
        }
        public async Task InsertIntoPayPeriod(string startDate, string endDate)
        {
            var dtStart = DateTime.Parse(startDate);
            var dtEnd = DateTime.Parse(endDate);
            if (!PayPeriodExists(dtStart, dtEnd))
            {
                var activePayPeriod = _payPeriodsRepository.GetAll().Where(x => x.IsActive).FirstOrDefault();

                if (activePayPeriod != null) // check if an active payperiod exists
                {
                    // then get the only record in payperiod history table 
                    var currentPayPeriodHistory = _payPeriodHistoryRepository.GetAll().FirstOrDefault();
                    if (currentPayPeriodHistory == null) // if it is null insert a new entry
                    {
                        currentPayPeriodHistory = new PayperiodHistories
                        {
                            active = true,
                            PayPeriodsId = activePayPeriod.Id,
                            period = activePayPeriod.Name
                        };
                        await _payPeriodHistoryRepository.InsertAsync(currentPayPeriodHistory);
                    }
                    else // otherwise, mark the previous entry as active and modify the payperiod id from foreign key.
                    {
                        currentPayPeriodHistory.PayPeriodsId = activePayPeriod.Id;
                        currentPayPeriodHistory.active = true;
                        await _payPeriodHistoryRepository.UpdateAsync(currentPayPeriodHistory);
                    }
                }
                // Marking Old pay periods inactive 
                foreach (var item in _payPeriodsRepository.GetAll())
                {
                    item.IsActive = false;
                    await _payPeriodsRepository.UpdateAsync(item);
                }
                // Inserting new payperiod as active 
                PayPeriods payPeriod = new PayPeriods
                {
                    StartDate = dtStart.Date,
                    EndDate = dtEnd.Date,
                    IsActive = true
                };

                await _payPeriodsRepository.InsertAsync(payPeriod);
            }
        }
        public async Task<dynamic> LoadNewPayPeriod()
        {
            using (var con = new OdbcConnection(OdbcConnection))
            {
                using (var cmd = new OdbcCommand("SELECT * FROM PRREGISTERMAIN", con))
                {
                    await con.OpenAsync();
                    var odr = await cmd.ExecuteReaderAsync();
                    while (await odr.ReadAsync())
                    {
                        var payrollDate = odr["payrollDate"].ToString();
                        var checkDate = odr["checkdate"].ToString();

                        await InsertIntoPayPeriod(payrollDate, checkDate);
                    }

                    await con.CloseAsync();
                }
            }

            return new
            {
                Success = "Successfully loaded."
            };
        }
        public bool PayPeriodExists(DateTime startDate, DateTime endDate)
        {
            var payPeriod = _payPeriodsRepository.GetAll().Where(x => x.StartDate.Date == startDate.Date && x.EndDate.Date == endDate.Date).FirstOrDefault();

            return payPeriod != null;
        }

        public async Task<PagedResultDto<GetStatusUpdatesForViewDto>> GetAll(GetAllStatusUpdatesInput input)
        {
            try
            {
                var filteredStatusUpdates = _statusUpdatesRepository.GetAll()
                        .Include(e => e.TimesheetsFk)
                        .Include(e => e.NewStatusesFk)
                        .Include(e => e.JobsFk)
                        .Include(e => e.ModifiedByFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinModifiedOnFilter != null, e => e.ModifiedOn >= input.MinModifiedOnFilter)
                        .WhereIf(input.MaxModifiedOnFilter != null, e => e.ModifiedOn <= input.MaxModifiedOnFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinOriginalstatusIdFilter != null, e => e.OriginalstatusId >= input.MinOriginalstatusIdFilter)
                        .WhereIf(input.MaxOriginalstatusIdFilter != null, e => e.OriginalstatusId <= input.MaxOriginalstatusIdFilter)
                        .WhereIf(input.MinActualCreateDateTimeFilter != null, e => e.ActualCreateDateTime >= input.MinActualCreateDateTimeFilter)
                        .WhereIf(input.MaxActualCreateDateTimeFilter != null, e => e.ActualCreateDateTime <= input.MaxActualCreateDateTimeFilter)
                        // .WhereIf(!string.IsNullOrWhiteSpace(input.TimeshetIdsFilter), e => e.TimeshetIds == input.TimeshetIdsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TimesheetsNameFilter), e => e.TimesheetsFk != null && e.TimesheetsFk.Name == input.TimesheetsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusesNameFilter), e => e.NewStatusesFk != null && e.NewStatusesFk.Name == input.StatusesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobsFk != null && e.JobsFk.Name == input.JobsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ModifiedByFk != null && e.ModifiedByFk.Name == input.UserNameFilter);

                var pagedAndFilteredStatusUpdates = filteredStatusUpdates
                    .OrderBy(input.Sorting ?? "id asc")
                    .PageBy(input);

                var statusUpdates = from o in pagedAndFilteredStatusUpdates
                                    join o1 in _lookup_timesheetsRepository.GetAll() on o.TimesheetsId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _statusesRepository.GetAll() on o.NewStatusesId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    join o3 in _jobsRepository.GetAll() on o.JobsId equals o3.Id into j3
                                    from s3 in j3.DefaultIfEmpty()

                                    join o4 in _userRepository.GetAll() on o.ModifiedBy equals o4.Id into j4
                                    from s4 in j4.DefaultIfEmpty()

                                    select new GetStatusUpdatesForViewDto()
                                    {
                                        StatusUpdates = new StatusUpdatesDto
                                        {
                                            ModifiedOn = o.ModifiedOn,
                                            Name = o.Name,
                                            OriginalstatusId = o.OriginalstatusId,
                                            ActualCreateDateTime = o.ActualCreateDateTime,
                                            //TimeshetIds = o.TimeshetIds,
                                            Id = o.Id
                                        },
                                        TimesheetsName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                        StatusesName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                        JobsName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                        UserName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                    };

                var totalCount = await filteredStatusUpdates.CountAsync();

                return new PagedResultDto<GetStatusUpdatesForViewDto>(
                    totalCount,
                    await statusUpdates.ToListAsync()
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<GetStatusUpdatesForViewDto> GetStatusUpdatesForView(int id)
        {
            var statusUpdates = await _statusUpdatesRepository.GetAsync(id);

            var output = new GetStatusUpdatesForViewDto { StatusUpdates = ObjectMapper.Map<StatusUpdatesDto>(statusUpdates) };

            if (output.StatusUpdates.TimesheetsId != null)
            {
                var _lookupTimesheets = await _lookup_timesheetsRepository.FirstOrDefaultAsync((int)output.StatusUpdates.TimesheetsId);
                output.TimesheetsName = _lookupTimesheets?.Name?.ToString();
            }

            if (output.StatusUpdates.NewStatusesId != null)
            {
                var _lookupStatuses = await _statusesRepository.FirstOrDefaultAsync((int)output.StatusUpdates.NewStatusesId);
                output.StatusesName = _lookupStatuses?.Name?.ToString();
            }

            if (output.StatusUpdates.JobsId != null)
            {
                var _lookupJobs = await _jobsRepository.FirstOrDefaultAsync((int)output.StatusUpdates.JobsId);
                output.JobsName = _lookupJobs?.Name?.ToString();
            }

            if (output.StatusUpdates.ModifiedBy != null)
            {
                var _lookupUser = await _userRepository.FirstOrDefaultAsync((long)output.StatusUpdates.ModifiedBy);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StatusUpdates_Edit)]
        public async Task<GetStatusUpdatesForEditOutput> GetStatusUpdatesForEdit(EntityDto input)
        {
            var statusUpdates = await _statusUpdatesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStatusUpdatesForEditOutput { StatusUpdates = ObjectMapper.Map<CreateOrEditStatusUpdatesDto>(statusUpdates) };

            if (output.StatusUpdates.TimesheetsId != null)
            {
                var _lookupTimesheets = await _lookup_timesheetsRepository.FirstOrDefaultAsync((int)output.StatusUpdates.TimesheetsId);
                output.TimesheetsName = _lookupTimesheets?.Name?.ToString();
            }

            if (output.StatusUpdates.NewStatusesId != null)
            {
                var _lookupStatuses = await _statusesRepository.FirstOrDefaultAsync((int)output.StatusUpdates.NewStatusesId);
                output.StatusesName = _lookupStatuses?.Name?.ToString();
            }

            if (output.StatusUpdates.JobsId != null)
            {
                var _lookupJobs = await _jobsRepository.FirstOrDefaultAsync((int)output.StatusUpdates.JobsId);
                output.JobsName = _lookupJobs?.Name?.ToString();
            }

            if (output.StatusUpdates.ModifiedBy != null)
            {
                var _lookupUser = await _userRepository.FirstOrDefaultAsync((long)output.StatusUpdates.ModifiedBy);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStatusUpdatesDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }


            // if(input.NewStatusesId)
        }

        [AbpAuthorize(AppPermissions.Pages_StatusUpdates_Create)]
        protected virtual async Task Create(CreateOrEditStatusUpdatesDto input)
        {
            var statusUpdates = ObjectMapper.Map<StatusUpdates>(input);



            await _statusUpdatesRepository.InsertAsync(statusUpdates);

            if (string.Equals(input.Name, "Submit", StringComparison.OrdinalIgnoreCase))
            {
                await ValidateAndGenerateTimetables(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_StatusUpdates_Edit)]
        protected virtual async Task Update(CreateOrEditStatusUpdatesDto input)
        {
            try
            {
                var statusUpdates = await _statusUpdatesRepository.FirstOrDefaultAsync((int)input.Id);
                ObjectMapper.Map(input, statusUpdates);
                await _statusUpdatesRepository.UpdateAsync(statusUpdates);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task ValidateAndGenerateTimetables(CreateOrEditStatusUpdatesDto input)
        {
            try
            {
                var payPeriod = _payPeriodsRepository.GetAll().Where(x => input.ActualCreateDateTime >= x.StartDate && input.ActualCreateDateTime <= x.EndDate).FirstOrDefault();

                if (payPeriod == null)
                {
                    // No Payperiod found
                    var message = ("PayPeriod does not exists in the current actual date.");
                    return;
                }

                var tShiftResourcesGrouped = _lookup_timesheetsRepository.GetAll()
                    .Include(x => x.StatusesFk)
                    .OrderBy(x => x.CreatedDate)
                    .SelectMany(timesheet => _shiftResourcesRepository.GetAll()
                        .Where(shiftResource => shiftResource.TimesheetsId == timesheet.Id)
                        .Include(shiftResource => shiftResource.ResourcesFk)
                        .Include(shiftResource => shiftResource.PayTypesFk)
                        .Include(shiftResource => shiftResource.ShiftsFk)
                            .ThenInclude(shift => shift.JobsFk)
                            .ThenInclude(job => job.AddressesFk)
                        .Include(shiftResource => shiftResource.ShiftsFk)
                            .ThenInclude(shift => shift.JobsFk)
                            .ThenInclude(job => job.JobClassesFk)
                        .Include(shiftResource => shiftResource.WorkerClaseesFk)
                        .Include(shiftResource => shiftResource.JobPhaseCodesFk)
                        .Include(shiftResource => shiftResource.JobCategoriesFk)
                        ,
                        (timesheet, shiftResource) => new
                        {
                            Timesheet = timesheet,
                            ShiftResource = shiftResource
                        }
                    )
                    .OrderBy(x => x.ShiftResource.TimesheetsId)
                        .ThenBy(x => x.Timesheet.CreatedDate)
                    .ToList()
                    .GroupBy(item => new
                    {
                        item.ShiftResource.TimesheetsId,
                        item.ShiftResource.ResourcesId,
                        item.ShiftResource.JobCategoriesId,
                        item.ShiftResource.JobPhaseCodesId,
                        item.ShiftResource.PayTypesId,
                        item.ShiftResource.ShiftsId,
                        item.ShiftResource.WorkerClaseesId,
                        AddressId = item.ShiftResource.ShiftsFk.JobsFk.AddressesFk?.Id
                    })
                    .Select(group => new
                    {
                        group.Key,
                        TotalHours = group.Sum(item => (item.ShiftResource.HoursWorked.HasValue) ? item.ShiftResource.HoursWorked.Value : 0),
                        ShiftResource = group.First().ShiftResource,
                        Timesheet = group.First().Timesheet,
                        // JobUnions = _jobsUnioinsRepository.GetAll().Where(x => x.JobsId == group.FirstOrDefault().ShiftResource.ShiftsFk.JobsId).ToList()
                    })
                    .Where(x => x.Timesheet.CreatedDate.Value.Date == input.ActualCreateDateTime.Value.Date /* && ( x.CreatedDate >= payPeriod.StartDate 
                            && x.CreatedDate <= payPeriod.EndDate )*/)
                    .ToList();
                if (tShiftResourcesGrouped == null || tShiftResourcesGrouped.Count < 1) return;
                var day = GetDayFromDates(payPeriod, input.ActualCreateDateTime);
                var createdBy = _abpSession.GetUserId();
                var createdOn = input.ActualCreateDateTime;
                // var costTypeId = _costTypesRepository.GetAll().FirstOrDefault()?.Id;
                var accountsId = _accountsRepository.GetAll().FirstOrDefault()?.Id;
                var periodDateId = payPeriod.Id;
                var payTypeIdExpese = _payTypesRepository.GetAll().Where(x => x.Code == "Nontax")?.FirstOrDefault();
                foreach (var tShiftResource in tShiftResourcesGrouped)
                {
                    var timesheetsId = tShiftResource.Key.TimesheetsId;
                    var resourceId = tShiftResource.Key.ResourcesId;
                    var shiftResourceId = tShiftResource.ShiftResource.Id;
                    var payTypeId = tShiftResource.Key.PayTypesId;
                    var workerClassId = tShiftResource.Key.WorkerClaseesId;
                    var jobPhaseCodeId = tShiftResource.Key.JobPhaseCodesId;

                    var jobCategoriesId = tShiftResource.Key.JobCategoriesId;
                    var jobId = tShiftResource.ShiftResource.ShiftsFk.JobsFk.Id;
                    var stateAddressId = tShiftResource.ShiftResource.ShiftsFk?.JobsFk?.AddressesFk?.Id;
                    var shiftExpenses = _shiftExpenseRepository.GetAll()
                        .Where(x => x.ShiftResourcesId == tShiftResource.ShiftResource.Id && x.Amount > 0)
                        .Include(x => x.ExpenseTypesFk)
                        .ToList(); // tShiftResource.ShiftExpenseJobUnions.ToList();
                    var totalHours = tShiftResource.TotalHours;
                    decimal multiplier = 0;
                    decimal.TryParse(tShiftResource.ShiftResource.PayTypesFk?.Multiplier.ToString(), out multiplier);
                    if (tShiftResource.ShiftResource.ResourcesFk.Type.Equals("Employees", StringComparison.OrdinalIgnoreCase))
                    {

                        decimal amount = 0;


                        var costCode = $"{tShiftResource.ShiftResource.ShiftsFk.JobsFk.Code}.{tShiftResource.ShiftResource.JobPhaseCodesFk.Code}.{tShiftResource.ShiftResource.JobCategoriesFk.Code}";
                        var jobCode = tShiftResource.ShiftResource.ShiftsFk.JobsFk.Code;

                        //var jobUnions = tShiftResource.JobUnions.ToList();

                        var workerInfo = _resourceWorkerInfosRepository.GetAll().Where(x => x.ResourcesId == resourceId).Include(x=>x.ResourcesFk).FirstOrDefault();
                        var wcomp1 = workerInfo.Wcomp1;
                        var workerClass = _workerClassesRepository.GetAll().Where(x => x.Id == workerClassId).FirstOrDefault();



                        /*
                        var sjueu = _shiftsRepository.GetAll()
                            .Where(x => x.Id == tShiftResource.Key.ShiftsId)
                            .Include(x => x.JobsFk)
                            .SelectMany(shift => _jobsUnioinsRepository.GetAll()
                            .Include(x => x.JobsFk)
                            .Include(x => x.UnionsFk)
                            .Where(x => x.JobsId == shift.JobsId)
                            ,
                            (shifts, jobUnions) => new
                            {
                                Shifts = shifts,
                                JobUnions = jobUnions
                            })
                            .SelectMany(sj => _unionsRepository.GetAll()
                            .Where(x => x.Id == sj.JobUnions.UnionsId && x.Number.ToUpper() != "EXEMPT")
                            ,
                            (sj, unions) => new
                            {
                                Shifts = sj.Shifts,
                                JobUnions = sj.JobUnions,
                                Unions = unions
                            })
                            //.SelectMany(sju => _employeeUnionsRepository.GetAll()
                            //.Where(x => x.UnionsId == sju.JobUnions.UnionsId && x.ResourcesId == tShiftResource.Key.ResourcesId)
                            //.Include(x => x.ResourcesFk)
                            //.Include(x => x.UnionsFk)
                            //,
                            //(sju, employeeUnions) => new
                            //{
                            //    Shifts = sju.Shifts,
                            //    JobUnions = sju.JobUnions,
                            //    Unions = sju.Unions,
                            //    EmployeeUnions = employeeUnions
                            //})
                            .SelectMany(sjue1 => _unionPayRatesRepository.GetAll()
                            .Where(x =>
                            x.UnionsId == sjue1.Unions.Id
                            && x.Class == _workerClassesRepository.Get(workerClassId.Value).Code)
                            ,
                            (sjue, payRate) => new
                            {
                                Shifts = sjue.Shifts,
                                JobUnions = sjue.JobUnions,
                                Unions = sjue.Unions,
                                // EmployeeUnions = sjue.EmployeeUnions,
                                UnionPayRate = payRate
                            })
                            .ToList();
                        JobUnions jobUnion = sjueu?.FirstOrDefault()?.JobUnions;
                        */
                        var unionLocal = workerInfo.UnionLocal; // LocalNumber
                        var unionNumber = workerInfo.UnionNumber; // IBEW or EXEMPT
                        
                        var union = _unionsRepository.GetAll().Where(x => x.LocalNumber == unionLocal && EF.Functions.Like(x.Number, $"%{unionNumber}%")).FirstOrDefault();
                        var unionPayRate = _unionPayRatesRepository.GetAll().Where(x => x.Class == workerClass.Code && x.UnionsId == union.Id).FirstOrDefault();

                        
                        // todo; Get Relevant JobUnion from EmployeeUnion to fetch Relevant
                        var unionLocalId = union?.Id;
                        var unionPayRateId = unionPayRate?.Id;
                        var unionPayRatePerHour = unionPayRate == null ? 0 : unionPayRate.Perhour;

                        if (multiplier > 0)
                        {
                            unionPayRatePerHour *= multiplier;
                        }

                        var timetable = _timetablesRepository.GetAll().Where(x => x.PeriodDate == periodDateId
                                    && x.Description == null
                                    && x.ResourcesCode == resourceId
                                    && x.PayTypesId == payTypeId
                                    && x.State == stateAddressId
                                    && x.Unionlocal == unionLocalId
                                    && x.CostCode == costCode
                                    && x.WorkerClaseesId == workerClassId
                                    && x.Rate == unionPayRateId)
                                .FirstOrDefault();

                        if (timetable != null)
                        {
                            bool dayPreFilled = false;

                            switch (day)
                            {
                                case 1:
                                    dayPreFilled = timetable.Day1 != null || timetable.Day1.HasValue || timetable.Day1 > 0;
                                    if (!dayPreFilled)
                                    {
                                        timetable.Day1 = totalHours;
                                    }
                                    break;
                                case 2:
                                    dayPreFilled = timetable.Day2 != null || timetable.Day2.HasValue || timetable.Day2 > 0;
                                    if (!dayPreFilled)
                                    {
                                        timetable.Day2 = totalHours;
                                    }
                                    break;
                                case 3:
                                    dayPreFilled = timetable.Day3 != null || timetable.Day3.HasValue || timetable.Day3 > 0;
                                    if (!dayPreFilled)
                                    {
                                        timetable.Day3 = totalHours;
                                    }
                                    break;
                                case 4:
                                    dayPreFilled = timetable.Day4 != null || timetable.Day4.HasValue || timetable.Day4 > 0;
                                    if (!dayPreFilled)
                                    {
                                        timetable.Day4 = totalHours;
                                    }
                                    break;
                                case 5:
                                    dayPreFilled = timetable.Day5 != null || timetable.Day5.HasValue || timetable.Day5 > 0;
                                    if (!dayPreFilled)
                                    {
                                        timetable.Day5 = totalHours;
                                    }
                                    break;
                                case 6:
                                    dayPreFilled = timetable.Day6 != null || timetable.Day6.HasValue || timetable.Day6 > 0;
                                    if (!dayPreFilled)
                                    {
                                        timetable.Day6 = totalHours;
                                    }
                                    break;
                                case 7:
                                    dayPreFilled = timetable.Day7 != null || timetable.Day7.HasValue || timetable.Day7 > 0;
                                    if (!dayPreFilled)
                                    {
                                        timetable.Day7 = totalHours;
                                    }
                                    break;
                            }

                            if (unionPayRate != null)
                            {
                                decimal totalSevenDaysHours = 0;
                                #region Summation
                                if (timetable.Day1 != null && timetable.Day1.HasValue)
                                {
                                    totalSevenDaysHours += timetable.Day1.Value;
                                }
                                if (timetable.Day2 != null && timetable.Day2.HasValue)
                                {
                                    totalSevenDaysHours += timetable.Day2.Value;
                                }
                                if (timetable.Day3 != null && timetable.Day3.HasValue)
                                {
                                    totalSevenDaysHours += timetable.Day3.Value;
                                }
                                if (timetable.Day4 != null && timetable.Day4.HasValue)
                                {
                                    totalSevenDaysHours += timetable.Day4.Value;
                                }
                                if (timetable.Day5 != null && timetable.Day5.HasValue)
                                {
                                    totalSevenDaysHours += timetable.Day5.Value;
                                }
                                if (timetable.Day6 != null && timetable.Day6.HasValue)
                                {
                                    totalSevenDaysHours += timetable.Day6.Value;
                                }
                                if (timetable.Day7 != null && timetable.Day7.HasValue)
                                {
                                    totalSevenDaysHours += timetable.Day7.Value;
                                }
                                #endregion
                                amount = totalSevenDaysHours * unionPayRatePerHour;
                            }


                            if (!dayPreFilled)
                            {
                                timetable.Amount = amount;
                                await _timetablesRepository.UpdateAsync(timetable);
                            }

                        }
                        else
                        {

                            timetable = new Timetables
                            {
                                AccountsId = accountsId,
                                CostTypesId = _costTypesRepository.GetAll().FirstOrDefault()?.Id,
                                CreatedBy = createdBy,
                                CreatedOn = createdOn,
                                IsActive = true,
                                PayTypesId = payTypeId,
                                Multiplier = Convert.ToDouble(multiplier),
                                PeriodDate = periodDateId,
                                Rate = unionPayRateId,
                                ResourcesCode = resourceId,
                                State = stateAddressId,
                                Unionlocal = unionLocalId,
                                CostCode = costCode,
                                WorkerClaseesId = workerClassId,
                                Wcomp1 = wcomp1
                            };
                            switch (day)
                            {
                                case 1:
                                    amount = totalHours * unionPayRatePerHour;

                                    timetable.Amount = amount;
                                    timetable.Day1 = totalHours;
                                    break;
                                case 2:
                                    amount = totalHours * unionPayRatePerHour;
                                    timetable.Amount = amount;
                                    timetable.Day2 = totalHours;
                                    break;
                                case 3:
                                    amount = totalHours * unionPayRatePerHour;
                                    timetable.Amount = amount;
                                    timetable.Day3 = totalHours;
                                    break;
                                case 4:
                                    amount = totalHours * unionPayRatePerHour;
                                    timetable.Amount = amount;
                                    timetable.Day4 = totalHours;
                                    break;
                                case 5:
                                    amount = totalHours * unionPayRatePerHour;
                                    timetable.Amount = amount;
                                    timetable.Day5 = totalHours;
                                    break;
                                case 6:
                                    amount = totalHours * unionPayRatePerHour;
                                    timetable.Amount = amount;
                                    timetable.Day6 = totalHours;
                                    break;
                                case 7:
                                    amount = totalHours * unionPayRatePerHour;
                                    timetable.Amount = amount;
                                    timetable.Day7 = totalHours;
                                    break;
                            }
                            //if (multiplier > 0)
                            //{
                            //    timetable.Amount = (decimal)multiplier * timetable.Amount;
                            //}
                            var inserted = await _timetablesRepository.InsertAsync(timetable);
                            await CurrentUnitOfWork.SaveChangesAsync(); //Uncomment to immediately insert.
                        }


                        foreach (var shiftExpense in shiftExpenses)
                        {
                            var costTypeId = _costTypesRepository.GetAll().Where(x => x.Name.ToLower() == "f").FirstOrDefault()?.Id;
                            multiplier = payTypeIdExpese.Multiplier;
                            var descriptionId = shiftExpense?.ExpenseTypesFk?.Id; // ShiftExpense has a ExpenseTypes object description, we are referring to that.
                            var timetableShiftExpense = _timetablesRepository.GetAll().Where(x => x.PeriodDate == periodDateId
                                    // && x.Description == descriptionId
                                    && x.CostTypesId == costTypeId
                                    && x.ResourcesCode == resourceId
                                    && x.PayTypesId == payTypeIdExpese.Id
                                    && x.State == stateAddressId
                                    && x.Unionlocal == unionLocalId
                                    && x.CostCode == costCode
                                    && x.WorkerClaseesId == workerClassId
                                    && x.Rate == unionPayRateId)
                                .FirstOrDefault();

                            if (timetableShiftExpense == null)
                            {
                                timetableShiftExpense = new Timetables
                                {
                                    AccountsId = accountsId,
                                    CreatedBy = createdBy,
                                    CreatedOn = createdOn,
                                    IsActive = true,
                                    PayTypesId = payTypeIdExpese?.Id,
                                    PeriodDate = periodDateId,
                                    Rate = unionPayRateId,
                                    ResourcesCode = resourceId,
                                    State = stateAddressId,
                                    Unionlocal = unionLocalId,
                                    CostCode = costCode,
                                    Description = descriptionId,
                                    WorkerClaseesId = workerClassId,
                                    Multiplier = (double)multiplier,
                                    Wcomp1 = wcomp1

                                };

                                if (shiftExpense.Amount > 0)
                                {
                                    switch (day)
                                    {
                                        case 1:
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day1 = shiftExpense.Amount;
                                            break;
                                        case 2:
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day2 = shiftExpense.Amount;
                                            break;
                                        case 3:
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day3 = shiftExpense.Amount;
                                            break;
                                        case 4:
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day4 = shiftExpense.Amount;
                                            break;
                                        case 5:
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day5 = shiftExpense.Amount;
                                            break;
                                        case 6:
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day6 = shiftExpense.Amount;
                                            break;
                                        case 7:
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day7 = shiftExpense.Amount;
                                            break;
                                    }
                                }
                                timetableShiftExpense.CostTypesId = costTypeId;

                                var inserted = await _timetablesRepository.InsertAsync(timetableShiftExpense);
                            }
                            else
                            {
                                if (shiftExpense.Amount > 0)
                                {
                                    var dayPrefilled = false;
                                    switch (day)
                                    {
                                        case 1:
                                            if (timetableShiftExpense.Day1 != null && timetableShiftExpense.Day1.HasValue)
                                            {
                                                dayPrefilled = true;
                                                if (timetableShiftExpense.Amount != null || timetableShiftExpense.Amount.HasValue)
                                                {
                                                    if (timetableShiftExpense.Amount >= timetableShiftExpense.Day1)
                                                    {
                                                        timetableShiftExpense.Amount -= timetableShiftExpense.Day1;
                                                        timetableShiftExpense.Day1 = null;
                                                    }
                                                }
                                                else
                                                {
                                                    timetableShiftExpense = ResetTimetableDays(timetableShiftExpense);
                                                }
                                            }
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }

                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day1 = shiftExpense.Amount;
                                            break;
                                        case 2:
                                            if (timetableShiftExpense.Day2 != null && timetableShiftExpense.Day2.HasValue)
                                            {
                                                dayPrefilled = true;
                                                if (timetableShiftExpense.Amount != null || timetableShiftExpense.Amount.HasValue)
                                                {
                                                    if (timetableShiftExpense.Amount >= timetableShiftExpense.Day2)
                                                    {
                                                        timetableShiftExpense.Amount -= timetableShiftExpense.Day2;
                                                        timetableShiftExpense.Day2 = null;
                                                    }
                                                }
                                                else
                                                {
                                                    timetableShiftExpense = ResetTimetableDays(timetableShiftExpense);
                                                }
                                            }
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day2 = shiftExpense.Amount;
                                            break;
                                        case 3:
                                            if (timetableShiftExpense.Day3 != null && timetableShiftExpense.Day3.HasValue)
                                            {
                                                dayPrefilled = true;
                                                if (timetableShiftExpense.Amount != null || timetableShiftExpense.Amount.HasValue)
                                                {
                                                    if (timetableShiftExpense.Amount >= timetableShiftExpense.Day3)
                                                    {
                                                        timetableShiftExpense.Amount -= timetableShiftExpense.Day3;
                                                        timetableShiftExpense.Day3 = null;
                                                    }
                                                }
                                                else
                                                {
                                                    timetableShiftExpense = ResetTimetableDays(timetableShiftExpense);
                                                }
                                            }
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day3 = shiftExpense.Amount;
                                            break;
                                        case 4:
                                            if (timetableShiftExpense.Day4 != null && timetableShiftExpense.Day4.HasValue)
                                            {
                                                dayPrefilled = true;
                                                if (timetableShiftExpense.Amount != null || timetableShiftExpense.Amount.HasValue)
                                                {
                                                    if (timetableShiftExpense.Amount >= timetableShiftExpense.Day4)
                                                    {
                                                        timetableShiftExpense.Amount -= timetableShiftExpense.Day4;
                                                        timetableShiftExpense.Day4 = null;
                                                    }
                                                }
                                                else
                                                {
                                                    timetableShiftExpense = ResetTimetableDays(timetableShiftExpense);
                                                }
                                            }
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day4 = shiftExpense.Amount;
                                            break;
                                        case 5:
                                            if (timetableShiftExpense.Day5 != null && timetableShiftExpense.Day5.HasValue)
                                            {
                                                dayPrefilled = true;
                                                if (timetableShiftExpense.Amount != null || timetableShiftExpense.Amount.HasValue)
                                                {
                                                    if (timetableShiftExpense.Amount >= timetableShiftExpense.Day5)
                                                    {
                                                        timetableShiftExpense.Amount -= timetableShiftExpense.Day5;
                                                        timetableShiftExpense.Day5 = null;
                                                    }
                                                }
                                                else
                                                {
                                                    timetableShiftExpense = ResetTimetableDays(timetableShiftExpense);
                                                }
                                            }
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day5 = shiftExpense.Amount;
                                            break;
                                        case 6:
                                            if (timetableShiftExpense.Day6 != null && timetableShiftExpense.Day6.HasValue)
                                            {
                                                dayPrefilled = true;
                                                if (timetableShiftExpense.Amount != null || timetableShiftExpense.Amount.HasValue)
                                                {
                                                    if (timetableShiftExpense.Amount >= timetableShiftExpense.Day6)
                                                    {
                                                        timetableShiftExpense.Amount -= timetableShiftExpense.Day6;
                                                        timetableShiftExpense.Day6 = null;
                                                    }
                                                }
                                                else
                                                {
                                                    timetableShiftExpense = ResetTimetableDays(timetableShiftExpense);
                                                }
                                            }
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day6 = shiftExpense.Amount;
                                            break;
                                        case 7:
                                            if (timetableShiftExpense.Day7 != null && timetableShiftExpense.Day7.HasValue)
                                            {
                                                dayPrefilled = true;
                                                if (timetableShiftExpense.Amount != null || timetableShiftExpense.Amount.HasValue)
                                                {
                                                    if (timetableShiftExpense.Amount >= timetableShiftExpense.Day7)
                                                    {
                                                        timetableShiftExpense.Amount -= timetableShiftExpense.Day7;
                                                        timetableShiftExpense.Day7 = null;
                                                    }
                                                }
                                                else
                                                {
                                                    timetableShiftExpense = ResetTimetableDays(timetableShiftExpense);
                                                }
                                            }
                                            if (timetableShiftExpense.Amount == null || !timetableShiftExpense.Amount.HasValue)
                                            {
                                                timetableShiftExpense.Amount = 0;
                                            }
                                            timetableShiftExpense.Amount += shiftExpense.Amount;
                                            timetableShiftExpense.Day7 = shiftExpense.Amount;
                                            break;
                                    }
                                    var updated = await _timetablesRepository.UpdateAsync(timetableShiftExpense);
                                }
                            }
                            await CurrentUnitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        decimal amount = 0;
                        var jobCode = tShiftResource.ShiftResource.ShiftsFk.JobsFk.Id;
                        var phaseCode = tShiftResource.ShiftResource.JobPhaseCodesFk?.Id;
                        var categoryCode = tShiftResource.ShiftResource.JobCategoriesFk?.Id;
                        decimal costPerHour = 0;
                        if (tShiftResource.ShiftResource.ResourcesFk != null && tShiftResource.ShiftResource.ResourcesFk.CostPerHour != null)
                        {
                            costPerHour = tShiftResource.ShiftResource.ResourcesFk.CostPerHour.Value;
                        }
                        var equiptimetable = _equipTimeTablesRepository.GetAll().Where(x => x.PeriodDate == periodDateId
                                    && x.ResourcesCode == resourceId
                                    && x.JobCode == jobCode
                                    && x.PhaseCode == phaseCode
                                    && x.CategoryCode == categoryCode)
                                .FirstOrDefault();

                        if (equiptimetable != null)
                        {
                            bool dayPreFilled = false;

                            switch (day)
                            {
                                case 1:
                                    dayPreFilled = equiptimetable.Day1 != null || equiptimetable.Day1.HasValue || equiptimetable.Day1 > 0;
                                    if (!dayPreFilled)
                                    {
                                        equiptimetable.Day1 = totalHours;
                                    }
                                    break;
                                case 2:
                                    dayPreFilled = equiptimetable.Day2 != null || equiptimetable.Day2.HasValue || equiptimetable.Day2 > 0;
                                    if (!dayPreFilled)
                                    {
                                        equiptimetable.Day2 = totalHours;
                                    }
                                    break;
                                case 3:
                                    dayPreFilled = equiptimetable.Day3 != null || equiptimetable.Day3.HasValue || equiptimetable.Day3 > 0;
                                    if (!dayPreFilled)
                                    {
                                        equiptimetable.Day3 = totalHours;
                                    }
                                    break;
                                case 4:
                                    dayPreFilled = equiptimetable.Day4 != null || equiptimetable.Day4.HasValue || equiptimetable.Day4 > 0;
                                    if (!dayPreFilled)
                                    {
                                        equiptimetable.Day4 = totalHours;
                                    }
                                    break;
                                case 5:
                                    dayPreFilled = equiptimetable.Day5 != null || equiptimetable.Day5.HasValue || equiptimetable.Day5 > 0;
                                    if (!dayPreFilled)
                                    {
                                        equiptimetable.Day5 = totalHours;
                                    }
                                    break;
                                case 6:
                                    dayPreFilled = equiptimetable.Day6 != null || equiptimetable.Day6.HasValue || equiptimetable.Day6 > 0;
                                    if (!dayPreFilled)
                                    {
                                        equiptimetable.Day6 = totalHours;
                                    }
                                    break;
                                case 7:
                                    dayPreFilled = equiptimetable.Day7 != null || equiptimetable.Day7.HasValue || equiptimetable.Day7 > 0;
                                    if (!dayPreFilled)
                                    {
                                        equiptimetable.Day7 = totalHours;
                                    }
                                    break;
                            }

                            if (costPerHour >= 0)
                            {
                                decimal totalSevenDaysHours = 0;
                                #region Summation
                                if (equiptimetable.Day1 != null && equiptimetable.Day1.HasValue)
                                {
                                    totalSevenDaysHours += equiptimetable.Day1.Value;
                                }
                                if (equiptimetable.Day2 != null && equiptimetable.Day2.HasValue)
                                {
                                    totalSevenDaysHours += equiptimetable.Day2.Value;
                                }
                                if (equiptimetable.Day3 != null && equiptimetable.Day3.HasValue)
                                {
                                    totalSevenDaysHours += equiptimetable.Day3.Value;
                                }
                                if (equiptimetable.Day4 != null && equiptimetable.Day4.HasValue)
                                {
                                    totalSevenDaysHours += equiptimetable.Day4.Value;
                                }
                                if (equiptimetable.Day5 != null && equiptimetable.Day5.HasValue)
                                {
                                    totalSevenDaysHours += equiptimetable.Day5.Value;
                                }
                                if (equiptimetable.Day6 != null && equiptimetable.Day6.HasValue)
                                {
                                    totalSevenDaysHours += equiptimetable.Day6.Value;
                                }
                                if (equiptimetable.Day7 != null && equiptimetable.Day7.HasValue)
                                {
                                    totalSevenDaysHours += equiptimetable.Day7.Value;
                                }
                                #endregion
                                amount = totalSevenDaysHours * costPerHour;
                            }


                            if (!dayPreFilled)
                            {
                                equiptimetable.Amount = amount;
                                await _equipTimeTablesRepository.UpdateAsync(equiptimetable);
                            }

                        }
                        else
                        {

                            equiptimetable = new EquipTimetables
                            {
                                CreatedOn = createdOn,
                                IsActive = true,
                                PeriodDate = periodDateId,
                                ResourcesCode = resourceId,
                                JobCode = jobCode,
                                CategoryCode = categoryCode,
                                PhaseCode = phaseCode
                            };
                            switch (day)
                            {
                                case 1:
                                    amount = totalHours * costPerHour;

                                    equiptimetable.Amount = amount;
                                    equiptimetable.Day1 = totalHours;
                                    break;
                                case 2:
                                    amount = totalHours * costPerHour;
                                    equiptimetable.Amount = amount;
                                    equiptimetable.Day2 = totalHours;
                                    break;
                                case 3:
                                    amount = totalHours * costPerHour;
                                    equiptimetable.Amount = amount;
                                    equiptimetable.Day3 = totalHours;
                                    break;
                                case 4:
                                    amount = totalHours * costPerHour;
                                    equiptimetable.Amount = amount;
                                    equiptimetable.Day4 = totalHours;
                                    break;
                                case 5:
                                    amount = totalHours * costPerHour;
                                    equiptimetable.Amount = amount;
                                    equiptimetable.Day5 = totalHours;
                                    break;
                                case 6:
                                    amount = totalHours * costPerHour;
                                    equiptimetable.Amount = amount;
                                    equiptimetable.Day6 = totalHours;
                                    break;
                                case 7:
                                    amount = totalHours * costPerHour;
                                    equiptimetable.Amount = amount;
                                    equiptimetable.Day7 = totalHours;
                                    break;
                            }
                            var inserted = await _equipTimeTablesRepository.InsertAsync(equiptimetable);
                            await CurrentUnitOfWork.SaveChangesAsync(); //Uncomment to immediately insert.
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private Timetables ResetTimetableDays(Timetables timetable)
        {
            timetable.Day1 = null;
            timetable.Day2 = null;
            timetable.Day3 = null;
            timetable.Day4 = null;
            timetable.Day5 = null;
            timetable.Day6 = null;
            timetable.Day7 = null;

            return timetable;

        }
        private int GetDayFromDates(PayPeriods payPeriod, DateTime? date)
        {
            if (date is null || !date.HasValue) return -1;
            if (date.Value.Date < payPeriod.StartDate.Date || date.Value.Date > payPeriod.EndDate.Date) return -1;

            return (date.Value.Date - payPeriod.StartDate.Date).Days + 1;
        }

        [AbpAuthorize(AppPermissions.Pages_StatusUpdates_Delete)]
        public async Task Delete(EntityDto input)
        {
            var recordToDelete = _statusUpdatesRepository.Get(input.Id);

            if (recordToDelete != null)
            {
                var payPeriod = _payPeriodsRepository.GetAll().Where(x => recordToDelete.ActualCreateDateTime >= x.StartDate && recordToDelete.ActualCreateDateTime <= x.EndDate).FirstOrDefault();

                if (payPeriod != null)
                {
                    var day = GetDayFromDates(payPeriod, recordToDelete.ActualCreateDateTime);

                    var timetables = _timetablesRepository.GetAll()
                        .Where(timetable => timetable.PeriodDate == payPeriod.Id)
                        .Include(timetable => timetable.RateFk)
                        .ToList();
                    var equiptimetables = _equipTimeTablesRepository.GetAll()
                        .Where(equiptimetable => equiptimetable.PeriodDate == payPeriod.Id)
                        .Include(equiptimetable => equiptimetable.CategoryCodeFk)
                        .Include(equiptimetable => equiptimetable.JobCodeFk)
                        .Include(equiptimetable => equiptimetable.PhaseCodeFk)
                        .Include(equiptimetable => equiptimetable.ResourcesCodeFk)
                        .ToList();
                    foreach (var timetable in timetables)
                    {
                        if (timetable is null) continue;

                        var costType = _costTypesRepository.GetAll().Where(x => x.Id == timetable.CostTypesId).FirstOrDefault();
                        var payType = _payTypesRepository.GetAll().Where(x => x.Id == timetable.PayTypesId).FirstOrDefault();


                        //if (costType.Name.ToLower() == "f")
                        //{
                        //    if (timetable.CreatedOn.HasValue && timetable.CreatedOn.Value.Date == recordToDelete.ActualCreateDateTime.Value.Date)
                        //        await _timetablesRepository.DeleteAsync(timetable);
                        //    continue;
                        //}
                        decimal? dayHours = 0;
                        decimal? amount = timetable.Amount;
                        var payRatePerHour = timetable.RateFk?.Perhour;

                        if (payType != null && payType.Multiplier > 0)
                        {
                            payRatePerHour *= payType.Multiplier;
                        }

                        switch (day)
                        {
                            case 1:
                                dayHours = timetable.Day1;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    if (costType.Name.ToLower() == "f")
                                    {
                                        amount = amount - (dayHours);
                                    }
                                    else
                                    {
                                        amount = amount - (dayHours * payRatePerHour);
                                    }

                                    timetable.Day1 = 0;
                                }
                                break;
                            case 2:
                                dayHours = timetable.Day2;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    if (costType.Name.ToLower() == "f")
                                    {
                                        amount = amount - (dayHours);
                                    }
                                    else
                                    {
                                        amount = amount - (dayHours * payRatePerHour);
                                    }
                                    timetable.Day2 = 0;
                                }
                                break;
                            case 3:
                                dayHours = timetable.Day3;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    if (costType.Name.ToLower() == "f")
                                    {
                                        amount = amount - (dayHours);
                                    }
                                    else
                                    {
                                        amount = amount - (dayHours * payRatePerHour);
                                    }
                                    timetable.Day3 = 0;
                                }
                                break;
                            case 4:
                                dayHours = timetable.Day4;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    if (costType.Name.ToLower() == "f")
                                    {
                                        amount = amount - (dayHours);
                                    }
                                    else
                                    {
                                        amount = amount - (dayHours * payRatePerHour);
                                    }
                                    timetable.Day4 = 0;
                                }
                                break;
                            case 5:
                                dayHours = timetable.Day5;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    if (costType.Name.ToLower() == "f")
                                    {
                                        amount = amount - (dayHours);
                                    }
                                    else
                                    {
                                        amount = amount - (dayHours * payRatePerHour);
                                    }
                                    timetable.Day5 = 0;
                                }
                                break;
                            case 6:
                                dayHours = timetable.Day6;
                                amount = timetable.Amount;
                                if (dayHours != null && dayHours.HasValue)
                                {
                                    if (costType.Name.ToLower() == "f")
                                    {
                                        amount = amount - (dayHours);
                                    }
                                    else
                                    {
                                        amount = amount - (dayHours * payRatePerHour);
                                    }
                                    timetable.Day6 = 0;
                                }
                                break;
                            case 7:
                                dayHours = timetable.Day7;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    if (costType.Name.ToLower() == "f")
                                    {
                                        amount = amount - (dayHours);
                                    }
                                    else
                                    {
                                        amount = amount - (dayHours * payRatePerHour);
                                    }
                                    timetable.Day7 = 0;
                                }
                                break;
                        }
                        timetable.Amount = amount;
                        await _timetablesRepository.UpdateAsync(timetable);

                        if (timetable.Day1.HasValue && timetable.Day1.Value <= 0 || !timetable.Day1.HasValue)
                        {
                            timetable.Day1 = null;
                        }
                        if (timetable.Day2.HasValue && timetable.Day2.Value <= 0 || !timetable.Day2.HasValue)
                        {
                            timetable.Day2 = null;
                        }
                        if (timetable.Day3.HasValue && timetable.Day3.Value <= 0 || !timetable.Day3.HasValue)
                        {
                            timetable.Day3 = null;
                        }
                        if (timetable.Day4.HasValue && timetable.Day4.Value <= 0 || !timetable.Day4.HasValue)
                        {
                            timetable.Day4 = null;
                        }
                        if (timetable.Day5.HasValue && timetable.Day5.Value <= 0 || !timetable.Day5.HasValue)
                        {
                            timetable.Day5 = null;
                        }
                        if (timetable.Day6.HasValue && timetable.Day6.Value <= 0 || !timetable.Day6.HasValue)
                        {
                            timetable.Day6 = null;
                        }
                        if (timetable.Day7.HasValue && timetable.Day7.Value <= 0 || !timetable.Day7.HasValue)
                        {
                            timetable.Day7 = null;
                        }

                        if (timetable.Day1 == null
                            && timetable.Day2 == null
                            && timetable.Day3 == null
                            && timetable.Day4 == null
                            && timetable.Day5 == null
                            && timetable.Day6 == null
                            && timetable.Day7 == null)
                        {
                            await _timetablesRepository.DeleteAsync(timetable);
                        }

                    }
                    foreach (var timetable in equiptimetables)
                    {
                        if (timetable is null) continue;


                        decimal? dayHours = 0;
                        decimal? amount = timetable.Amount;
                        var payRatePerHour = timetable.ResourcesCodeFk.CostPerDay;

                        switch (day)
                        {
                            case 1:
                                dayHours = timetable.Day1;

                                if (dayHours != null && dayHours.HasValue)
                                {

                                    amount = amount - (dayHours * payRatePerHour);


                                    timetable.Day1 = 0;
                                }
                                break;
                            case 2:
                                dayHours = timetable.Day2;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                   
                                        amount = amount - (dayHours * payRatePerHour);
                                    
                                    timetable.Day2 = 0;
                                }
                                break;
                            case 3:
                                dayHours = timetable.Day3;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                        amount = amount - (dayHours * payRatePerHour);
                                    
                                    timetable.Day3 = 0;
                                }
                                break;
                            case 4:
                                dayHours = timetable.Day4;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    
                                        amount = amount - (dayHours * payRatePerHour);
                                    timetable.Day4 = 0;
                                }
                                break;
                            case 5:
                                dayHours = timetable.Day5;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    
                                        amount = amount - (dayHours * payRatePerHour);
                                    
                                    timetable.Day5 = 0;
                                }
                                break;
                            case 6:
                                dayHours = timetable.Day6;
                                amount = timetable.Amount;
                                if (dayHours != null && dayHours.HasValue)
                                {
                                   
                                        amount = amount - (dayHours * payRatePerHour);
                                    
                                    timetable.Day6 = 0;
                                }
                                break;
                            case 7:
                                dayHours = timetable.Day7;

                                if (dayHours != null && dayHours.HasValue)
                                {
                                    
                                        amount = amount - (dayHours * payRatePerHour);
                                    
                                    timetable.Day7 = 0;
                                }
                                break;
                        }
                        timetable.Amount = amount;
                        await _equipTimeTablesRepository.UpdateAsync(timetable);


                        // Uncomment if you want to delete the record where all of the day values are null
                        if (timetable.Day1.HasValue && timetable.Day1.Value <= 0 || !timetable.Day1.HasValue)
                        {
                            timetable.Day1 = null;
                        }
                        if (timetable.Day2.HasValue && timetable.Day2.Value <= 0 || !timetable.Day2.HasValue)
                        {
                            timetable.Day2 = null;
                        }
                        if (timetable.Day3.HasValue && timetable.Day3.Value <= 0 || !timetable.Day3.HasValue)
                        {
                            timetable.Day3 = null;
                        }
                        if (timetable.Day4.HasValue && timetable.Day4.Value <= 0 || !timetable.Day4.HasValue)
                        {
                            timetable.Day4 = null;
                        }
                        if (timetable.Day5.HasValue && timetable.Day5.Value <= 0 || !timetable.Day5.HasValue)
                        {
                            timetable.Day5 = null;
                        }
                        if (timetable.Day6.HasValue && timetable.Day6.Value <= 0 || !timetable.Day6.HasValue)
                        {
                            timetable.Day6 = null;
                        }
                        if (timetable.Day7.HasValue && timetable.Day7.Value <= 0 || !timetable.Day7.HasValue)
                        {
                            timetable.Day7 = null;
                        }

                        if (timetable.Day1 == null
                            && timetable.Day2 == null
                            && timetable.Day3 == null
                            && timetable.Day4 == null
                            && timetable.Day5 == null
                            && timetable.Day6 == null
                            && timetable.Day7 == null)
                        {
                            await _equipTimeTablesRepository.DeleteAsync(timetable);
                        }

                    }
                }
            }

            await _statusUpdatesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStatusUpdatesToExcel(GetAllStatusUpdatesForExcelInput input)
        {

            var filteredStatusUpdates = _statusUpdatesRepository.GetAll()
                        .Include(e => e.TimesheetsFk)
                        .Include(e => e.NewStatusesFk)
                        .Include(e => e.JobsFk)
                        .Include(e => e.ModifiedByFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinModifiedOnFilter != null, e => e.ModifiedOn >= input.MinModifiedOnFilter)
                        .WhereIf(input.MaxModifiedOnFilter != null, e => e.ModifiedOn <= input.MaxModifiedOnFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinOriginalstatusIdFilter != null, e => e.OriginalstatusId >= input.MinOriginalstatusIdFilter)
                        .WhereIf(input.MaxOriginalstatusIdFilter != null, e => e.OriginalstatusId <= input.MaxOriginalstatusIdFilter)
                        .WhereIf(input.MinActualCreateDateTimeFilter != null, e => e.ActualCreateDateTime >= input.MinActualCreateDateTimeFilter)
                        .WhereIf(input.MaxActualCreateDateTimeFilter != null, e => e.ActualCreateDateTime <= input.MaxActualCreateDateTimeFilter)
                        // .WhereIf(!string.IsNullOrWhiteSpace(input.TimeshetIdsFilter), e => e.TimeshetIds == input.TimeshetIdsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TimesheetsNameFilter), e => e.TimesheetsFk != null && e.TimesheetsFk.Name == input.TimesheetsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusesNameFilter), e => e.NewStatusesFk != null && e.NewStatusesFk.Name == input.StatusesNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobsNameFilter), e => e.JobsFk != null && e.JobsFk.Name == input.JobsNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ModifiedByFk != null && e.ModifiedByFk.Name == input.UserNameFilter);

            var query = (from o in filteredStatusUpdates
                         join o1 in _lookup_timesheetsRepository.GetAll() on o.TimesheetsId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _statusesRepository.GetAll() on o.NewStatusesId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _jobsRepository.GetAll() on o.JobsId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _userRepository.GetAll() on o.ModifiedBy equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetStatusUpdatesForViewDto()
                         {
                             StatusUpdates = new StatusUpdatesDto
                             {
                                 ModifiedOn = o.ModifiedOn,
                                 Name = o.Name,
                                 OriginalstatusId = o.OriginalstatusId,
                                 ActualCreateDateTime = o.ActualCreateDateTime,
                                 // TimeshetIds = o.TimeshetIds,
                                 Id = o.Id
                             },
                             TimesheetsName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StatusesName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             JobsName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             UserName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });


            var statusUpdatesListDtos = await query.ToListAsync();

            return _statusUpdatesExcelExporter.ExportToFile(statusUpdatesListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_StatusUpdates)]
        public async Task<PagedResultDto<StatusUpdatesTimesheetsLookupTableDto>> GetAllTimesheetsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_timesheetsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var timesheetsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StatusUpdatesTimesheetsLookupTableDto>();
            foreach (var timesheets in timesheetsList)
            {
                lookupTableDtoList.Add(new StatusUpdatesTimesheetsLookupTableDto
                {
                    Id = timesheets.Id,
                    DisplayName = timesheets.Name?.ToString()
                });
            }

            return new PagedResultDto<StatusUpdatesTimesheetsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StatusUpdates)]
        public async Task<PagedResultDto<StatusUpdatesStatusesLookupTableDto>> GetAllStatusesForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _statusesRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var statusesList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StatusUpdatesStatusesLookupTableDto>();
            foreach (var statuses in statusesList)
            {
                lookupTableDtoList.Add(new StatusUpdatesStatusesLookupTableDto
                {
                    Id = statuses.Id,
                    DisplayName = statuses.Name?.ToString()
                });
            }

            return new PagedResultDto<StatusUpdatesStatusesLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StatusUpdates)]
        public async Task<PagedResultDto<StatusUpdatesJobsLookupTableDto>> GetAllJobsForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _jobsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var jobsList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StatusUpdatesJobsLookupTableDto>();
            foreach (var jobs in jobsList)
            {
                lookupTableDtoList.Add(new StatusUpdatesJobsLookupTableDto
                {
                    Id = jobs.Id,
                    DisplayName = jobs.Name?.ToString()
                });
            }

            return new PagedResultDto<StatusUpdatesJobsLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StatusUpdates)]
        public async Task<PagedResultDto<StatusUpdatesUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StatusUpdatesUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new StatusUpdatesUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<StatusUpdatesUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}
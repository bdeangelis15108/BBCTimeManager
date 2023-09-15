using Nucleus.Timetable;
using Nucleus.CostType;
using Nucleus.Account;
using Nucleus.UnionPayRate;
using Nucleus.PayperiodHistory;
using Nucleus.ShiftExpense;
using Nucleus.EmployeeUnion;
using Nucleus.JobUnion;
using Nucleus.Union;
using Nucleus.ShiftResource;
using Nucleus.JobCategory;
using Nucleus.JobPhaseCode;
using Nucleus.Shift;
using Nucleus.PayPeriod;
using Nucleus.Timesheet;
using Nucleus.Status;
using Nucleus.ExpenseType;
using Nucleus.PayType;
using Nucleus.ResourceReservation;
using Nucleus.Job;
using Nucleus.JobClass;
using Nucleus.Address;
using Nucleus.ResourceEquipmentInfo;
using Nucleus.ResourceWorkerInfo;
using Nucleus.WorkerClasee;
using Nucleus.Resource;
using Nucleus.ECCOSTS;
using Nucleus.EQUIPMENTS;
using Nucleus.PRDEDRATES;
using Nucleus.JCJOBS;
using Nucleus.PRCLASSES;
using Nucleus.JCUNIONS;
using Nucleus.JCCAT;
using Nucleus.PREMPLOYEES;
using System;
using System.Linq;
using Abp.Organizations;
using Nucleus.Authorization.Roles;
using Nucleus.MultiTenancy;

namespace Nucleus.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(Timetables),
            typeof(CostTypes),
            typeof(Accounts),
            typeof(UnionPayRates),
            typeof(PayperiodHistories),
            typeof(ShiftExpenses),
            typeof(EmployeeUnions),
            typeof(JobUnions),
            typeof(Unions),
            typeof(ShiftResources),
            typeof(JobCategories),
            typeof(JobPhaseCodes),
            typeof(Shifts),
            typeof(PayPeriods),
            typeof(Timesheets),
            typeof(Statuses),
            typeof(ExpenseTypes),
            typeof(PayTypes),
            typeof(ResourceReservations),
            typeof(Jobs),
            typeof(JobClasses),
            typeof(Addresses),
            typeof(ResourceEquipmentInfos),
            typeof(ResourceWorkerInfos),
            typeof(WorkerClasees),
            typeof(Resources),
            typeof(ECCOST),
            typeof(EQUIPMENT),
            typeof(PRDEDRATE),
            typeof(JCJOB),
            typeof(PRCLASS),
            typeof(JCUNION),
            typeof(JACCAT),
            typeof(PREMPLOYEE),
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(ECCOST),
            typeof(EQUIPMENT),
            typeof(PRDEDRATE),
            typeof(JCJOB),
            typeof(PRCLASS),
            typeof(JCUNION),
            typeof(JACCAT),
            typeof(PREMPLOYEE),
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}
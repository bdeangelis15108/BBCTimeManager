using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Nucleus.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var equipTimetables = pages.CreateChildPermission(AppPermissions.Pages_EquipTimetables, L("EquipTimetables"), multiTenancySides: MultiTenancySides.Host);
            equipTimetables.CreateChildPermission(AppPermissions.Pages_EquipTimetables_Create, L("CreateNewEquipTimetables"), multiTenancySides: MultiTenancySides.Host);
            equipTimetables.CreateChildPermission(AppPermissions.Pages_EquipTimetables_Edit, L("EditEquipTimetables"), multiTenancySides: MultiTenancySides.Host);
            equipTimetables.CreateChildPermission(AppPermissions.Pages_EquipTimetables_Delete, L("DeleteEquipTimetables"), multiTenancySides: MultiTenancySides.Host);

            var timetables = pages.CreateChildPermission(AppPermissions.Pages_Timetables, L("Timetables"), multiTenancySides: MultiTenancySides.Host);
            timetables.CreateChildPermission(AppPermissions.Pages_Timetables_Create, L("CreateNewTimetables"), multiTenancySides: MultiTenancySides.Host);
            timetables.CreateChildPermission(AppPermissions.Pages_Timetables_Edit, L("EditTimetables"), multiTenancySides: MultiTenancySides.Host);
            timetables.CreateChildPermission(AppPermissions.Pages_Timetables_Delete, L("DeleteTimetables"), multiTenancySides: MultiTenancySides.Host);

            var costTypese = pages.CreateChildPermission(AppPermissions.Pages_CostTypese, L("CostTypese"), multiTenancySides: MultiTenancySides.Host);
            costTypese.CreateChildPermission(AppPermissions.Pages_CostTypese_Create, L("CreateNewCostTypes"), multiTenancySides: MultiTenancySides.Host);
            costTypese.CreateChildPermission(AppPermissions.Pages_CostTypese_Edit, L("EditCostTypes"), multiTenancySides: MultiTenancySides.Host);
            costTypese.CreateChildPermission(AppPermissions.Pages_CostTypese_Delete, L("DeleteCostTypes"), multiTenancySides: MultiTenancySides.Host);

            var accounts = pages.CreateChildPermission(AppPermissions.Pages_Accounts, L("Accounts"), multiTenancySides: MultiTenancySides.Host);
            accounts.CreateChildPermission(AppPermissions.Pages_Accounts_Create, L("CreateNewAccounts"), multiTenancySides: MultiTenancySides.Host);
            accounts.CreateChildPermission(AppPermissions.Pages_Accounts_Edit, L("EditAccounts"), multiTenancySides: MultiTenancySides.Host);
            accounts.CreateChildPermission(AppPermissions.Pages_Accounts_Delete, L("DeleteAccounts"), multiTenancySides: MultiTenancySides.Host);

            var unionPayRates = pages.CreateChildPermission(AppPermissions.Pages_UnionPayRates, L("UnionPayRates"), multiTenancySides: MultiTenancySides.Host);
            unionPayRates.CreateChildPermission(AppPermissions.Pages_UnionPayRates_Create, L("CreateNewUnionPayRates"), multiTenancySides: MultiTenancySides.Host);
            unionPayRates.CreateChildPermission(AppPermissions.Pages_UnionPayRates_Edit, L("EditUnionPayRates"), multiTenancySides: MultiTenancySides.Host);
            unionPayRates.CreateChildPermission(AppPermissions.Pages_UnionPayRates_Delete, L("DeleteUnionPayRates"), multiTenancySides: MultiTenancySides.Host);

            var payperiodHistories = pages.CreateChildPermission(AppPermissions.Pages_PayperiodHistories, L("PayperiodHistories"), multiTenancySides: MultiTenancySides.Host);
            payperiodHistories.CreateChildPermission(AppPermissions.Pages_PayperiodHistories_Create, L("CreateNewPayperiodHistories"), multiTenancySides: MultiTenancySides.Host);
            payperiodHistories.CreateChildPermission(AppPermissions.Pages_PayperiodHistories_Edit, L("EditPayperiodHistories"), multiTenancySides: MultiTenancySides.Host);
            payperiodHistories.CreateChildPermission(AppPermissions.Pages_PayperiodHistories_Delete, L("DeletePayperiodHistories"), multiTenancySides: MultiTenancySides.Host);

            var statusUpdates = pages.CreateChildPermission(AppPermissions.Pages_StatusUpdates, L("StatusUpdates"), multiTenancySides: MultiTenancySides.Host);
            statusUpdates.CreateChildPermission(AppPermissions.Pages_StatusUpdates_Create, L("CreateNewStatusUpdates"), multiTenancySides: MultiTenancySides.Host);
            statusUpdates.CreateChildPermission(AppPermissions.Pages_StatusUpdates_Edit, L("EditStatusUpdates"), multiTenancySides: MultiTenancySides.Host);
            statusUpdates.CreateChildPermission(AppPermissions.Pages_StatusUpdates_Delete, L("DeleteStatusUpdates"), multiTenancySides: MultiTenancySides.Host);

            var shiftExpenses = pages.CreateChildPermission(AppPermissions.Pages_ShiftExpenses, L("ShiftExpenses"), multiTenancySides: MultiTenancySides.Host);
            shiftExpenses.CreateChildPermission(AppPermissions.Pages_ShiftExpenses_Create, L("CreateNewShiftExpenses"), multiTenancySides: MultiTenancySides.Host);
            shiftExpenses.CreateChildPermission(AppPermissions.Pages_ShiftExpenses_Edit, L("EditShiftExpenses"), multiTenancySides: MultiTenancySides.Host);
            shiftExpenses.CreateChildPermission(AppPermissions.Pages_ShiftExpenses_Delete, L("DeleteShiftExpenses"), multiTenancySides: MultiTenancySides.Host);

            var employeeUnions = pages.CreateChildPermission(AppPermissions.Pages_EmployeeUnions, L("EmployeeUnions"), multiTenancySides: MultiTenancySides.Host);
            employeeUnions.CreateChildPermission(AppPermissions.Pages_EmployeeUnions_Create, L("CreateNewEmployeeUnions"), multiTenancySides: MultiTenancySides.Host);
            employeeUnions.CreateChildPermission(AppPermissions.Pages_EmployeeUnions_Edit, L("EditEmployeeUnions"), multiTenancySides: MultiTenancySides.Host);
            employeeUnions.CreateChildPermission(AppPermissions.Pages_EmployeeUnions_Delete, L("DeleteEmployeeUnions"), multiTenancySides: MultiTenancySides.Host);

            var jobUnions = pages.CreateChildPermission(AppPermissions.Pages_JobUnions, L("JobUnions"), multiTenancySides: MultiTenancySides.Host);
            jobUnions.CreateChildPermission(AppPermissions.Pages_JobUnions_Create, L("CreateNewJobUnions"), multiTenancySides: MultiTenancySides.Host);
            jobUnions.CreateChildPermission(AppPermissions.Pages_JobUnions_Edit, L("EditJobUnions"), multiTenancySides: MultiTenancySides.Host);
            jobUnions.CreateChildPermission(AppPermissions.Pages_JobUnions_Delete, L("DeleteJobUnions"), multiTenancySides: MultiTenancySides.Host);

            var unions = pages.CreateChildPermission(AppPermissions.Pages_Unions, L("Unions"), multiTenancySides: MultiTenancySides.Host);
            unions.CreateChildPermission(AppPermissions.Pages_Unions_Create, L("CreateNewUnions"), multiTenancySides: MultiTenancySides.Host);
            unions.CreateChildPermission(AppPermissions.Pages_Unions_Edit, L("EditUnions"), multiTenancySides: MultiTenancySides.Host);
            unions.CreateChildPermission(AppPermissions.Pages_Unions_Delete, L("DeleteUnions"), multiTenancySides: MultiTenancySides.Host);

            var equipments = pages.CreateChildPermission(AppPermissions.Pages_EQUIPMENTS, L("EQUIPMENTS"), multiTenancySides: MultiTenancySides.Host);
            equipments.CreateChildPermission(AppPermissions.Pages_EQUIPMENTS_Create, L("CreateNewEQUIPMENT"), multiTenancySides: MultiTenancySides.Host);
            equipments.CreateChildPermission(AppPermissions.Pages_EQUIPMENTS_Edit, L("EditEQUIPMENT"), multiTenancySides: MultiTenancySides.Host);
            equipments.CreateChildPermission(AppPermissions.Pages_EQUIPMENTS_Delete, L("DeleteEQUIPMENT"), multiTenancySides: MultiTenancySides.Host);

            var eccosts = pages.CreateChildPermission(AppPermissions.Pages_ECCOSTS, L("ECCOSTS"), multiTenancySides: MultiTenancySides.Host);
            eccosts.CreateChildPermission(AppPermissions.Pages_ECCOSTS_Create, L("CreateNewECCOST"), multiTenancySides: MultiTenancySides.Host);
            eccosts.CreateChildPermission(AppPermissions.Pages_ECCOSTS_Edit, L("EditECCOST"), multiTenancySides: MultiTenancySides.Host);
            eccosts.CreateChildPermission(AppPermissions.Pages_ECCOSTS_Delete, L("DeleteECCOST"), multiTenancySides: MultiTenancySides.Host);

            var shiftResources = pages.CreateChildPermission(AppPermissions.Pages_ShiftResources, L("ShiftResources"), multiTenancySides: MultiTenancySides.Host);
            shiftResources.CreateChildPermission(AppPermissions.Pages_ShiftResources_Create, L("CreateNewShiftResources"), multiTenancySides: MultiTenancySides.Host);
            shiftResources.CreateChildPermission(AppPermissions.Pages_ShiftResources_Edit, L("EditShiftResources"), multiTenancySides: MultiTenancySides.Host);
            shiftResources.CreateChildPermission(AppPermissions.Pages_ShiftResources_Delete, L("DeleteShiftResources"), multiTenancySides: MultiTenancySides.Host);

            var jobCategories = pages.CreateChildPermission(AppPermissions.Pages_JobCategories, L("JobCategories"), multiTenancySides: MultiTenancySides.Host);
            jobCategories.CreateChildPermission(AppPermissions.Pages_JobCategories_Create, L("CreateNewJobCategories"), multiTenancySides: MultiTenancySides.Host);
            jobCategories.CreateChildPermission(AppPermissions.Pages_JobCategories_Edit, L("EditJobCategories"), multiTenancySides: MultiTenancySides.Host);
            jobCategories.CreateChildPermission(AppPermissions.Pages_JobCategories_Delete, L("DeleteJobCategories"), multiTenancySides: MultiTenancySides.Host);

            var jobPhaseCodes = pages.CreateChildPermission(AppPermissions.Pages_JobPhaseCodes, L("JobPhaseCodes"), multiTenancySides: MultiTenancySides.Host);
            jobPhaseCodes.CreateChildPermission(AppPermissions.Pages_JobPhaseCodes_Create, L("CreateNewJobPhaseCodes"), multiTenancySides: MultiTenancySides.Host);
            jobPhaseCodes.CreateChildPermission(AppPermissions.Pages_JobPhaseCodes_Edit, L("EditJobPhaseCodes"), multiTenancySides: MultiTenancySides.Host);
            jobPhaseCodes.CreateChildPermission(AppPermissions.Pages_JobPhaseCodes_Delete, L("DeleteJobPhaseCodes"), multiTenancySides: MultiTenancySides.Host);

            var shifts = pages.CreateChildPermission(AppPermissions.Pages_Shifts, L("Shifts"), multiTenancySides: MultiTenancySides.Host);
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Create, L("CreateNewShifts"), multiTenancySides: MultiTenancySides.Host);
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Edit, L("EditShifts"), multiTenancySides: MultiTenancySides.Host);
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Delete, L("DeleteShifts"), multiTenancySides: MultiTenancySides.Host);

            var payPeriods = pages.CreateChildPermission(AppPermissions.Pages_PayPeriods, L("PayPeriods"), multiTenancySides: MultiTenancySides.Host);
            payPeriods.CreateChildPermission(AppPermissions.Pages_PayPeriods_Create, L("CreateNewPayPeriods"), multiTenancySides: MultiTenancySides.Host);
            payPeriods.CreateChildPermission(AppPermissions.Pages_PayPeriods_Edit, L("EditPayPeriods"), multiTenancySides: MultiTenancySides.Host);
            payPeriods.CreateChildPermission(AppPermissions.Pages_PayPeriods_Delete, L("DeletePayPeriods"), multiTenancySides: MultiTenancySides.Host);

            var timesheets = pages.CreateChildPermission(AppPermissions.Pages_Timesheets, L("Timesheets"), multiTenancySides: MultiTenancySides.Host);
            timesheets.CreateChildPermission(AppPermissions.Pages_Timesheets_Create, L("CreateNewTimesheets"), multiTenancySides: MultiTenancySides.Host);
            timesheets.CreateChildPermission(AppPermissions.Pages_Timesheets_Edit, L("EditTimesheets"), multiTenancySides: MultiTenancySides.Host);
            timesheets.CreateChildPermission(AppPermissions.Pages_Timesheets_Delete, L("DeleteTimesheets"), multiTenancySides: MultiTenancySides.Host);

            var statuses = pages.CreateChildPermission(AppPermissions.Pages_Statuses, L("Statuses"), multiTenancySides: MultiTenancySides.Host);
            statuses.CreateChildPermission(AppPermissions.Pages_Statuses_Create, L("CreateNewStatuses"), multiTenancySides: MultiTenancySides.Host);
            statuses.CreateChildPermission(AppPermissions.Pages_Statuses_Edit, L("EditStatuses"), multiTenancySides: MultiTenancySides.Host);
            statuses.CreateChildPermission(AppPermissions.Pages_Statuses_Delete, L("DeleteStatuses"), multiTenancySides: MultiTenancySides.Host);

            var expenseTypeses = pages.CreateChildPermission(AppPermissions.Pages_ExpenseTypeses, L("ExpenseTypeses"), multiTenancySides: MultiTenancySides.Host);
            expenseTypeses.CreateChildPermission(AppPermissions.Pages_ExpenseTypeses_Create, L("CreateNewExpenseTypes"), multiTenancySides: MultiTenancySides.Host);
            expenseTypeses.CreateChildPermission(AppPermissions.Pages_ExpenseTypeses_Edit, L("EditExpenseTypes"), multiTenancySides: MultiTenancySides.Host);
            expenseTypeses.CreateChildPermission(AppPermissions.Pages_ExpenseTypeses_Delete, L("DeleteExpenseTypes"), multiTenancySides: MultiTenancySides.Host);

            var payTypeses = pages.CreateChildPermission(AppPermissions.Pages_PayTypeses, L("PayTypeses"), multiTenancySides: MultiTenancySides.Host);
            payTypeses.CreateChildPermission(AppPermissions.Pages_PayTypeses_Create, L("CreateNewPayTypes"), multiTenancySides: MultiTenancySides.Host);
            payTypeses.CreateChildPermission(AppPermissions.Pages_PayTypeses_Edit, L("EditPayTypes"), multiTenancySides: MultiTenancySides.Host);
            payTypeses.CreateChildPermission(AppPermissions.Pages_PayTypeses_Delete, L("DeletePayTypes"), multiTenancySides: MultiTenancySides.Host);

            var resourceReservationses = pages.CreateChildPermission(AppPermissions.Pages_ResourceReservationses, L("ResourceReservationses"), multiTenancySides: MultiTenancySides.Host);
            resourceReservationses.CreateChildPermission(AppPermissions.Pages_ResourceReservationses_Create, L("CreateNewResourceReservations"), multiTenancySides: MultiTenancySides.Host);
            resourceReservationses.CreateChildPermission(AppPermissions.Pages_ResourceReservationses_Edit, L("EditResourceReservations"), multiTenancySides: MultiTenancySides.Host);
            resourceReservationses.CreateChildPermission(AppPermissions.Pages_ResourceReservationses_Delete, L("DeleteResourceReservations"), multiTenancySides: MultiTenancySides.Host);

            var jobses = pages.CreateChildPermission(AppPermissions.Pages_Jobses, L("Jobses"), multiTenancySides: MultiTenancySides.Host);
            jobses.CreateChildPermission(AppPermissions.Pages_Jobses_Create, L("CreateNewJobs"), multiTenancySides: MultiTenancySides.Host);
            jobses.CreateChildPermission(AppPermissions.Pages_Jobses_Edit, L("EditJobs"), multiTenancySides: MultiTenancySides.Host);
            jobses.CreateChildPermission(AppPermissions.Pages_Jobses_Delete, L("DeleteJobs"), multiTenancySides: MultiTenancySides.Host);

            var jobClasseses = pages.CreateChildPermission(AppPermissions.Pages_JobClasseses, L("JobClasseses"), multiTenancySides: MultiTenancySides.Host);
            jobClasseses.CreateChildPermission(AppPermissions.Pages_JobClasseses_Create, L("CreateNewJobClasses"), multiTenancySides: MultiTenancySides.Host);
            jobClasseses.CreateChildPermission(AppPermissions.Pages_JobClasseses_Edit, L("EditJobClasses"), multiTenancySides: MultiTenancySides.Host);
            jobClasseses.CreateChildPermission(AppPermissions.Pages_JobClasseses_Delete, L("DeleteJobClasses"), multiTenancySides: MultiTenancySides.Host);

            var addresseses = pages.CreateChildPermission(AppPermissions.Pages_Addresseses, L("Addresseses"), multiTenancySides: MultiTenancySides.Host);
            addresseses.CreateChildPermission(AppPermissions.Pages_Addresseses_Create, L("CreateNewAddresses"), multiTenancySides: MultiTenancySides.Host);
            addresseses.CreateChildPermission(AppPermissions.Pages_Addresseses_Edit, L("EditAddresses"), multiTenancySides: MultiTenancySides.Host);
            addresseses.CreateChildPermission(AppPermissions.Pages_Addresseses_Delete, L("DeleteAddresses"), multiTenancySides: MultiTenancySides.Host);

            var resourceEquipmentInfoses = pages.CreateChildPermission(AppPermissions.Pages_ResourceEquipmentInfoses, L("ResourceEquipmentInfoses"), multiTenancySides: MultiTenancySides.Host);
            resourceEquipmentInfoses.CreateChildPermission(AppPermissions.Pages_ResourceEquipmentInfoses_Create, L("CreateNewResourceEquipmentInfos"), multiTenancySides: MultiTenancySides.Host);
            resourceEquipmentInfoses.CreateChildPermission(AppPermissions.Pages_ResourceEquipmentInfoses_Edit, L("EditResourceEquipmentInfos"), multiTenancySides: MultiTenancySides.Host);
            resourceEquipmentInfoses.CreateChildPermission(AppPermissions.Pages_ResourceEquipmentInfoses_Delete, L("DeleteResourceEquipmentInfos"), multiTenancySides: MultiTenancySides.Host);

            var resourceWorkerInfoses = pages.CreateChildPermission(AppPermissions.Pages_ResourceWorkerInfoses, L("ResourceWorkerInfoses"), multiTenancySides: MultiTenancySides.Host);
            resourceWorkerInfoses.CreateChildPermission(AppPermissions.Pages_ResourceWorkerInfoses_Create, L("CreateNewResourceWorkerInfos"), multiTenancySides: MultiTenancySides.Host);
            resourceWorkerInfoses.CreateChildPermission(AppPermissions.Pages_ResourceWorkerInfoses_Edit, L("EditResourceWorkerInfos"), multiTenancySides: MultiTenancySides.Host);
            resourceWorkerInfoses.CreateChildPermission(AppPermissions.Pages_ResourceWorkerInfoses_Delete, L("DeleteResourceWorkerInfos"), multiTenancySides: MultiTenancySides.Host);

            var workerClaseeses = pages.CreateChildPermission(AppPermissions.Pages_WorkerClaseeses, L("WorkerClaseeses"), multiTenancySides: MultiTenancySides.Host);
            workerClaseeses.CreateChildPermission(AppPermissions.Pages_WorkerClaseeses_Create, L("CreateNewWorkerClasees"), multiTenancySides: MultiTenancySides.Host);
            workerClaseeses.CreateChildPermission(AppPermissions.Pages_WorkerClaseeses_Edit, L("EditWorkerClasees"), multiTenancySides: MultiTenancySides.Host);
            workerClaseeses.CreateChildPermission(AppPermissions.Pages_WorkerClaseeses_Delete, L("DeleteWorkerClasees"), multiTenancySides: MultiTenancySides.Host);

            var resourceses = pages.CreateChildPermission(AppPermissions.Pages_Resourceses, L("Resourceses"), multiTenancySides: MultiTenancySides.Host);
            resourceses.CreateChildPermission(AppPermissions.Pages_Resourceses_Create, L("CreateNewResources"), multiTenancySides: MultiTenancySides.Host);
            resourceses.CreateChildPermission(AppPermissions.Pages_Resourceses_Edit, L("EditResources"), multiTenancySides: MultiTenancySides.Host);
            resourceses.CreateChildPermission(AppPermissions.Pages_Resourceses_Delete, L("DeleteResources"), multiTenancySides: MultiTenancySides.Host);

            var prdedrates = pages.CreateChildPermission(AppPermissions.Pages_PRDEDRATES, L("PRDEDRATES"));
            prdedrates.CreateChildPermission(AppPermissions.Pages_PRDEDRATES_Create, L("CreateNewPRDEDRATE"));
            prdedrates.CreateChildPermission(AppPermissions.Pages_PRDEDRATES_Edit, L("EditPRDEDRATE"));
            prdedrates.CreateChildPermission(AppPermissions.Pages_PRDEDRATES_Delete, L("DeletePRDEDRATE"));

            var jcjoBs = pages.CreateChildPermission(AppPermissions.Pages_JCJOBs, L("JCJOBs"));
            jcjoBs.CreateChildPermission(AppPermissions.Pages_JCJOBs_Create, L("CreateNewJCJOB"));
            jcjoBs.CreateChildPermission(AppPermissions.Pages_JCJOBs_Edit, L("EditJCJOB"));
            jcjoBs.CreateChildPermission(AppPermissions.Pages_JCJOBs_Delete, L("DeleteJCJOB"));

            var prclasSs = pages.CreateChildPermission(AppPermissions.Pages_PRCLASSs, L("PRCLASSs"));
            prclasSs.CreateChildPermission(AppPermissions.Pages_PRCLASSs_Create, L("CreateNewPRCLASS"));
            prclasSs.CreateChildPermission(AppPermissions.Pages_PRCLASSs_Edit, L("EditPRCLASS"));
            prclasSs.CreateChildPermission(AppPermissions.Pages_PRCLASSs_Delete, L("DeletePRCLASS"));

            var jcunioNs = pages.CreateChildPermission(AppPermissions.Pages_JCUNIONs, L("JCUNIONs"));
            jcunioNs.CreateChildPermission(AppPermissions.Pages_JCUNIONs_Create, L("CreateNewJCUNION"));
            jcunioNs.CreateChildPermission(AppPermissions.Pages_JCUNIONs_Edit, L("EditJCUNION"));
            jcunioNs.CreateChildPermission(AppPermissions.Pages_JCUNIONs_Delete, L("DeleteJCUNION"));

            var jaccaTs = pages.CreateChildPermission(AppPermissions.Pages_JACCATs, L("JACCATs"));
            jaccaTs.CreateChildPermission(AppPermissions.Pages_JACCATs_Create, L("CreateNewJACCAT"));
            jaccaTs.CreateChildPermission(AppPermissions.Pages_JACCATs_Edit, L("EditJACCAT"));
            jaccaTs.CreateChildPermission(AppPermissions.Pages_JACCATs_Delete, L("DeleteJACCAT"));

            var premployees = pages.CreateChildPermission(AppPermissions.Pages_PREMPLOYEES, L("PREMPLOYEES"));
            premployees.CreateChildPermission(AppPermissions.Pages_PREMPLOYEES_Create, L("CreateNewPREMPLOYEE"));
            premployees.CreateChildPermission(AppPermissions.Pages_PREMPLOYEES_Edit, L("EditPREMPLOYEE"));
            premployees.CreateChildPermission(AppPermissions.Pages_PREMPLOYEES_Delete, L("DeletePREMPLOYEE"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, NucleusConsts.LocalizationSourceName);
        }
    }
}
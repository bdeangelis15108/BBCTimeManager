using Nucleus.EquipTimetable.Dtos;
using Nucleus.EquipTimetable;
using Nucleus.Timetable.Dtos;
using Nucleus.Timetable;
using Nucleus.CostType.Dtos;
using Nucleus.CostType;
using Nucleus.Account.Dtos;
using Nucleus.Account;
using Nucleus.UnionPayRate.Dtos;
using Nucleus.UnionPayRate;
using Nucleus.PayperiodHistory.Dtos;
using Nucleus.PayperiodHistory;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.StatusUpdate;
using Nucleus.ShiftExpense.Dtos;
using Nucleus.ShiftExpense;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.EmployeeUnion;
using Nucleus.JobUnion.Dtos;
using Nucleus.JobUnion;
using Nucleus.Union.Dtos;
using Nucleus.Union;
using Nucleus.ShiftResource.Dtos;
using Nucleus.ShiftResource;
using Nucleus.JobCategory.Dtos;
using Nucleus.JobCategory;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.JobPhaseCode;
using Nucleus.Shift.Dtos;
using Nucleus.Shift;
using Nucleus.PayPeriod.Dtos;
using Nucleus.PayPeriod;
using Nucleus.Timesheet.Dtos;
using Nucleus.Timesheet;
using Nucleus.Status.Dtos;
using Nucleus.Status;
using Nucleus.ExpenseType.Dtos;
using Nucleus.ExpenseType;
using Nucleus.PayType.Dtos;
using Nucleus.PayType;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.Job.Dtos;
using Nucleus.Job;
using Nucleus.JobClass.Dtos;
using Nucleus.JobClass;
using Nucleus.Address.Dtos;
using Nucleus.Address;
using Nucleus.ResourceEquipmentInfo.Dtos;
using Nucleus.ResourceEquipmentInfo;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.ResourceWorkerInfo;
using Nucleus.WorkerClasee.Dtos;
using Nucleus.WorkerClasee;
using Nucleus.Resource.Dtos;
using Nucleus.Resource;
using Nucleus.ECCOSTS.Dtos;
using Nucleus.ECCOSTS;
using Nucleus.EQUIPMENTS.Dtos;
using Nucleus.EQUIPMENTS;
using Nucleus.PRDEDRATES.Dtos;
using Nucleus.PRDEDRATES;
using Nucleus.JCJOBS.Dtos;
using Nucleus.JCJOBS;
using Nucleus.PRCLASSES.Dtos;
using Nucleus.PRCLASSES;
using Nucleus.JCUNIONS.Dtos;
using Nucleus.JCUNIONS;
using Nucleus.JCCAT.Dtos;
using Nucleus.JCCAT;
using Nucleus.PREMPLOYEES.Dtos;
using Nucleus.PREMPLOYEES;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using AutoMapper;
using Nucleus.Auditing.Dto;
using Nucleus.Authorization.Accounts.Dto;
using Nucleus.Authorization.Permissions.Dto;
using Nucleus.Authorization.Roles;
using Nucleus.Authorization.Roles.Dto;
using Nucleus.Authorization.Users;
using Nucleus.Authorization.Users.Dto;
using Nucleus.Authorization.Users.Importing.Dto;
using Nucleus.Authorization.Users.Profile.Dto;
using Nucleus.Chat;
using Nucleus.Chat.Dto;
using Nucleus.Editions;
using Nucleus.Editions.Dto;
using Nucleus.Friendships;
using Nucleus.Friendships.Cache;
using Nucleus.Friendships.Dto;
using Nucleus.Localization.Dto;
using Nucleus.MultiTenancy;
using Nucleus.MultiTenancy.Dto;
using Nucleus.MultiTenancy.HostDashboard.Dto;
using Nucleus.MultiTenancy.Payments;
using Nucleus.MultiTenancy.Payments.Dto;
using Nucleus.Notifications.Dto;
using Nucleus.Organizations.Dto;
using Nucleus.Sessions.Dto;

namespace Nucleus
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
           configuration.CreateMap<CreateOrEditResourceWorkerInfosDto, ResourceWorkerInfos>();
            configuration.CreateMap<CreateOrEditEquipTimetablesDto, EquipTimetables>().ReverseMap();
            configuration.CreateMap<EquipTimetablesDto, EquipTimetables>().ReverseMap();
            configuration.CreateMap<CreateOrEditTimetablesDto, Timetables>().ReverseMap();
            configuration.CreateMap<TimetablesDto, Timetables>().ReverseMap();
            configuration.CreateMap<CreateOrEditCostTypesDto, CostTypes>().ReverseMap();
            configuration.CreateMap<CostTypesDto, CostTypes>().ReverseMap();
            configuration.CreateMap<CreateOrEditAccountsDto, Accounts>().ReverseMap();
            configuration.CreateMap<AccountsDto, Accounts>().ReverseMap();
            configuration.CreateMap<CreateOrEditUnionPayRatesDto, UnionPayRates>().ReverseMap();
            configuration.CreateMap<UnionPayRatesDto, UnionPayRates>().ReverseMap();
            configuration.CreateMap<CreateOrEditPayperiodHistoriesDto, PayperiodHistories>().ReverseMap();
            configuration.CreateMap<PayperiodHistoriesDto, PayperiodHistories>().ReverseMap();
            configuration.CreateMap<CreateOrEditStatusUpdatesDto, StatusUpdates>().ReverseMap();
            configuration.CreateMap<StatusUpdatesDto, StatusUpdates>().ReverseMap();
            configuration.CreateMap<CreateOrEditShiftExpensesDto, ShiftExpenses>().ReverseMap();
            configuration.CreateMap<ShiftExpensesDto, ShiftExpenses>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeUnionsDto, EmployeeUnions>().ReverseMap();
            configuration.CreateMap<EmployeeUnionsDto, EmployeeUnions>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobUnionsDto, JobUnions>().ReverseMap();
            configuration.CreateMap<JobUnionsDto, JobUnions>().ReverseMap();
            configuration.CreateMap<CreateOrEditUnionsDto, Unions>().ReverseMap();
            configuration.CreateMap<UnionsDto, Unions>().ReverseMap();
            configuration.CreateMap<CreateOrEditShiftResourcesDto, ShiftResources>().ReverseMap();
            configuration.CreateMap<ShiftResourcesDto, ShiftResources>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobCategoriesDto, JobCategories>().ReverseMap();
            configuration.CreateMap<JobCategoriesDto, JobCategories>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobPhaseCodesDto, JobPhaseCodes>().ReverseMap();
            configuration.CreateMap<JobPhaseCodesDto, JobPhaseCodes>().ReverseMap();
            configuration.CreateMap<CreateOrEditShiftsDto, Shifts>().ReverseMap();
            configuration.CreateMap<ShiftsDto, Shifts>().ReverseMap();
            configuration.CreateMap<CreateOrEditPayPeriodsDto, PayPeriods>().ReverseMap();
            configuration.CreateMap<PayPeriodsDto, PayPeriods>().ReverseMap();
            configuration.CreateMap<CreateOrEditTimesheetsDto, Timesheets>().ReverseMap();
            configuration.CreateMap<TimesheetsDto, Timesheets>().ReverseMap();
            configuration.CreateMap<CreateOrEditStatusesDto, Statuses>().ReverseMap();
            configuration.CreateMap<StatusesDto, Statuses>().ReverseMap();
            configuration.CreateMap<CreateOrEditExpenseTypesDto, ExpenseTypes>().ReverseMap();
            configuration.CreateMap<ExpenseTypesDto, ExpenseTypes>().ReverseMap();
            configuration.CreateMap<CreateOrEditPayTypesDto, PayTypes>().ReverseMap();
            configuration.CreateMap<PayTypesDto, PayTypes>().ReverseMap();
            configuration.CreateMap<CreateOrEditResourceReservationsDto, ResourceReservations>().ReverseMap();
            configuration.CreateMap<ResourceReservationsDto, ResourceReservations>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobsDto, Jobs>().ReverseMap();
            configuration.CreateMap<JobsDto, Jobs>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobClassesDto, JobClasses>().ReverseMap();
            configuration.CreateMap<JobClassesDto, JobClasses>().ReverseMap();
            configuration.CreateMap<CreateOrEditAddressesDto, Addresses>().ReverseMap();
            configuration.CreateMap<AddressesDto, Addresses>().ReverseMap();
            configuration.CreateMap<CreateOrEditResourceEquipmentInfosDto, ResourceEquipmentInfos>().ReverseMap();
            configuration.CreateMap<ResourceEquipmentInfosDto, ResourceEquipmentInfos>().ReverseMap();
            configuration.CreateMap<CreateOrEditResourceWorkerInfosDto, ResourceWorkerInfos>().ReverseMap();
            configuration.CreateMap<ResourceWorkerInfosDto, ResourceWorkerInfos>().ReverseMap();
            configuration.CreateMap<CreateOrEditWorkerClaseesDto, WorkerClasees>().ReverseMap();
            configuration.CreateMap<WorkerClaseesDto, WorkerClasees>().ReverseMap();
            configuration.CreateMap<CreateOrEditResourcesDto, Resources>().ReverseMap();
            configuration.CreateMap<ResourcesDto, Resources>().ReverseMap();
            configuration.CreateMap<CreateOrEditECCOSTDto, ECCOST>().ReverseMap();
            configuration.CreateMap<ECCOSTDto, ECCOST>().ReverseMap();
            configuration.CreateMap<CreateOrEditEQUIPMENTDto, EQUIPMENT>().ReverseMap();
            configuration.CreateMap<EQUIPMENTDto, EQUIPMENT>().ReverseMap();
            configuration.CreateMap<CreateOrEditPRDEDRATEDto, PRDEDRATE>().ReverseMap();
            configuration.CreateMap<PRDEDRATEDto, PRDEDRATE>().ReverseMap();
            configuration.CreateMap<CreateOrEditJCJOBDto, JCJOB>().ReverseMap();
            configuration.CreateMap<JCJOBDto, JCJOB>().ReverseMap();
            configuration.CreateMap<CreateOrEditPRCLASSDto, PRCLASS>().ReverseMap();
            configuration.CreateMap<PRCLASSDto, PRCLASS>().ReverseMap();
            configuration.CreateMap<CreateOrEditJCUNIONDto, JCUNION>().ReverseMap();
            configuration.CreateMap<JCUNIONDto, JCUNION>().ReverseMap();
            configuration.CreateMap<CreateOrEditJACCATDto, JACCAT>().ReverseMap();
            configuration.CreateMap<JACCATDto, JACCAT>().ReverseMap();
            configuration.CreateMap<CreateOrEditPREMPLOYEEDto, PREMPLOYEE>().ReverseMap();
            configuration.CreateMap<PREMPLOYEEDto, PREMPLOYEE>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
            //Employees value
            configuration.CreateMap<ResourceWorkerInfos, ResourceWorkerInfosDto>();
        }
    }
}
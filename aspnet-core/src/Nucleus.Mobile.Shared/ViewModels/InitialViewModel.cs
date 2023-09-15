using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Core.Threading;
using Nucleus.Models.NavigationMenu;
using Nucleus.Services.Navigation;
using Nucleus.ViewModels.Base;
using Xamarin.Forms.Internals;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.Job;
using Nucleus.Job.Dtos;
using MonkeyCache.SQLite;
using System;
using Plugin.Connectivity;
using Nucleus.ResourceWorkerInfo;
using Nucleus.ResourceReservation;
using Nucleus.Resource;
using Nucleus.PayPeriod.Dtos;
using Nucleus.PayPeriod;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.StatusUpdate;
using Nucleus.JobCategory;
using Nucleus.JobCategory.Dtos;
using Nucleus.JobPhaseCode;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.Timesheet;
using Nucleus.Timesheet.Dtos;
using Nucleus.ShiftResource;
using Nucleus.ShiftResource.Dtos;
using Nucleus.Shift;
using Nucleus.Shift.Dtos;
using Nucleus.ShiftExpense;
using Nucleus.ShiftExpense.Dtos;
using Nucleus.Status.Dtos;
using Nucleus.Status;
using Nucleus.ExpenseType;
using Nucleus.ExpenseType.Dtos;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;

namespace Nucleus.ViewModels
{
    public class InitialViewModel : XamarinViewModel
    {
        private readonly IMenuProvider _menuProvider;
        private readonly IApplicationContext _appContext;
        private bool _noAuthorizedMenuItem;

        #region getdb
        private readonly IJobsesAppService _jobsesAppService;
        private readonly IResourceWorkerInfosesAppService _resourceWorkerInfosesAppService;
        private readonly IResourcesesAppService _resourcesesAppService;
        private readonly IResourceReservationsesAppService _resourceReservationService;
        private readonly IPayPeriodsAppService _payPeriodsAppService;
        private readonly IStatusUpdatesAppService _statusUpdatesAppService;
        private readonly IApplicationContext _applicationContext;
        private readonly IJobCategoriesAppService _jobCategoriesAppService;
        private readonly IJobPhaseCodesAppService _jobPhaseCodesAppService;
        private readonly ITimesheetsAppService _timesheetsAppService;
        private readonly IShiftResourcesAppService _shiftResourcesAppService;
        private readonly IShiftsAppService _shiftsAppService;
        private readonly IShiftExpensesAppService _shiftExpensesAppService;
        private readonly IExpenseTypesesAppService _expenseTypesesAppService;
        private readonly IPayTypesesAppService _payTypesAppService;
        //private readonly IStatusesAppService _statusesAppService;

        private readonly GetAllJobsesInput getAllJobsesInput;
        private GetAllResourceWorkerInfosesInput getAllResourceWorkerInfosesInput { get; set; }
        public GetAllResourceReservationsesInput getAllResourceReservationsesInput { get; set; }
        private GetAllResourcesesInput getAllResourcesesInput { get; set; }
        private GetAllPayPeriodsInput getAllPayPeriodsInput { get; set; }
        #endregion

        

        public bool NoAuthorizedMenuItem
        {
            get => _noAuthorizedMenuItem;
            set
            {
                _noAuthorizedMenuItem = value;
                RaisePropertyChanged(() => NoAuthorizedMenuItem);
            }
        }

        public ICommand PageAppearingCommand => AsyncCommand.Create(SetInitialPageAsync);

        public InitialViewModel(IMenuProvider menuProvider, IApplicationContext appContext, IShiftResourcesAppService shiftResourcesAppService, ITimesheetsAppService timesheetsAppService, 
            IJobPhaseCodesAppService jobPhaseCodesAppService, IJobCategoriesAppService jobCategoriesAppService, IStatusUpdatesAppService statusUpdatesAppService, IApplicationContext applicationContext, 
            IJobsesAppService jobsesAppService, IResourceWorkerInfosesAppService resourceWorkerInfosesAppService, IResourcesesAppService resourcesesAppService, 
            IResourceReservationsesAppService resourceReservationsesAppService, IPayTypesesAppService payTypesAppService, IExpenseTypesesAppService expenseTypesesAppService, IPayPeriodsAppService payPeriodsAppService, IShiftsAppService shiftsAppService, IShiftExpensesAppService shiftExpensesAppService)
        {
            _menuProvider = menuProvider;
            _appContext = appContext;

            if (CrossConnectivity.Current.IsConnected)
            {
                
                #region getdb
                _jobsesAppService = jobsesAppService;
                _resourceWorkerInfosesAppService = resourceWorkerInfosesAppService;
                _resourcesesAppService = resourcesesAppService;
                _resourceReservationService = resourceReservationsesAppService;
                _payPeriodsAppService = payPeriodsAppService;
                _statusUpdatesAppService = statusUpdatesAppService;
                _applicationContext = applicationContext;
                _jobCategoriesAppService = jobCategoriesAppService;
                _jobPhaseCodesAppService = jobPhaseCodesAppService;
                _timesheetsAppService = timesheetsAppService;
                _shiftResourcesAppService = shiftResourcesAppService;
                _shiftsAppService = shiftsAppService;
                _shiftExpensesAppService = shiftExpensesAppService;
                _expenseTypesesAppService = expenseTypesesAppService;
                _payTypesAppService = payTypesAppService;
                //_statusesAppService = statusesAppService;

                getAllJobsesInput = new GetAllJobsesInput();
                getAllResourceWorkerInfosesInput = new GetAllResourceWorkerInfosesInput
                {
                    MaxResultCount = 10000,
                    SkipCount = 0
                };
                getAllResourceReservationsesInput = new GetAllResourceReservationsesInput();
                getAllResourcesesInput = new GetAllResourcesesInput
                {
                    //TypeFilter = "Equipment",
                    MaxResultCount = 10000,
                };
                getAllPayPeriodsInput = new GetAllPayPeriodsInput
                {
                    IsActiveFilter = 1
                };

                Barrel.Current.Empty("createOrEditTimesheets");
                Barrel.Current.Empty("createOrEditShifts");
                Barrel.Current.Empty("createOrEditShiftExpensesDto");
                Barrel.Current.Empty("createOrEditShiftResourcesDto");
                #endregion
            }

        }


        private async Task FetchJobsAsync()
        {
            await WebRequestExecuter.Execute(async () => await _jobsesAppService.GetAll(getAllJobsesInput), result =>
            {

                Barrel.Current.Empty("JobDB");
                Barrel.Current.Add(key: "JobDB", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetJobsForViewDto>>("JobDB");
                return Task.CompletedTask;
            });
        }

        private async Task FetchEmployeesAsync()
        {

            await WebRequestExecuter.Execute(async () => await _resourceWorkerInfosesAppService.GetAll(getAllResourceWorkerInfosesInput), result =>
            {
                Barrel.Current.Empty("ResourceWorkerInfoses");
                Barrel.Current.Add(key: "ResourceWorkerInfoses", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetResourceWorkerInfosForViewDto>>("ResourceWorkerInfoses");
                return Task.CompletedTask;
            });
        }

        public async Task FetchReservedResource()
        {
            //Get reserved Employees data 
            await WebRequestExecuter.Execute(async () => await _resourceReservationService.GetMyReservedResource(getAllResourceReservationsesInput), result =>
            {
                Barrel.Current.Empty("ResourceReservationses");
                Barrel.Current.Add(key: "ResourceReservationses", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetMyResourceReservationsDto>>("ResourceReservationses");
                return Task.CompletedTask;

            });

        }

        private async Task FetchEquipmentAsync()
        {
            getAllResourcesesInput.TypeFilter = "Equipment";
            await WebRequestExecuter.Execute(async () => await _resourcesesAppService.GetAll(getAllResourcesesInput), result =>
            {
                Barrel.Current.Empty("Resourceses");
                Barrel.Current.Add(key: "Resourceses", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetResourcesForViewDto>>("Resourceses");

                return Task.CompletedTask;
            });
        }

        public async Task populatePayPeriod()
        {
            await WebRequestExecuter.Execute(async () => await _payPeriodsAppService.GetAll(getAllPayPeriodsInput), result =>
            {
                Barrel.Current.Empty("Periods");
                Barrel.Current.Add(key: "Periods", data: result.Items.First(), expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<GetPayPeriodsForViewDto>("Periods");
                return Task.CompletedTask;
            });

        }

        public async Task fechstatusUpdates()
        {
            var res = Barrel.Current.Get<GetPayPeriodsForViewDto>("Periods");
            GetAllStatusUpdatesInput getAllStatusUpdatesInput = new GetAllStatusUpdatesInput
            {
                MaxActualCreateDateTimeFilter = res.PayPeriods.EndDate,
                MinActualCreateDateTimeFilter = res.PayPeriods.StartDate,
                UserNameFilter = _applicationContext.LoginInfo.User.Name
            };
            await WebRequestExecuter.Execute(async () => await _statusUpdatesAppService.GetAll(getAllStatusUpdatesInput), result =>
            {
                Barrel.Current.Empty("StatusUpdates");
                Barrel.Current.Add(key: "StatusUpdates", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetStatusUpdatesForViewDto>>("StatusUpdates");

                return Task.CompletedTask;
            });
        }

        private async Task PopulateJobCategory()
        {
            await WebRequestExecuter.Execute(async () => await _jobCategoriesAppService.GetAll(null), result =>
            {
                Barrel.Current.Empty("JobCategories");
                Barrel.Current.Add(key: "JobCategories", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetJobCategoriesForViewDto>>("JobCategories");
                return Task.CompletedTask;
            });
        }

        private async Task PopulatePhaseCode()
        {
            GetAllJobPhaseCodesInput getAllJobPhaseCodesInput = new GetAllJobPhaseCodesInput();

            await WebRequestExecuter.Execute(async () => await _jobPhaseCodesAppService.GetAll(getAllJobPhaseCodesInput), result =>
            {
                Barrel.Current.Empty("PhaseCode");
                Barrel.Current.Add(key: "PhaseCode", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetJobPhaseCodesForViewDto>>("PhaseCode");
                return Task.CompletedTask;
            });
        }

        private async Task PopulateTimeSheet()
        {
            GetAllTimesheetsInput getAllTimesheetsInput = new GetAllTimesheetsInput();
            await WebRequestExecuter.Execute(async () => await _timesheetsAppService.GetAll(getAllTimesheetsInput), result =>
            {
                Barrel.Current.Empty("TimeSheet");
                Barrel.Current.Add(key: "TimeSheet", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");
                return Task.CompletedTask;
            });

        }

        private async Task populateShiftResources()
        {
            GetAllShiftResourcesInput getAllShiftResourcesInput = new GetAllShiftResourcesInput();
            await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
            {
                Barrel.Current.Empty("ShiftResources");
                Barrel.Current.Add(key: "ShiftResources", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetShiftResourcesForViewDto>>("ShiftResources");
                return Task.CompletedTask;
            });
        }

        private async Task populateShift()
        {
            GetAllShiftsInput getAllShiftsInput = new GetAllShiftsInput();
            await WebRequestExecuter.Execute(async () => await _shiftsAppService.GetAll(getAllShiftsInput), result =>
            {
                Barrel.Current.Empty("Shift");
                Barrel.Current.Add(key: "Shift", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetShiftsForViewDto>>("Shift");
                return Task.CompletedTask;
            });
        }

        private async Task populateShiftExpand()
        {
            GetAllShiftExpensesInput getAllShiftExpensesInput = new GetAllShiftExpensesInput();
            
            await WebRequestExecuter.Execute(async () => await _shiftExpensesAppService.GetAll(getAllShiftExpensesInput), result =>
            {
                Barrel.Current.Empty("ShiftExpand");
                Barrel.Current.Add(key: "ShiftExpand", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetShiftExpensesForViewDto>>("ShiftExpand");
                return Task.CompletedTask;
            });
        }

        private async Task populateExpenseType()
        {
            GetAllExpenseTypesesInput getAllExpenseTypesesInput = new GetAllExpenseTypesesInput();

            await WebRequestExecuter.Execute(async () => await _expenseTypesesAppService.GetAll(getAllExpenseTypesesInput), result =>
            {
                Barrel.Current.Empty("ExpenseType");
                Barrel.Current.Add(key: "ExpenseType", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetExpenseTypesForViewDto>>("ExpenseType");
                return Task.CompletedTask;
            });
        }

        private async Task PopulatePayTypes()
        {

            await WebRequestExecuter.Execute(async () => await _payTypesAppService.GetAll(null), result =>
            {
                Barrel.Current.Empty("PayTypes");
                Barrel.Current.Add(key: "PayTypes", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetPayTypesForViewDto>>("PayTypes");
                return Task.CompletedTask;
            });
        }

        /*private async Task populateStatus()
        {
            GetAllStatusesInput getAllStatusesInput = new GetAllStatusesInput();

            await WebRequestExecuter.Execute(async () => await _statusesAppService.GetAll(getAllStatusesInput), result =>
            {
                Barrel.Current.Empty("Status");
                Barrel.Current.Add(key: "Status", data: result.Items, expireIn: TimeSpan.FromDays(1));
                var response = Barrel.Current.Get<IReadOnlyList<GetStatusesForViewDto>>("Status");
                return Task.CompletedTask;
            });
        }*/

        private async Task SetInitialPageAsync()
        {
            var firstAuthorizedMenuItem = GetFirstAuthorizedMenuItemOrNull(out var authorizedMenuItems);

            NoAuthorizedMenuItem = firstAuthorizedMenuItem == null;

            if (firstAuthorizedMenuItem != null)
            {
                await NavigationService.SetDetailPageAsync(firstAuthorizedMenuItem.ViewType);
                SetSelectedMenuItem(authorizedMenuItems, firstAuthorizedMenuItem);
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                await SetBusyAsync(() =>
                 {
                     AsyncRunner.Run(FetchJobsAsync());
                     AsyncRunner.Run(FetchEmployeesAsync());
                     AsyncRunner.Run(FetchReservedResource());
                     AsyncRunner.Run(FetchEquipmentAsync());
                     AsyncRunner.Run(populatePayPeriod());
                     AsyncRunner.Run(fechstatusUpdates());
                     AsyncRunner.Run(PopulateJobCategory());
                     AsyncRunner.Run(PopulatePhaseCode());
                     AsyncRunner.Run(PopulateTimeSheet());
                     AsyncRunner.Run(populateShiftResources());
                     AsyncRunner.Run(populateShift());
                     AsyncRunner.Run(populateShiftExpand());
                     AsyncRunner.Run(populateExpenseType());
                     AsyncRunner.Run(PopulatePayTypes());
                     return Task.CompletedTask;
                 });
                
                //await populateStatus();
            }
                
        }

        private NavigationMenuItem GetFirstAuthorizedMenuItemOrNull(out ObservableRangeCollection<NavigationMenuItem> authorizedMenuItems)
        {
            authorizedMenuItems = _menuProvider.GetAuthorizedMenuItems(_appContext.Configuration.Auth.GrantedPermissions);
            var firstMenuItem = authorizedMenuItems.FirstOrDefault();
            if (firstMenuItem?.ViewType == null)
            {
                return null;
            }

            return firstMenuItem;
        }

        private static void SetSelectedMenuItem(IEnumerable<NavigationMenuItem> menuItems, NavigationMenuItem selectedMenuItem)
        {
            menuItems.ForEach(m => m.IsSelected = false);
            selectedMenuItem.IsSelected = true;
        }
    }
}

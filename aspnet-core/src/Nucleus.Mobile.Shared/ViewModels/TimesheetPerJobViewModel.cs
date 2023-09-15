using Abp;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Castle.Core.Internal;
using MonkeyCache.SQLite;
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Commands;
using Nucleus.Core.Threading;
using Nucleus.ExpenseType;
using Nucleus.ExpenseType.Dtos;
using Nucleus.Job.Dtos;
using Nucleus.JobCategory;
using Nucleus.JobCategory.Dtos;
using Nucleus.JobPhaseCode;
using Nucleus.JobPhaseCode.Dtos;
using Nucleus.Localization;
using Nucleus.Models.Employees;
using Nucleus.Models.Equipment;
using Nucleus.Models.TimesheetPerJob;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.WorkerClasee.Dtos;
using Nucleus.ResourceWorkerInfo;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.Shift;
using Nucleus.Shift.Dtos;
using Nucleus.ShiftExpense;
using Nucleus.ShiftExpense.Dtos;
using Nucleus.ShiftResource;
using Nucleus.ShiftResource.Dtos;
using Nucleus.Status.Dtos;
using Nucleus.Timesheet;
using Nucleus.Timesheet.Dtos;
using Nucleus.ViewModels.Base;
using NUglify.Helpers;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Nucleus.ViewModels.Helpers;
using Nucleus.WorkerClasee;
using Abp.Application.Services.Dto;

namespace Nucleus.ViewModels
{
    public class TimesheetPerJobViewModel : XamarinViewModel
    {
        // Page Properties
        public string Title { get; set; }
        private string toolbar_name { get; set; }
        public string Toolbar_name { get => toolbar_name; set { toolbar_name = value; OnPropertyChanged(); } }
        private string _selected_time { get; set; }
        public string selected_time
        {
            get => _selected_time;
            set
            {
                _selected_time = value;
                OnPropertyChanged();
            }
        }
        public int _currentPage;
        public string temp;
        public int runThisOnetimeOnly { get; set; }
        //private bool _selectenabled { get; set; }
        //public bool selectenabled { get => _selectenabled; set { _selectenabled = value; OnPropertyChanged(); } }
        public ObservableRangeCollection<EmployeesresevedResorses> reservedEmpResourceId { get; set; }
        public ObservableRangeCollection<TimesheetPerJobListModel> TimesheetLineItems { get; set; }
        public ObservableRangeCollection<TimesheetPerJobForEquipmentListModel> TimesheetPerJobForEquipmentListItems { get; set; }
        public ObservableRangeCollection<PayTypesDto> PayTypes1 { get; set; }
        public ObservableRangeCollection<ShiftResourcesDto> ListofShiftResource { get; set; }
        public ObservableRangeCollection<ShiftExpensesDto> ListofShiftExpenses { get; set; }
        public ObservableRangeCollection<PayTypesDto> PayTypes2 { get; set; }
        public ObservableRangeCollection<PayTypesDto> PayTypes3 { get; set; }
        public ObservableRangeCollection<JobPhaseCodesDto> PhaseCode { get; set; }
        public ObservableRangeCollection<JobCategoriesDto> PhaseCategoryCode { get; set; }
        public ObservableRangeCollection<EmployeesListModel> Employees { get; set; }
        public ObservableRangeCollection<EquipmentListModel> Equipment { get; set; }
        public ObservableRangeCollection<ExpenseTypesDto> ExpenseTypes { get; set; }
        public ObservableRangeCollection<WorkerClaseesDto> workerClaseesDto { get; set; }
        public GetAllResourceReservationsesInput getAllResourceReservationsesInput { get; set; }
        public GetAllResourceWorkerInfosesInput getAllResourceWorkerInfosesInput { get; set; }
        public GetAllJobPhaseCodesInput getAllJobPhaseCodesInput { get; set; }
        public GetAllTimesheetsInput getAllTimesheetsInput { get; set; }
        public GetAllShiftsInput getAllShiftsInput { get; set; }
        public GetAllShiftResourcesInput getAllShiftResourcesInput { get; set; }
        public ShiftsDto selectedJobandDate { get; set; }
        public string selectedDescription { get; set; }

        public CreateOrEditTimesheetsDto createOrEditTimesheetsDto { get; set; }
        public CreateOrEditShiftsDto createOrEditShiftsDto { get; set; }
        public CreateOrEditShiftResourcesDto createOrEditShiftResourcesDto { get; set; }
        public GetAllWorkerClaseesesInput getAllWorkerClaseesesInput { get; set; }


        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);
        /*public ICommand selected_UsageCommand => new Command<int>(selected_UsageCommandAsync);
        public ICommand selected_paytype1Command => new Command<int>(selected_paytype1CommandAsync);
        public ICommand selected_paytype2Command => new Command<int>(selected_paytype2CommandAsync);
        public ICommand selected_paytype3Command => new Command<int>(selected_paytype3CommandAsync);
        public ICommand selected_perDimCommand => new Command<int>(selected_perDimCommandAsync);
        public ICommand selected_miscCommand => new Command<int>(selected_miscCommandAsync);*/
        // Services
        private readonly IPayTypesesAppService _payTypesAppService;
        private readonly IResourceWorkerInfosesAppService _employeesService;
        private readonly IResourceReservationsesAppService _reservedResourceService;
        private readonly IExpenseTypesesAppService _expenseTypesesAppService;
        private readonly IJobPhaseCodesAppService _jobPhaseCodesAppService;
        private readonly IJobCategoriesAppService _jobCategoriesAppService;
        private readonly IShiftResourcesAppService _shiftResourcesAppService;
        private readonly IShiftsAppService _shiftsAppService;
        private readonly ITimesheetsAppService _timesheetsAppService;
        private readonly IShiftExpensesAppService _shiftExpensesAppService;
        private readonly IApplicationContext _applicationContext;
        private readonly IWorkerClaseesesAppService _workerClaseesesAppService;

        //Constructor
        public TimesheetPerJobViewModel(IResourceWorkerInfosesAppService employeesService, IPayTypesesAppService paytypes, IResourceReservationsesAppService reservedResourceService, IExpenseTypesesAppService expenseTypesesAppService, IJobPhaseCodesAppService jobPhaseCodesAppService, IJobCategoriesAppService jobCategoriesAppService, IShiftResourcesAppService shiftResourcesAppService, IShiftsAppService shiftsAppService, ITimesheetsAppService timesheetsAppService, IShiftExpensesAppService shiftExpensesAppService, IApplicationContext applicationContext, IWorkerClaseesesAppService workerClaseesesAppService)
        {
            _payTypesAppService = paytypes;
            _reservedResourceService = reservedResourceService;
            _expenseTypesesAppService = expenseTypesesAppService;
            _employeesService = employeesService;
            _jobPhaseCodesAppService = jobPhaseCodesAppService;
            _jobCategoriesAppService = jobCategoriesAppService;
            _shiftResourcesAppService = shiftResourcesAppService;
            _shiftsAppService = shiftsAppService;
            _timesheetsAppService = timesheetsAppService;
            _shiftExpensesAppService = shiftExpensesAppService;
            _applicationContext = applicationContext;
            _workerClaseesesAppService = workerClaseesesAppService;
            Title = L.Localize("Timesheet");
            selectedJobandDate = new ShiftsDto();
            reservedEmpResourceId = new ObservableRangeCollection<EmployeesresevedResorses>();
            PayTypes1 = new ObservableRangeCollection<PayTypesDto>();
            PayTypes2 = new ObservableRangeCollection<PayTypesDto>();
            PayTypes3 = new ObservableRangeCollection<PayTypesDto>();
            Employees = new ObservableRangeCollection<EmployeesListModel>();
            Equipment = new ObservableRangeCollection<EquipmentListModel>();
            workerClaseesDto = new ObservableRangeCollection<WorkerClaseesDto>();
            TimesheetLineItems = new ObservableRangeCollection<TimesheetPerJobListModel>();
            ListofShiftResource = new ObservableRangeCollection<ShiftResourcesDto>();
            ListofShiftExpenses = new ObservableRangeCollection<ShiftExpensesDto>();
            TimesheetPerJobForEquipmentListItems = new ObservableRangeCollection<TimesheetPerJobForEquipmentListModel>();
            ExpenseTypes = new ObservableRangeCollection<ExpenseTypesDto>();
            getAllResourceReservationsesInput = new GetAllResourceReservationsesInput();
            getAllJobPhaseCodesInput = new GetAllJobPhaseCodesInput();
            getAllResourceWorkerInfosesInput = new GetAllResourceWorkerInfosesInput();
            getAllTimesheetsInput = new GetAllTimesheetsInput();
            getAllShiftResourcesInput = new GetAllShiftResourcesInput();
            getAllShiftsInput = new GetAllShiftsInput();
            PhaseCode = new ObservableRangeCollection<JobPhaseCodesDto>();
            PhaseCategoryCode = new ObservableRangeCollection<JobCategoriesDto>();
            createOrEditTimesheetsDto = new CreateOrEditTimesheetsDto();
            createOrEditShiftsDto = new CreateOrEditShiftsDto();
            selectedDescription = "N/A";
            runThisOnetimeOnly = 1;
            getAllWorkerClaseesesInput = new GetAllWorkerClaseesesInput
            {
                MaxResultCount = 20
            };
            // Daisy chain the load so pay type LOV loads before epmloyees, which depend on that list already being loaded.
            PopulateExpenseTypeListofValue();
            PopulateListofWorkerClass();
            PopulateReservedResource(
            async () =>
            {
                PopulateReservedEmployees();
                AsyncRunner.Run(PopulateJobCategoryCodeListOfValue());

            });

        }

        //Populate Job Categories, Paytype and Reserved Resources
        /* populate Resources/ Paytypes/ Job code/ Phase Code Extra Cost value */

        public async void PopulateReservedResource(Action callback)
        {
            TimesheetLineItems.Clear();
            await FetchEquipmentAsync();
            callback();
        }

        public async void PopulateListofWorkerClass()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _workerClaseesesAppService.GetAll(getAllWorkerClaseesesInput), result =>
                {
                    foreach (var item in result.Items)
                    {
                        workerClaseesDto.Add(new WorkerClaseesDto
                        {
                            Code = item.WorkerClasees.Code,
                            Name = item.WorkerClasees.Name,
                            Id = item.WorkerClasees.Id

                        });
                    }
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetWorkerClaseesForViewDto>>("WorkerClasees");
                foreach (var item in response)
                {
                    workerClaseesDto.Add(new WorkerClaseesDto
                    {
                        Code = item.WorkerClasees.Code,
                        Name = item.WorkerClasees.Name,
                        Id = item.WorkerClasees.Id

                    });
                }

            }
        }

        private async Task FetchEquipmentAsync()
        {
            getAllResourceReservationsesInput.MaxResultCount = 1000;
            getAllResourceReservationsesInput.UserNameFilter = _applicationContext.LoginInfo.User.Name;
            //Get reserved Employees/Equipment data 
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _reservedResourceService.GetMyReservedResource(getAllResourceReservationsesInput), result =>
                {
                    foreach (var item in result.Items)
                    {
                        if (item.Resources.Type.Equals("Employees"))
                        {
                            reservedEmpResourceId.Add(new EmployeesresevedResorses
                            {
                                resourcesDto = item.Resources,
                                ResourcesId = item.ResourcesId,
                                ReservedFrom = item.ReservedFrom,
                                ReservedUntil = item.ReservedUntil,
                                UserId = item.UserId,
                                ReservedBy = item.UserName,
                                Id = item.Id
                            });
                            getAllResourceWorkerInfosesInput.Filter = item.ResourcesName.Split(" ")[0];
                            AsyncRunner.Run(FetchEmployeesAsync());
                        }
                        else if (item.Resources.Type.Equals("Equipment"))
                        {
                            Equipment.Add(new EquipmentListModel
                            {
                                Name = item.Resources.Name,
                                ResourceNumber = item.Resources.ResourceNumber,
                                CostPerHour = item.Resources.CostPerHour,
                                Id = item.Resources.Id,
                                Type = item.Resources.Type,
                                CostPerDay = item.Resources.CostPerDay,
                                CostPerUser = item.Resources.CostPerUser
                            });
                        }

                    }
                    return Task.CompletedTask;

                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetMyResourceReservationsDto>>("ResourceReservationses");
                foreach (var item in response)
                {
                    if (item.UserName != getAllResourceReservationsesInput.UserNameFilter)
                        continue;
                    if (item.Resources.Type.Equals("Employees"))
                    {
                        reservedEmpResourceId.Add(new EmployeesresevedResorses
                        {
                            resourcesDto = item.Resources,
                            ResourcesId = item.ResourcesId,
                            ReservedFrom = item.ReservedFrom,
                            ReservedUntil = item.ReservedUntil,
                            UserId = item.UserId,
                            ReservedBy = item.UserName,
                            Id = item.Id
                        });
                        getAllResourceWorkerInfosesInput.Filter = item.ResourcesName.Split(" ")[0];
                        AsyncRunner.Run(FetchEmployeesAsync());
                    }
                    else if (item.Resources.Type.Equals("Equipment"))
                    {
                        Equipment.Add(new EquipmentListModel
                        {
                            Name = item.Resources.Name,
                            ResourceNumber = item.Resources.ResourceNumber,
                            CostPerHour = item.Resources.CostPerHour,
                            Id = item.Resources.Id,
                            Type = item.Resources.Type,
                            CostPerDay = item.Resources.CostPerDay,
                            CostPerUser = item.Resources.CostPerUser
                        });
                    }

                }
            }


        }


        public async void PopulateReservedEmployees()
        {
            await FetchEmployeesAsync();
        }
        private async Task FetchEmployeesAsync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _employeesService.GetAll(getAllResourceWorkerInfosesInput), result =>
                {
                    foreach (var emp in result.Items.OrderBy(a => a.ResourcesName))
                    {
                        bool ischecked = false;
                        bool reservedByOther = false;
                        var reservedBy = "N/A";
                        if (reservedEmpResourceId.Where(a => a.resourcesDto.Name.Equals(emp.ResourcesName)).Count() > 0 && Employees.Where(b => b.FullName.Equals(emp.ResourcesName)).Count() <= 0)
                        {
                            Employees.Add(new EmployeesListModel
                            {
                                FirstName = emp.ResourceWorkerInfos.FirstName,
                                LastName = emp.ResourceWorkerInfos.LastName,
                                RefNumber = emp.ResourceWorkerInfos.RefNumber,
                                WorkerClasees = emp.WorkerClaseesName,
                                ResourcesId = emp.ResourceWorkerInfos.ResourcesId,
                                IsChecked = ischecked,
                                ReservedByName = reservedBy,
                                IsReservedByOther = reservedByOther

                            });
                        }
                    }
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetResourceWorkerInfosForViewDto>>("ResourceWorkerInfoses");
                foreach (var emp in response.OrderBy(a => a.ResourcesName))
                {
                    bool ischecked = false;
                    bool reservedByOther = false;
                    var reservedBy = "N/A";
                    if (reservedEmpResourceId.Where(a => a.resourcesDto.Name.Equals(emp.ResourcesName)).Count() > 0 && Employees.Where(b => b.FullName.Equals(emp.ResourcesName)).Count() <= 0)
                    {
                        Employees.Add(new EmployeesListModel
                        {
                            FirstName = emp.ResourceWorkerInfos.FirstName,
                            LastName = emp.ResourceWorkerInfos.LastName,
                            RefNumber = emp.ResourceWorkerInfos.RefNumber,
                            WorkerClasees = emp.WorkerClaseesName,
                            ResourcesId = emp.ResourceWorkerInfos.ResourcesId,
                            IsChecked = ischecked,
                            ReservedByName = reservedBy,
                            IsReservedByOther = reservedByOther

                        });
                    }
                }
            }

        }
        private async Task PopulateJobCategoryCodeListOfValue()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _jobCategoriesAppService.GetAll(null), result =>
                {
                    foreach (var item in result.Items)
                        PhaseCategoryCode.Add(item.JobCategories);
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetJobCategoriesForViewDto>>("JobCategories");
                foreach (var item in response)
                    PhaseCategoryCode.Add(item.JobCategories);

            }

        }
        private async void PopulatePhaseCodeListOfValue(string jobName)
        {
            //getAllJobPhaseCodesInput.JobsNameFilter = jobName;
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _jobPhaseCodesAppService.GetAll(getAllJobPhaseCodesInput), result =>
                {
                    foreach (var item in result.Items)
                        PhaseCode.Add(item.JobPhaseCodes);
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetJobPhaseCodesForViewDto>>("PhaseCode");
                foreach (var item in response)
                {
                    if (item.JobsName != getAllJobPhaseCodesInput.JobsNameFilter)
                        continue;
                    PhaseCode.Add(item.JobPhaseCodes);
                }
            }

        }
        private async void PopulateExpenseTypeListofValue()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _expenseTypesesAppService.GetAll(null), result =>
                {
                    foreach (var item in result.Items)
                    {
                        ExpenseTypes.Add(new ExpenseTypesDto
                        {
                            Name = item.ExpenseTypes.Name,
                            Code = item.ExpenseTypes.Code,
                            Description = item.ExpenseTypes.Description,
                            Id = item.ExpenseTypes.Id,

                        });
                    }
                    return PopulatePayTypesListOfValues();
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetExpenseTypesForViewDto>>("ExpenseType");
                foreach (var item in response)
                {
                    ExpenseTypes.Add(new ExpenseTypesDto
                    {
                        Name = item.ExpenseTypes.Name,
                        Code = item.ExpenseTypes.Code,
                        Description = item.ExpenseTypes.Description,
                        Id = item.ExpenseTypes.Id,

                    });
                }
                await PopulatePayTypesListOfValues();
            }

        }
        private async Task PopulatePayTypesListOfValues()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _payTypesAppService.GetAll(null), result =>
                {
                    foreach (var item in result.Items)
                    {
                        if (item.PayTypes.Section1.Equals(true))
                            PayTypes1.Add(item.PayTypes);
                        if (item.PayTypes.Section2.Equals(true))
                            PayTypes2.Add(item.PayTypes);
                        if (item.PayTypes.Section3.Equals(true))
                            PayTypes3.Add(item.PayTypes);
                    }
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetPayTypesForViewDto>>("PayTypes");
                foreach (var item in response)
                {
                    if (item.PayTypes.Section1.Equals(true))
                        PayTypes1.Add(item.PayTypes);
                    if (item.PayTypes.Section2.Equals(true))
                        PayTypes2.Add(item.PayTypes);
                    if (item.PayTypes.Section3.Equals(true))
                        PayTypes3.Add(item.PayTypes);
                }
            }

        }
        private async Task PopulateExistedTimeSheet()
        {

            if (selectedJobandDate.Name == null)
            {
                return;
            }
            if (CrossConnectivity.Current.IsConnected)
            {
                getAllTimesheetsInput.NameFilter = selectedJobandDate.Name.ToString();
                getAllTimesheetsInput.MaxCreatedDateFilter = selectedJobandDate.ScheduledStart;
                getAllTimesheetsInput.MinCreatedDateFilter = selectedJobandDate.ScheduledStart;
                await WebRequestExecuter.Execute(async () => await _timesheetsAppService.GetAll(getAllTimesheetsInput), result =>
                {
                    var items = result;
                    if (items.Items.Count() > 0)
                    {
                        return populateTimesheetPerJobGrid(items.Items.FirstOrDefault().Timesheets);
                    }
                    else
                    {
                        return Task.CompletedTask;
                    }

                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");
                foreach (var item in response)
                {
                    if (item.Timesheets.Name == selectedJobandDate.Name.ToString() && item.Timesheets.CreatedDate == selectedJobandDate.ScheduledStart)
                    {
                        await populateTimesheetPerJobGrid(item.Timesheets);
                    }
                }
                return;
            }


        }
        private async Task populateTimesheetPerJobGrid(TimesheetsDto items)
        {
            var timesheetVal = items;
            getAllShiftResourcesInput.TimesheetId = timesheetVal.Id;
            getAllShiftResourcesInput.TimesheetsNameFilter = timesheetVal.Name;
            getAllShiftResourcesInput.NameFilter = _applicationContext.LoginInfo.User.Name;
            getAllShiftResourcesInput.MaxCreatedDateFilter = selectedJobandDate.ScheduledStart;
            getAllShiftResourcesInput.MinCreatedDateFilter = selectedJobandDate.ScheduledStart;
            temp = "";
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
                {
                    var item = result;
                    var resourceType = new ResourcesDto();

                    if (item.Items.Count() > 0)
                    {
                        TimesheetLineItems.Clear();
                        var sortedItems = item.Items.OrderByDescending(a => a.ResourcesName);

                        foreach (GetShiftResourcesForViewDto timesheetPerJobListModel in sortedItems)
                        {
                            ListofShiftResource.Add(new ShiftResourcesDto() { Id = timesheetPerJobListModel.ShiftResources.Id, ResourcesId = timesheetPerJobListModel.ShiftResources.ResourcesId });
                            if (Equipment.Where(a => a.Id.Equals(timesheetPerJobListModel.ShiftResources.ResourcesId)).Count() > 0)
                            {
                                resourceType.Id = Equipment.Where(a => a.Id.Equals(timesheetPerJobListModel.ShiftResources.ResourcesId)).FirstOrDefault().Id;
                                resourceType.Type = "Equipment";
                                resourceType.CostPerHour = timesheetPerJobListModel.ShiftResources.HoursWorked;
                                TimesheetLineItems.Add(new TimesheetPerJobListModel()
                                {
                                    shiftResourceId = timesheetPerJobListModel.ShiftResources.Id,
                                    ResourceName = timesheetPerJobListModel.ResourcesName,
                                    ResourceId = resourceType.Id,
                                    JobName = selectedJobandDate.Name,
                                    SelectedPhaseCode = PhaseCode.Count() > 0 ? PhaseCode.Where(a => a.Name.Equals(timesheetPerJobListModel.JobPhaseCodesName)).First() : null,
                                    SelectedJobCategoryCode = PhaseCategoryCode.Count() > 0 ? PhaseCategoryCode.Where(a => a.Name.Equals(timesheetPerJobListModel.JobCategoriesName)).First() : null,
                                    Type = resourceType.Type,
                                    SelectedDate = selectedJobandDate.ScheduledStart.Value.ToString("MMM") + "-" + selectedJobandDate.ScheduledStart.Value.Date.Day.ToString(),
                                    EquipmentUsage = resourceType.CostPerHour.ToString(),
                                    trdColWidth = (int)(Application.Current.MainPage.Width - 200),
                                    UsageColor = Color.White,
                                    UsageEnabled = true
                                });

                                var emp = Equipment.Where(a => a.Id.Equals(timesheetPerJobListModel.ShiftResources.ResourcesId)).First();
                                emp.IsChecked = true;
                            }
                            else if (Employees.Where(a => a.ResourcesId.Equals(timesheetPerJobListModel.ShiftResources.ResourcesId)).Count() > 0)
                            {
                                if (!temp.Equals(timesheetPerJobListModel.ResourcesName))
                                {
                                    temp = timesheetPerJobListModel.ResourcesName;
                                    if (!Convert.ToString(timesheetPerJobListModel.ShiftResources.ResourcesId).IsNullOrEmpty())
                                        resourceType.Id = timesheetPerJobListModel.ShiftResources.ResourcesId;
                                    else if (!Convert.ToString(Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First().ResourcesId).IsNullOrEmpty())
                                        resourceType.Id = (int)Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First().ResourcesId;
                                    resourceType.Type = "Employees";
                                    //Here rrsource numb is Worker's class name


                                    resourceType.ResourceNumber = Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First().WorkerClasees.ToString().IsNullOrEmpty() ? "" : Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First().WorkerClasees.ToString();
                                    TimesheetLineItems.Add(new TimesheetPerJobListModel()
                                    {
                                        shiftResourceId = timesheetPerJobListModel.ShiftResources.Id,
                                        ResourceName = timesheetPerJobListModel.ResourcesName,
                                        ResourceId = resourceType.Id,
                                        JobName = selectedJobandDate.Name,
                                        Type = resourceType.Type,
                                        WorkerClass = timesheetPerJobListModel.WorkerClaseesName.ToString() != null ? workerClaseesDto.Where(a => a.Name != null && a.Name.Equals(timesheetPerJobListModel.WorkerClaseesName)).FirstOrDefault() : workerClaseesDto.Where(a => a.Name.Equals(resourceType.ResourceNumber)).FirstOrDefault(),
                                        SelectedPhaseCode = PhaseCode.Count() > 0 ? PhaseCode.Where(a => a.Name.Equals(timesheetPerJobListModel.JobPhaseCodesName)).First() : null,
                                        SelectedJobCategoryCode = PhaseCategoryCode.Count() > 0 ? PhaseCategoryCode.Where(a => a.Name.Equals(timesheetPerJobListModel.JobCategoriesName)).First() : null,
                                        SelectedDate = selectedJobandDate.ScheduledStart.Value.ToString("MMM") + "-" + selectedJobandDate.ScheduledStart.Value.Date.Day.ToString(),
                                        paytype1Color = Color.White,
                                        paytype2Color = Color.White,
                                        paytype3Color = Color.White,
                                        perDimColor = Color.White,
                                        miscColor = Color.White,
                                        paytype1Enabled = true,
                                        paytype2Enabled = true,
                                        paytype3Enabled = true,
                                        perdimEnabled = true,
                                        miscEnabled = true,
                                    });

                                    var emp = Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First();
                                    emp.IsChecked = true;
                                    //Populate Per dim and Misc value with Expense Type
                                    checkIsExpenseExist(timesheetPerJobListModel.ShiftResources.Id, timesheetPerJobListModel.ResourcesName);

                                }
                                if (temp.Equals(timesheetPerJobListModel.ResourcesName) && TimesheetLineItems.Where(a => a.ResourceName.Equals(temp)).Count() > 0)
                                {
                                    //Add Only Paytyoe1,2,3 as per resource in grid 
                                    var TimehseetItme = TimesheetLineItems.Where(a => a.ResourceName.Equals(temp)).FirstOrDefault();
                                    if (!timesheetPerJobListModel.PayTypesCode.ToString().IsNullOrEmpty() && PayTypes1.Where(j => j.Code.Equals(timesheetPerJobListModel.PayTypesCode)).Count() > 0 && TimehseetItme.Paytype1Text.IsNullOrEmpty())
                                    {

                                        TimehseetItme.Section1 = PayTypes1.Where(a => a.Code.Equals(timesheetPerJobListModel.PayTypesCode)).First();
                                        TimehseetItme.Paytype1Text = (timesheetPerJobListModel.ShiftResources.HoursWorked).ToString();

                                    }
                                    else
                                    if (!timesheetPerJobListModel.PayTypesCode.ToString().IsNullOrEmpty() && PayTypes2.Where(j => j.Code.Equals(timesheetPerJobListModel.PayTypesCode)).Count() > 0 && TimehseetItme.Paytype2Text.IsNullOrEmpty())
                                    {

                                        TimehseetItme.Section2 = PayTypes2.Where(a => a.Code.Equals(timesheetPerJobListModel.PayTypesCode)).First();
                                        TimehseetItme.Paytype2Text = timesheetPerJobListModel.ShiftResources.HoursWorked.ToString();
                                    }
                                    else
                                    if (!timesheetPerJobListModel.PayTypesCode.ToString().IsNullOrEmpty() && PayTypes3.Where(j => j.Code.Equals(timesheetPerJobListModel.PayTypesCode)).Count() > 0 && TimehseetItme.Paytype3Text.IsNullOrEmpty())
                                    {

                                        TimehseetItme.Section3 = PayTypes3.Where(a => a.Code.Equals(timesheetPerJobListModel.PayTypesCode)).First();
                                        TimehseetItme.Paytype3Text = timesheetPerJobListModel.ShiftResources.HoursWorked.ToString().IsNullOrEmpty() ? "0.00" : timesheetPerJobListModel.ShiftResources.HoursWorked.ToString();
                                    }
                                }
                            }
                        }

                    }
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetShiftResourcesForViewDto>>("ShiftResources");
                var isExist = response.Where(a => a.ShiftResources.TimesheetsId.Equals(getAllShiftResourcesInput.TimesheetId) && a.TimesheetsName.Equals(getAllShiftResourcesInput.TimesheetsNameFilter) && a.ShiftResources.Name.Equals(getAllShiftResourcesInput.NameFilter));
                var resourceType = new ResourcesDto();

                if (isExist.Count() > 0)
                {
                    TimesheetLineItems.Clear();
                    var sortedItems = isExist.OrderByDescending(a => a.ResourcesName);

                    foreach (GetShiftResourcesForViewDto timesheetPerJobListModel in sortedItems)
                    {

                        if (Equipment.Where(a => a.Id.Equals(timesheetPerJobListModel.ShiftResources.ResourcesId)).Count() > 0)
                        {
                            resourceType.Id = Equipment.Where(a => a.Id.Equals(timesheetPerJobListModel.ShiftResources.ResourcesId)).FirstOrDefault().Id;
                            resourceType.Type = "Equipment";
                            resourceType.CostPerHour = timesheetPerJobListModel.ShiftResources.HoursWorked;
                            TimesheetLineItems.Add(new TimesheetPerJobListModel()
                            {
                                shiftResourceId = timesheetPerJobListModel.ShiftResources.Id,
                                ResourceName = timesheetPerJobListModel.ResourcesName,
                                ResourceId = resourceType.Id,
                                JobName = selectedJobandDate.Name,
                                Type = resourceType.Type,
                                SelectedDate = selectedJobandDate.ScheduledStart.Value.ToString("MMM") + "-" + selectedJobandDate.ScheduledStart.Value.Date.Day.ToString(),
                                EquipmentUsage = resourceType.CostPerHour.ToString(),
                                trdColWidth = (int)(Application.Current.MainPage.Width - 200),
                                UsageColor = Color.White,
                                UsageEnabled = true
                            });
                            var emp = Equipment.Where(a => a.Id.Equals(timesheetPerJobListModel.ShiftResources.ResourcesId)).First();
                            emp.IsChecked = true;
                        }
                        else if (Employees.Where(a => a.ResourcesId.Equals(timesheetPerJobListModel.ShiftResources.ResourcesId)).Count() > 0)
                        {
                            if (!temp.Equals(timesheetPerJobListModel.ResourcesName))
                            {
                                temp = timesheetPerJobListModel.ResourcesName;
                                if (!Convert.ToString(timesheetPerJobListModel.ShiftResources.ResourcesId).IsNullOrEmpty())
                                    resourceType.Id = timesheetPerJobListModel.ShiftResources.ResourcesId;
                                else if (!Convert.ToString(Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First().ResourcesId).IsNullOrEmpty())
                                    resourceType.Id = (int)Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First().ResourcesId;
                                resourceType.Type = "Employees";
                                //Here rrsource numb is Worker's class name
                                resourceType.ResourceNumber = Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First().WorkerClasees.ToString().IsNullOrEmpty() ? "" : Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First().WorkerClasees.ToString();
                                TimesheetLineItems.Add(new TimesheetPerJobListModel()
                                {
                                    shiftResourceId = timesheetPerJobListModel.ShiftResources.Id,
                                    ResourceName = timesheetPerJobListModel.ResourcesName,
                                    ResourceId = resourceType.Id,
                                    JobName = selectedJobandDate.Name,
                                    Type = resourceType.Type,
                                    WorkerClass = workerClaseesDto.Where(a => a.Name != null && a.Name.Equals(timesheetPerJobListModel.WorkerClaseesName)).FirstOrDefault(),
                                    SelectedPhaseCode = PhaseCode.Count() > 0 ? PhaseCode.Where(a => a.Name.Equals(timesheetPerJobListModel.JobPhaseCodesName)).First() : null,
                                    SelectedJobCategoryCode = PhaseCategoryCode.Count() > 0 ? PhaseCategoryCode.Where(a => a.Name.Equals(timesheetPerJobListModel.JobCategoriesName)).First() : null,
                                    SelectedDate = selectedJobandDate.ScheduledStart.Value.ToString("MMM") + "-" + selectedJobandDate.ScheduledStart.Value.Date.Day.ToString(),
                                    paytype1Color = Color.White,
                                    paytype2Color = Color.White,
                                    paytype3Color = Color.White,
                                    perDimColor = Color.White,
                                    miscColor = Color.White,
                                    paytype1Enabled = true,
                                    paytype2Enabled = true,
                                    paytype3Enabled = true,
                                    perdimEnabled = true,
                                    miscEnabled = true,
                                });
                                var emp = Employees.Where(a => a.FullName.Equals(timesheetPerJobListModel.ResourcesName)).First();
                                emp.IsChecked = true;
                                //Populate Per dim and Misc value with Expense Type
                                checkIsExpenseExist(timesheetPerJobListModel.ShiftResources.Id, timesheetPerJobListModel.ResourcesName);

                            }
                            if (temp.Equals(timesheetPerJobListModel.ResourcesName) && TimesheetLineItems.Where(a => a.ResourceName.Equals(temp)).Count() > 0)
                            {
                                //Add Only Paytyoe1,2,3 as per resource in grid 
                                var TimehseetItme = TimesheetLineItems.Where(a => a.ResourceName.Equals(temp)).FirstOrDefault();
                                if (!timesheetPerJobListModel.PayTypesCode.ToString().IsNullOrEmpty() && PayTypes1.Where(j => j.Code.Equals(timesheetPerJobListModel.PayTypesCode)).Count() > 0 && TimehseetItme.Paytype1Text.IsNullOrEmpty())
                                {

                                    TimehseetItme.Section1 = PayTypes1.Where(a => a.Code.Equals(timesheetPerJobListModel.PayTypesCode)).First();
                                    TimehseetItme.Paytype1Text = timesheetPerJobListModel.ShiftResources.HoursWorked.ToString();
                                }
                                else
                                if (!timesheetPerJobListModel.PayTypesCode.ToString().IsNullOrEmpty() && PayTypes2.Where(j => j.Code.Equals(timesheetPerJobListModel.PayTypesCode)).Count() > 0 && TimehseetItme.Paytype2Text.IsNullOrEmpty())
                                {

                                    TimehseetItme.Section2 = PayTypes2.Where(a => a.Code.Equals(timesheetPerJobListModel.PayTypesCode)).First();
                                    TimehseetItme.Paytype2Text = timesheetPerJobListModel.ShiftResources.HoursWorked.ToString();
                                }
                                else
                                if (!timesheetPerJobListModel.PayTypesCode.ToString().IsNullOrEmpty() && PayTypes3.Where(j => j.Code.Equals(timesheetPerJobListModel.PayTypesCode)).Count() > 0 && TimehseetItme.Paytype3Text.IsNullOrEmpty())
                                {

                                    TimehseetItme.Section3 = PayTypes3.Where(a => a.Code.Equals(timesheetPerJobListModel.PayTypesCode)).First();
                                    TimehseetItme.Paytype3Text = timesheetPerJobListModel.ShiftResources.HoursWorked.ToString().IsNullOrEmpty() ? "0.00" : timesheetPerJobListModel.ShiftResources.HoursWorked.ToString();
                                }
                            }
                        }
                    }

                }
            }

        }
        //Commands 
        public Command<JobsDto> SelectedJob
        {
            get
            {
                return new Command<JobsDto>((selectedJob) =>
                {

                    PopulatePhaseCodeListOfValue(selectedJob.Name);
                });
            }
        }
        internal void SelectedDescription(TimesheetPerJobListModel sd, string desc)
        {

            var lineitem = TimesheetLineItems.Where(a => a.JobName.Equals(sd.JobName) && a.ResourceName.Equals(sd.ResourceName)).FirstOrDefault();
            lineitem.SelectedDesctiption = desc;

        }
        public Command<EmployeesListModel> PopulateEmployeeGrid
        {
            get
            {
                return new Command<EmployeesListModel>((reservedReosurces) =>
                {
                    try
                    {
                        if (TimesheetLineItems.Where(a => a.ResourceName.Equals(reservedReosurces.FullName)).Count() == 0)
                        {
                            TimesheetLineItems.Add(new TimesheetPerJobListModel()
                            {
                                Section1 = PayTypes1.First(),
                                Section2 = PayTypes2.First(),
                                Section3 = PayTypes3.First(),
                                ResourceName = reservedReosurces.FullName,
                                ResourceId = reservedReosurces.ResourcesId,
                                JobName = selectedJobandDate.Name,
                                Type = "Employees",
                                WorkerClass = workerClaseesDto.Where(a => a.Name != null && a.Name.Equals(reservedReosurces.WorkerClasees, StringComparison.OrdinalIgnoreCase)).FirstOrDefault(),
                                SelectedExpenseItem = ExpenseTypes.First(),
                                SelectedPhaseCode = PhaseCode.Count() > 0 ? PhaseCode.First() : null,
                                SelectedJobCategoryCode = PhaseCategoryCode.Count() > 0 ? PhaseCategoryCode.First() : null,
                                SelectedDate = selectedJobandDate.ScheduledStart.Value.ToString("MMM") + "-" + selectedJobandDate.ScheduledStart.Value.Date.Day.ToString(),
                                paytype1Color = Color.White,
                                paytype2Color = Color.White,
                                paytype3Color = Color.White,
                                perDimColor = Color.White,
                                miscColor = Color.White,
                                paytype1Enabled = true,
                                paytype2Enabled = true,
                                paytype3Enabled = true,
                                perdimEnabled = true,
                                miscEnabled = true,
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        e.Message.ToString();
                    }


                });
            }
        }
        public Command<EquipmentListModel> PopulateEquipmentGrid
        {
            get
            {
                return new Command<EquipmentListModel>((reservedReosurces) =>
                {
                    if (TimesheetLineItems.Where(a => a.ResourceName.Equals(reservedReosurces.Name)).Count() == 0)
                    {
                        TimesheetLineItems.Add(new TimesheetPerJobListModel()
                        {

                            ResourceName = reservedReosurces.Name,
                            ResourceId = reservedReosurces.Id,
                            JobName = selectedJobandDate.Name,
                            Type = reservedReosurces.Type,
                            SelectedExpenseItem = ExpenseTypes.First(),
                            SelectedDate = selectedJobandDate.ScheduledStart.Value.ToString("MMM") + "-" + selectedJobandDate.ScheduledStart.Value.Date.Day.ToString(),
                            trdColWidth = (int)(Application.Current.MainPage.Width - 200),
                            UsageColor = Color.White,
                            UsageEnabled = true
                        });
                    }
                });
            }
        }
        public Command<EmployeesListModel> RemoveItemfromEmployeeGrid
        {
            get
            {
                return new Command<EmployeesListModel>((reservedReosurces) =>
                {
                    var item = TimesheetLineItems.Where(a => a.ResourceName.Equals(reservedReosurces.FullName)).FirstOrDefault();
                    TimesheetLineItems.Remove(item);

                    //Remove All entry for the timesheet
                    foreach (ShiftResourcesDto itm in ListofShiftResource)
                    {
                        if (itm.ResourcesId.Equals(item.ResourceId))
                            deleteEntry((EntityDto)itm);
                        var exprenseId = ListofShiftExpenses.Where(c => c.ShiftResourcesId.Equals(itm.Id)).FirstOrDefault();
                        if (exprenseId != null && exprenseId.ShiftResourcesId != 0)
                            deleteExpenseEntry((EntityDto)exprenseId);
                    }

                });
            }
        }
        public Command<EquipmentListModel> RemoveItemfromEquipmentGrid
        {
            get
            {
                return new Command<EquipmentListModel>((reservedReosurces) =>
                {
                    var item = TimesheetLineItems.Where(a => a.ResourceName.Equals(reservedReosurces.Name)).FirstOrDefault();
                    var id = TimesheetLineItems.Where(a => a.ResourceName.Equals(reservedReosurces.Name)).FirstOrDefault().shiftResourceId;
                    TimesheetLineItems.Remove(item);

                    if (id != 0)
                    {
                        var entryItem = ListofShiftResource.Where(b => b.Id.Equals(id)).FirstOrDefault();
                        deleteEntry((EntityDto)entryItem);
                    }
                });
            }
        }

        //Delete Selected Item entries from 
        private async void deleteEntry(EntityDto model)
        {
            await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.Delete(new EntityDto(model.Id)));
        }
        private async void deleteExpenseEntry(EntityDto model)
        {
            await WebRequestExecuter.Execute(async () => await _shiftExpensesAppService.Delete(new EntityDto(model.Id)));
        }
        //Filter and search
        public string FilterText
        {
            get => getAllResourceReservationsesInput.ResourcesNameFilter;
            set
            {
                getAllResourceReservationsesInput.ResourcesTypeFilter = "Employees";
                getAllResourceReservationsesInput.ResourcesNameFilter = value;
                Employees.Clear();
                AsyncRunner.Run(SearchWithDelayAsync(getAllResourceReservationsesInput.ResourcesNameFilter));
            }
        }
        public string FilterTextEquip
        {
            get => getAllResourceReservationsesInput.ResourcesNameFilter;
            set
            {
                getAllResourceReservationsesInput.ResourcesTypeFilter = "Equipment";
                getAllResourceReservationsesInput.ResourcesNameFilter = value;
                Equipment.Clear();
                AsyncRunner.Run(SearchWithDelayAsync(getAllResourceReservationsesInput.ResourcesNameFilter));
            }
        }
        private async Task SearchWithDelayAsync(string filterText)
        {

            if (!string.IsNullOrEmpty(filterText))
            {
                await Task.Delay(PageDefaults.SearchDelayMilliseconds);

                if (filterText != getAllResourceReservationsesInput.ResourcesNameFilter)
                {
                    return;
                }
            }

            RefreshEquipment();
        }
        private void RefreshEquipment()
        {
            getAllResourceReservationsesInput.SkipCount = 0;
            _currentPage = 0;
            PopulateReservedResource(
             () =>
             {
                 PopulateReservedEmployees();

             });
        }

        //Saving entries
        public string SaveTimesheetPerJob(TimesheetPerJobListModel dto)
        {
            try
            {

                AsyncRunner.Run(checkTimesheetExist(dto));
                return "You may now return to Employee and Equipment tab to view the time summary.";

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message.ToString();
            }

        }
        private async Task checkTimesheetExist(TimesheetPerJobListModel dto)
        {
            if (selectedJobandDate.Name == null)
            {
                return;
            }
            getAllTimesheetsInput.NameFilter = selectedJobandDate.Name.ToString();
            getAllTimesheetsInput.MaxCreatedDateFilter = selectedJobandDate.ScheduledStart;
            getAllTimesheetsInput.MinCreatedDateFilter = selectedJobandDate.ScheduledStart;
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _timesheetsAppService.GetAll(getAllTimesheetsInput), result =>
                {
                    var items = result;
                    var isExist = items.Items.Where(a => a.Timesheets.Name.Equals(selectedJobandDate.Name.ToString()) && selectedJobandDate.ScheduledStart.Equals(a.Timesheets.CreatedDate));
                    if (isExist.Count() > 0)
                    {
                        var existedItem = isExist.FirstOrDefault().Timesheets;
                        createOrEditTimesheetsDto = new CreateOrEditTimesheetsDto();
                        createOrEditTimesheetsDto.CreatedDate = existedItem.CreatedDate;
                        createOrEditTimesheetsDto.Id = existedItem.Id;
                        createOrEditTimesheetsDto.StatusesId = existedItem.StatusesId;
                        createOrEditTimesheetsDto.Name = existedItem.Name;
                        createOrEditTimesheetsDto.SubmitedDate = existedItem.SubmitedDate;
                        AsyncRunner.Run(checkIsShiftExist(dto));
                    }
                    else
                        AsyncRunner.Run(SaveTimesheetIntoTable(dto));

                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");

                var isExist = response.Where(a => a.Timesheets.Name.Equals(selectedJobandDate.Name.ToString()) && selectedJobandDate.ScheduledStart.Equals(a.Timesheets.CreatedDate));
                if (isExist.Count() > 0)
                {
                    var existedItem = isExist.FirstOrDefault().Timesheets;
                    createOrEditTimesheetsDto = new CreateOrEditTimesheetsDto();
                    createOrEditTimesheetsDto.CreatedDate = existedItem.CreatedDate;
                    createOrEditTimesheetsDto.Id = existedItem.Id;
                    createOrEditTimesheetsDto.StatusesId = existedItem.StatusesId;
                    createOrEditTimesheetsDto.Name = existedItem.Name;
                    createOrEditTimesheetsDto.SubmitedDate = existedItem.SubmitedDate;
                    AsyncRunner.Run(checkIsShiftExist(dto));
                }
                else
                    AsyncRunner.Run(SaveTimesheetIntoTable(dto));

            }

        }

        private async Task checkIsShiftExist(TimesheetPerJobListModel dto)
        {
            getAllShiftsInput.NameFilter = selectedJobandDate.Name;

            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _shiftsAppService.GetAll(getAllShiftsInput), result =>
                {
                    var items = result;
                    var isExist = items.Items.Where(a => a.JobsName.Equals(selectedJobandDate.Name.ToString()) && selectedJobandDate.ScheduledStart.Equals(a.Shifts.ScheduledStart));
                    if (isExist.Count() > 0)
                    {
                        var existedItem = isExist.FirstOrDefault().Shifts;
                        createOrEditShiftsDto = new CreateOrEditShiftsDto();
                        createOrEditShiftsDto.ScheduledStart = existedItem.ScheduledStart;
                        createOrEditShiftsDto.Id = existedItem.Id;
                        createOrEditShiftsDto.JobsId = existedItem.JobsId;
                        createOrEditShiftsDto.Name = existedItem.Name;
                        createOrEditShiftsDto.ScheduledEnd = existedItem.ScheduledEnd;
                        AsyncRunner.Run(checkTimesheetExistPerjob(dto));
                    }
                    else
                    {
                        AsyncRunner.Run(SaveJobInShiftsTable(dto));

                    }

                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetShiftsForViewDto>>("Shift");
                var isExist = response.Where(a => a.JobsName.Equals(selectedJobandDate.Name.ToString()) && selectedJobandDate.ScheduledStart.Equals(a.Shifts.ScheduledStart));
                if (isExist.Count() > 0)
                {
                    var existedItem = isExist.FirstOrDefault().Shifts;
                    createOrEditShiftsDto = new CreateOrEditShiftsDto();
                    createOrEditShiftsDto.ScheduledStart = existedItem.ScheduledStart;
                    createOrEditShiftsDto.Id = existedItem.Id;
                    createOrEditShiftsDto.JobsId = existedItem.JobsId;
                    createOrEditShiftsDto.Name = existedItem.Name;
                    createOrEditShiftsDto.ScheduledEnd = existedItem.ScheduledEnd;
                    AsyncRunner.Run(checkTimesheetExistPerjob(dto));
                }
                else
                {
                    AsyncRunner.Run(SaveJobInShiftsTable(dto));

                }
            }

        }
        private async Task SaveTimesheetIntoTable(TimesheetPerJobListModel dto)
        {
            if (runThisOnetimeOnly.Equals(1))
            {
                runThisOnetimeOnly = 2;
                createOrEditTimesheetsDto.CreatedDate = selectedJobandDate.ScheduledStart;
                createOrEditTimesheetsDto.Name = selectedJobandDate.Name.ToString();
                createOrEditTimesheetsDto.StatusesId = 3;
                if (CrossConnectivity.Current.IsConnected)
                {
                    await WebRequestExecuter.Execute(async () => await _timesheetsAppService.CreateOrEdit(createOrEditTimesheetsDto));

                    GetAllTimesheetsInput getAllTimesheetsInput = new GetAllTimesheetsInput();
                    await WebRequestExecuter.Execute(async () => await _timesheetsAppService.GetAll(getAllTimesheetsInput), result =>
                    {
                        Barrel.Current.Empty("TimeSheet");
                        Barrel.Current.Add(key: "TimeSheet", data: result.Items, expireIn: TimeSpan.FromDays(1));
                        var response = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");
                        return Task.CompletedTask;
                    });
                }
                else
                {
                    var ress = Barrel.Current.Get<List<CreateOrEditTimesheetsDto>>("createOrEditTimesheets");
                    if (ress == null)
                        ress = new List<CreateOrEditTimesheetsDto>();
                    ress.Add(createOrEditTimesheetsDto);
                    Barrel.Current.Empty("createOrEditTimesheets");
                    Barrel.Current.Add(key: "createOrEditTimesheets", data: ress, expireIn: TimeSpan.FromDays(1));
                    var response = Barrel.Current.Get<List<GetTimesheetsForViewDto>>("TimeSheet");
                    var id = 0;
                    foreach (var i in response)
                    {
                        if (id < i.Timesheets.Id)
                        {
                            id = i.Timesheets.Id;
                        }
                    }
                    if (createOrEditTimesheetsDto.Id == null)
                    {
                        GetTimesheetsForViewDto res = new GetTimesheetsForViewDto()
                        {
                            StatusesName = "Create",
                            Timesheets = new TimesheetsDto()
                            {
                                CreatedDate = createOrEditTimesheetsDto.CreatedDate,
                                Id = id + 1,
                                Name = createOrEditTimesheetsDto.Name,
                                StatusesId = createOrEditTimesheetsDto.StatusesId,
                                SubmitedDate = createOrEditTimesheetsDto.SubmitedDate

                            }
                        };
                        response.Add(res);
                        Barrel.Current.Empty("TimeSheet");
                        Barrel.Current.Add(key: "TimeSheet", data: response, expireIn: TimeSpan.FromDays(1));
                        var q = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");
                    }
                }

            }

            AsyncRunner.Run(checkTimesheetExist(dto));

        }
        private async Task SaveJobInShiftsTable(TimesheetPerJobListModel dto)
        {
            if (runThisOnetimeOnly.Equals(2))
            {
                runThisOnetimeOnly = 3;
                createOrEditShiftsDto.ScheduledStart = selectedJobandDate.ScheduledStart;
                createOrEditShiftsDto.Name = selectedJobandDate.Name;
                createOrEditShiftsDto.JobsId = selectedJobandDate.JobsId;
                if (CrossConnectivity.Current.IsConnected)
                {
                    await WebRequestExecuter.Execute(async () => await _shiftsAppService.CreateOrEdit(createOrEditShiftsDto));
                    GetAllShiftsInput getAllShiftsInput = new GetAllShiftsInput();
                    await WebRequestExecuter.Execute(async () => await _shiftsAppService.GetAll(getAllShiftsInput), result =>
                    {
                        Barrel.Current.Empty("Shift");
                        Barrel.Current.Add(key: "Shift", data: result.Items, expireIn: TimeSpan.FromDays(1));
                        var response = Barrel.Current.Get<IReadOnlyList<GetShiftsForViewDto>>("Shift");
                        return Task.CompletedTask;
                    });
                }
                else
                {
                    var ress = Barrel.Current.Get<List<CreateOrEditShiftsDto>>("createOrEditShifts");
                    if (ress == null)
                        ress = new List<CreateOrEditShiftsDto>();
                    ress.Add(createOrEditShiftsDto);
                    Barrel.Current.Empty("createOrEditShifts");
                    Barrel.Current.Add(key: "createOrEditShifts", data: ress, expireIn: TimeSpan.FromDays(1));
                    var JobDB = Barrel.Current.Get<IReadOnlyList<GetJobsForViewDto>>("JobDB");
                    var Shift = Barrel.Current.Get<List<GetShiftsForViewDto>>("Shift");
                    var id = 0;
                    foreach (var i in Shift)
                    {
                        if (id < i.Shifts.Id)
                        {
                            id = i.Shifts.Id;
                        }
                    }
                    if (createOrEditShiftsDto.Id == null)
                    {
                        GetShiftsForViewDto res = new GetShiftsForViewDto()
                        {
                            JobsName = JobDB.WhereIf(!createOrEditShiftsDto.JobsId.Equals(0), e => e.Jobs.Id == createOrEditShiftsDto.JobsId).FirstOrDefault().Jobs.Name,
                            Shifts = new ShiftsDto()
                            {
                                Id = id + 1,
                                JobsId = createOrEditShiftsDto.JobsId,
                                Name = createOrEditShiftsDto.Name,
                                ScheduledStart = createOrEditShiftsDto.ScheduledStart
                            }
                        };

                        Shift.Add(res);

                    }

                    Barrel.Current.Empty("Shift");
                    Barrel.Current.Add(key: "Shift", data: Shift, expireIn: TimeSpan.FromDays(1));
                    var q = Barrel.Current.Get<IReadOnlyList<GetShiftsForViewDto>>("Shift");
                }
            }

            await checkIsShiftExist(dto);
        }
        private async Task checkTimesheetExistPerjob(TimesheetPerJobListModel timesheetPerJobListModel)
        {

            try
            {
                createOrEditShiftResourcesDto = new CreateOrEditShiftResourcesDto();
                getAllShiftResourcesInput.MaxCreatedDateFilter = selectedJobandDate.ScheduledStart;
                getAllShiftResourcesInput.MinCreatedDateFilter = selectedJobandDate.ScheduledStart;
                if (timesheetPerJobListModel.Type.Equals("Equipment"))
                {
                    if (!timesheetPerJobListModel.ResourceName.IsNullOrEmpty())
                        getAllShiftResourcesInput.ResourcesNameFilter = timesheetPerJobListModel.ResourceName;
                    if (!createOrEditShiftsDto.Name.IsNullOrEmpty())
                        getAllShiftResourcesInput.ShiftsNameFilter = createOrEditShiftsDto.Name;

                }
                else
                {
                    //Timesheet ID

                    if (!createOrEditTimesheetsDto.Name.IsNullOrEmpty())
                        getAllShiftResourcesInput.TimesheetsNameFilter = createOrEditTimesheetsDto.Name;
                    if (!createOrEditTimesheetsDto.Id.Equals(0))
                        getAllShiftResourcesInput.TimesheetId = (int)createOrEditTimesheetsDto.Id;

                    //Filter Job Name
                    if (!createOrEditShiftsDto.Name.IsNullOrEmpty())
                        getAllShiftResourcesInput.ShiftsNameFilter = createOrEditShiftsDto.Name;

                    //Filter Resource(Employee) Name
                    if (!timesheetPerJobListModel.ResourceName.IsNullOrEmpty())
                        getAllShiftResourcesInput.ResourcesNameFilter = timesheetPerJobListModel.ResourceName;


                }

                if (CrossConnectivity.Current.IsConnected)
                {
                    await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
                    {

                        var items = result;
                        if (items.Items.Count() > 0)
                        {
                            if (!Convert.ToString(timesheetPerJobListModel.SelectedPhaseCode).IsNullOrEmpty())
                                createOrEditShiftResourcesDto.JobPhaseCodesId = timesheetPerJobListModel.SelectedPhaseCode.Id;
                            if (!Convert.ToString(timesheetPerJobListModel.SelectedJobCategoryCode).IsNullOrEmpty())
                                createOrEditShiftResourcesDto.JobCategoriesId = timesheetPerJobListModel.SelectedJobCategoryCode.Id;
                            createOrEditShiftResourcesDto.ResourcesId = (int)timesheetPerJobListModel.ResourceId;
                            createOrEditShiftResourcesDto.ShiftsId = createOrEditShiftsDto.Id;
                            createOrEditShiftResourcesDto.TimesheetsId = createOrEditTimesheetsDto.Id;
                            createOrEditShiftResourcesDto.Name = _applicationContext.LoginInfo.User.Name;
                            //if It's Equipment 
                            if (timesheetPerJobListModel.Type.Equals("Equipment"))
                            {
                                createOrEditShiftResourcesDto.Id = items.Items.First().ShiftResources.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.EquipmentUsage);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            else
                            {

                                //check paytype 1 exist or create one 
                                if (items.Items.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section1.Code)).Count() > 0)
                                {
                                    var id = items.Items.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section1.Code)).FirstOrDefault().ShiftResources.Id;
                                    createOrEditShiftResourcesDto.Id = id;
                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section1.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype1Text);
                                    createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }
                                else
                                {
                                    var isExist = false; var id = 0;
                                    foreach (var i in items.Items)
                                    {
                                        if (PayTypes1.Where(a => a.Code.Equals(i.PayTypesCode)).Count() > 0)
                                        {
                                            isExist = true; id = i.ShiftResources.Id;
                                        }
                                    }
                                    if (isExist)
                                        createOrEditShiftResourcesDto.Id = id;
                                    else
                                        createOrEditShiftResourcesDto.Id = null;
                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section1.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype1Text);
                                    createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }
                                //check paytype 2 exist or create one 

                                if (items.Items.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section2.Code)).Count() > 0)
                                {
                                    var id = items.Items.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section2.Code)).FirstOrDefault().ShiftResources.Id;
                                    createOrEditShiftResourcesDto.Id = id;
                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section2.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype2Text);
                                    createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }
                                else if (items.Items.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section2.Code)).Count() == 0)
                                {
                                    var isExist = false; var id = 0;
                                    foreach (var i in items.Items)
                                    {
                                        if (PayTypes2.Where(a => a.Code.Equals(i.PayTypesCode)).Count() > 0)
                                        {
                                            isExist = true; id = i.ShiftResources.Id;
                                        }
                                    }
                                    if (isExist)
                                        createOrEditShiftResourcesDto.Id = id;
                                    else
                                        createOrEditShiftResourcesDto.Id = null;
                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section2.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype2Text);
                                    createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }
                                //check paytype 3 exist or create one 
                                if (items.Items.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section3.Code)).Count() > 0)
                                {
                                    var id = items.Items.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section3.Code)).FirstOrDefault().ShiftResources.Id;
                                    createOrEditShiftResourcesDto.Id = id;
                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section3.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype3Text);
                                    createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }
                                else if (items.Items.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section3.Code)).Count() == 0)
                                {
                                    var isExist = false; var id = 0;
                                    foreach (var i in items.Items)
                                    {
                                        if (PayTypes3.Where(a => a.Code.Equals(i.PayTypesCode)).Count() > 0)
                                        {
                                            isExist = true; id = i.ShiftResources.Id;
                                        }
                                    }
                                    if (isExist)
                                        createOrEditShiftResourcesDto.Id = id;
                                    else
                                        createOrEditShiftResourcesDto.Id = null;
                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section3.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype3Text);
                                    createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }

                                timesheetPerJobListModel.shiftResourceId = items.Items.FirstOrDefault().ShiftResources.Id;
                                if ((!timesheetPerJobListModel.PerDimText.Equals(0) && !timesheetPerJobListModel.PerDimText.ToString().IsNullOrEmpty()) || (!timesheetPerJobListModel.MiscText.Equals(0) && !timesheetPerJobListModel.MiscText.ToString().IsNullOrEmpty()))
                                {
                                    return getSavedShiftResourceId(timesheetPerJobListModel);
                                }

                            }

                        }
                        else
                        {
                            createOrEditShiftResourcesDto.ShiftsId = createOrEditShiftsDto.Id;
                            createOrEditShiftResourcesDto.TimesheetsId = createOrEditTimesheetsDto.Id;
                            createOrEditShiftResourcesDto.ResourcesId = (int)timesheetPerJobListModel.ResourceId;
                            createOrEditShiftResourcesDto.Name = _applicationContext.LoginInfo.User.Name;

                            if (!Convert.ToString(timesheetPerJobListModel.SelectedPhaseCode).IsNullOrEmpty())
                                createOrEditShiftResourcesDto.JobPhaseCodesId = timesheetPerJobListModel.SelectedPhaseCode.Id;
                            if (!Convert.ToString(timesheetPerJobListModel.SelectedJobCategoryCode).IsNullOrEmpty())
                                createOrEditShiftResourcesDto.JobCategoriesId = timesheetPerJobListModel.SelectedJobCategoryCode.Id;
                            if (timesheetPerJobListModel.Type.Equals("Equipment"))
                            {
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.EquipmentUsage);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            else
                            {
                                createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a != null && a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                                if (!timesheetPerJobListModel.Paytype1Text.IsNullOrEmpty())
                                {

                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section1.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype1Text);
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }
                                if (!timesheetPerJobListModel.Paytype2Text.IsNullOrEmpty())
                                {
                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section2.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype2Text);
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }
                                if (!timesheetPerJobListModel.Paytype3Text.IsNullOrEmpty())
                                {
                                    createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section3.Id;
                                    createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype3Text);
                                    AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                                }
                                if ((!timesheetPerJobListModel.PerDimText.Equals(0) && !Convert.ToString(timesheetPerJobListModel.PerDimText).IsNullOrEmpty()) || (!timesheetPerJobListModel.MiscText.Equals(0) && !timesheetPerJobListModel.MiscText.ToString().IsNullOrEmpty()))
                                {
                                    return getSavedShiftResourceId(timesheetPerJobListModel);
                                }
                                return Task.CompletedTask;
                            }
                        }
                        return Task.CompletedTask;

                    });
                }
                else
                {
                    var response = Barrel.Current.Get<IReadOnlyList<GetShiftResourcesForViewDto>>("ShiftResources");
                    var exist = response
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.NameFilter), e => e.ShiftResources.Name == getAllShiftResourcesInput.NameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.ResourcesNameFilter), e => e.ResourcesName == getAllShiftResourcesInput.ResourcesNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.TimesheetsNameFilter), e => e.TimesheetsName == getAllShiftResourcesInput.TimesheetsNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.ShiftsNameFilter), e => e.ShiftsName == getAllShiftResourcesInput.ShiftsNameFilter)
                            .WhereIf(!getAllShiftResourcesInput.TimesheetId.Equals(0) && !getAllShiftResourcesInput.TimesheetId.Equals(null), e => e.ShiftResources.TimesheetsId == getAllShiftResourcesInput.TimesheetId);

                    if (exist.Count() > 0)
                    {
                        if (!Convert.ToString(timesheetPerJobListModel.SelectedPhaseCode).IsNullOrEmpty())
                            createOrEditShiftResourcesDto.JobPhaseCodesId = timesheetPerJobListModel.SelectedPhaseCode.Id;
                        if (!Convert.ToString(timesheetPerJobListModel.SelectedJobCategoryCode).IsNullOrEmpty())
                            createOrEditShiftResourcesDto.JobCategoriesId = timesheetPerJobListModel.SelectedJobCategoryCode.Id;
                        createOrEditShiftResourcesDto.ResourcesId = (int)timesheetPerJobListModel.ResourceId;
                        createOrEditShiftResourcesDto.ShiftsId = createOrEditShiftsDto.Id;
                        createOrEditShiftResourcesDto.TimesheetsId = createOrEditTimesheetsDto.Id;
                        createOrEditShiftResourcesDto.Name = _applicationContext.LoginInfo.User.Name;
                        //if It's Equipment 
                        if (timesheetPerJobListModel.Type.Equals("Equipment"))
                        {
                            createOrEditShiftResourcesDto.Id = exist.First().ShiftResources.Id;
                            createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.EquipmentUsage);
                            AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                        }
                        else
                        {
                            createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                            //check paytype 1 exist or create one 
                            if (exist.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section1.Code)).Count() > 0)
                            {
                                var id = exist.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section1.Code)).FirstOrDefault().ShiftResources.Id;
                                createOrEditShiftResourcesDto.Id = id;
                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section1.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype1Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            else
                            {
                                var isExist = false; var id = 0;
                                foreach (var i in exist)
                                {
                                    if (PayTypes1.Where(a => a.Code.Equals(i.PayTypesCode)).Count() > 0)
                                    {
                                        isExist = true; id = i.ShiftResources.Id;
                                    }
                                }
                                if (isExist)
                                    createOrEditShiftResourcesDto.Id = id;
                                else
                                    createOrEditShiftResourcesDto.Id = null;
                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section1.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype1Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            //check paytype 2 exist or create one 

                            if (exist.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section2.Code)).Count() > 0)
                            {
                                var id = exist.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section2.Code)).FirstOrDefault().ShiftResources.Id;
                                createOrEditShiftResourcesDto.Id = id;
                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section2.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype2Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            else if (exist.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section2.Code)).Count() == 0)
                            {
                                var isExist = false; var id = 0;
                                foreach (var i in exist)
                                {
                                    if (PayTypes2.Where(a => a.Code.Equals(i.PayTypesCode)).Count() > 0)
                                    {
                                        isExist = true; id = i.ShiftResources.Id;
                                    }
                                }
                                if (isExist)
                                    createOrEditShiftResourcesDto.Id = id;
                                else
                                    createOrEditShiftResourcesDto.Id = null;
                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section2.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype2Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            //check paytype 3 exist or create one 
                            if (exist.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section3.Code)).Count() > 0)
                            {
                                var id = exist.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section3.Code)).FirstOrDefault().ShiftResources.Id;
                                createOrEditShiftResourcesDto.Id = id;
                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section3.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype3Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            else if (exist.Where(a => a.PayTypesCode.Equals(timesheetPerJobListModel.Section3.Code)).Count() == 0)
                            {
                                var isExist = false; var id = 0;
                                foreach (var i in exist)
                                {
                                    if (PayTypes3.Where(a => a.Code.Equals(i.PayTypesCode)).Count() > 0)
                                    {
                                        isExist = true; id = i.ShiftResources.Id;
                                    }
                                }
                                if (isExist)
                                    createOrEditShiftResourcesDto.Id = id;
                                else
                                    createOrEditShiftResourcesDto.Id = null;
                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section3.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype3Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }

                            timesheetPerJobListModel.shiftResourceId = exist.FirstOrDefault().ShiftResources.Id;
                            if ((!timesheetPerJobListModel.PerDimText.Equals(0) && !timesheetPerJobListModel.PerDimText.ToString().IsNullOrEmpty()) || (!timesheetPerJobListModel.MiscText.Equals(0) && !timesheetPerJobListModel.MiscText.ToString().IsNullOrEmpty()))
                            {
                                await getSavedShiftResourceId(timesheetPerJobListModel);
                            }

                        }

                    }
                    else
                    {
                        createOrEditShiftResourcesDto.ShiftsId = createOrEditShiftsDto.Id;
                        createOrEditShiftResourcesDto.TimesheetsId = createOrEditTimesheetsDto.Id;
                        createOrEditShiftResourcesDto.ResourcesId = (int)timesheetPerJobListModel.ResourceId;
                        createOrEditShiftResourcesDto.Name = _applicationContext.LoginInfo.User.Name;
                        createOrEditShiftResourcesDto.WorkerClaseesId = workerClaseesDto.Where(a => a.Code != null && a.Code.Equals(timesheetPerJobListModel.WorkerClass.Code)).FirstOrDefault().Id;
                        if (!Convert.ToString(timesheetPerJobListModel.SelectedPhaseCode).IsNullOrEmpty())
                            createOrEditShiftResourcesDto.JobPhaseCodesId = timesheetPerJobListModel.SelectedPhaseCode.Id;
                        if (!Convert.ToString(timesheetPerJobListModel.SelectedJobCategoryCode).IsNullOrEmpty())
                            createOrEditShiftResourcesDto.JobCategoriesId = timesheetPerJobListModel.SelectedJobCategoryCode.Id;
                        if (timesheetPerJobListModel.Type.Equals("Equipment"))
                        {
                            createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.EquipmentUsage);
                            AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                        }
                        else
                        {
                            if (!timesheetPerJobListModel.Paytype1Text.IsNullOrEmpty())
                            {

                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section1.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype1Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            if (!timesheetPerJobListModel.Paytype2Text.IsNullOrEmpty())
                            {
                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section2.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype2Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            if (!timesheetPerJobListModel.Paytype3Text.IsNullOrEmpty())
                            {
                                createOrEditShiftResourcesDto.PayTypesId = timesheetPerJobListModel.Section3.Id;
                                createOrEditShiftResourcesDto.HoursWorked = Convert.ToDecimal(timesheetPerJobListModel.Paytype3Text);
                                AsyncRunner.Run(SaveTimesheetPerJobInShiftResourceTable(createOrEditShiftResourcesDto));
                            }
                            if ((!timesheetPerJobListModel.PerDimText.Equals(0) && !Convert.ToString(timesheetPerJobListModel.PerDimText).IsNullOrEmpty()) || (!timesheetPerJobListModel.MiscText.Equals(0) && !timesheetPerJobListModel.MiscText.ToString().IsNullOrEmpty()))
                            {
                                await getSavedShiftResourceId(timesheetPerJobListModel);
                            }
                            await Task.CompletedTask;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }

        private async void checkIsExpenseExist(int id, string resourceName)
        {
            GetAllShiftExpensesInput getAllShiftExpensesInput = new GetAllShiftExpensesInput
            {
                ShiftResourcesIdFilter = id
            };
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _shiftExpensesAppService.GetAll(getAllShiftExpensesInput), result =>
                {
                    var results = result;
                    foreach (var i in results.Items)
                    {
                        var timesheetitem = TimesheetLineItems.Where(a => a.ResourceName.Equals(resourceName)).First();
                        ListofShiftExpenses.Add(new ShiftExpensesDto() { ShiftResourcesId = i.ShiftExpenses.ShiftResourcesId });
                        if (timesheetitem.MiscText.IsNullOrEmpty() && i.ShiftExpenses.Name.Equals("Misc"))
                        {
                            timesheetitem.MiscText = i.ShiftExpenses.Amount.ToString();
                            if (!i.ExpenseTypesName.ToString().IsNullOrEmpty())
                                timesheetitem.SelectedDesctiption = ExpenseTypes.Where(a => a.Name.Equals(i.ExpenseTypesName)).FirstOrDefault().Name;
                            selectedDescription = i.ExpenseTypesName;
                        }
                        else
                        if (timesheetitem.PerDimText.IsNullOrEmpty() && i.ShiftExpenses.Name.Equals("Per Dim"))
                        {
                            timesheetitem.PerDimText = i.ShiftExpenses.Amount.ToString();
                            if (!i.ExpenseTypesName.ToString().IsNullOrEmpty())
                                timesheetitem.SelectedDesctiption = ExpenseTypes.Where(a => a.Name.Equals(i.ExpenseTypesName)).FirstOrDefault().Name;
                            selectedDescription = i.ExpenseTypesName;
                        }

                    }
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetShiftExpensesForViewDto>>("ShiftExpand");
                var exist = response.WhereIf(!getAllShiftExpensesInput.ShiftResourcesIdFilter.Equals(0), e => e.ShiftExpenses.ShiftResourcesId != null && e.ShiftExpenses.ShiftResourcesId.Equals(getAllShiftExpensesInput.ShiftResourcesIdFilter));
                foreach (var i in exist)
                {
                    var timesheetitem = TimesheetLineItems.Where(a => a.ResourceName.Equals(resourceName)).First();
                    if (timesheetitem.MiscText.IsNullOrEmpty() && i.ShiftExpenses.Name.Equals("Misc"))
                    {
                        timesheetitem.MiscText = i.ShiftExpenses.Amount.ToString();
                        if (!i.ExpenseTypesName.ToString().IsNullOrEmpty())
                            timesheetitem.SelectedDesctiption = ExpenseTypes.Where(a => a.Name.Equals(i.ExpenseTypesName)).FirstOrDefault().Name;
                        selectedDescription = i.ExpenseTypesName;
                    }
                    else
                    if (timesheetitem.PerDimText.IsNullOrEmpty() && i.ShiftExpenses.Name.Equals("Per Dim"))
                    {
                        timesheetitem.PerDimText = i.ShiftExpenses.Amount.ToString();
                        if (!i.ExpenseTypesName.ToString().IsNullOrEmpty())
                            timesheetitem.SelectedDesctiption = ExpenseTypes.Where(a => a.Name.Equals(i.ExpenseTypesName)).FirstOrDefault().Name;
                        selectedDescription = i.ExpenseTypesName;
                    }

                }
            }

        }

        private async Task getSavedShiftResourceId(TimesheetPerJobListModel timesheetPerJobListModel)
        {

            if (!timesheetPerJobListModel.shiftResourceId.Equals(0) && !timesheetPerJobListModel.shiftResourceId.ToString().IsNullOrEmpty())
            {
                await checkIsExpenseExist(timesheetPerJobListModel.shiftResourceId, timesheetPerJobListModel);
            }
            else
            {
                getAllShiftResourcesInput.TimesheetId = (int)createOrEditTimesheetsDto.Id;
                getAllShiftResourcesInput.ShiftsNameFilter = createOrEditTimesheetsDto.Name;
                getAllShiftResourcesInput.ResourcesNameFilter = timesheetPerJobListModel.ResourceName;
                if (CrossConnectivity.Current.IsConnected)
                {
                    await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
                    {
                        var results = result;
                        timesheetPerJobListModel.shiftResourceId = results.Items.Count() > 0 ? results.Items.FirstOrDefault().ShiftResources.Id : 0;
                        if (!timesheetPerJobListModel.shiftResourceId.Equals(0))
                            return checkIsExpenseExist(timesheetPerJobListModel.shiftResourceId, timesheetPerJobListModel);
                        else
                            return Task.CompletedTask;
                    });
                }
                else
                {
                    var response = Barrel.Current.Get<IReadOnlyList<GetShiftResourcesForViewDto>>("ShiftResources");
                    var exist = response
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.NameFilter), e => e.ShiftResources.Name == getAllShiftResourcesInput.NameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.ResourcesNameFilter), e => e.ResourcesName == getAllShiftResourcesInput.ResourcesNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.ShiftsNameFilter), e => e.ShiftsName == getAllShiftResourcesInput.ShiftsNameFilter)
                            .WhereIf(getAllShiftResourcesInput.TimesheetId != 0, e => e.ShiftResources.TimesheetsId == getAllShiftResourcesInput.TimesheetId);
                    timesheetPerJobListModel.shiftResourceId = exist.Count() > 0 ? exist.FirstOrDefault().ShiftResources.Id : 0;
                    if (!timesheetPerJobListModel.shiftResourceId.Equals(0))
                        await checkIsExpenseExist(timesheetPerJobListModel.shiftResourceId, timesheetPerJobListModel);
                    else
                        return;
                }

            }


        }

        private async Task checkIsExpenseExist(int shiftResorceId, TimesheetPerJobListModel timesheetPerJobListModel)
        {
            GetAllShiftExpensesInput getAllShiftExpensesInput = new GetAllShiftExpensesInput
            {
                ShiftResourcesIdFilter = shiftResorceId
            };

            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _shiftExpensesAppService.GetAll(getAllShiftExpensesInput), result =>
                {
                    var results = result;
                    if (results.Items.Count() > 0)
                    {
                        //Per Dim 
                        if (results.Items.Where(a => a.ShiftExpenses.Name.Equals("Per Dim")).Count() > 0)
                        {
                            CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();
                            createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.PerDimText);
                            createOrEditShiftExpensesDto.Name = "Per Dim";
                            createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                            createOrEditShiftExpensesDto.Id = results.Items.Where(a => a.ShiftExpenses.Name.Equals("Per Dim")).FirstOrDefault().ShiftExpenses.Id;
                            if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                                createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;

                            AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                        }
                        else if (results.Items.Where(a => a.ShiftExpenses.Name.Equals("Per Dim")).Count() == 0)
                        {
                            CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();
                            createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.PerDimText);
                            createOrEditShiftExpensesDto.Name = "Per Dim";
                            createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                            if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                                createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;

                            AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                        }
                        //Misc
                        if (results.Items.Where(a => a.ShiftExpenses.Name.Equals("Misc")).Count() > 0)
                        {
                            CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();
                            createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.MiscText);
                            createOrEditShiftExpensesDto.Name = "Misc";
                            createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                            createOrEditShiftExpensesDto.Id = results.Items.Where(a => a.ShiftExpenses.Name.Equals("Misc")).FirstOrDefault().ShiftExpenses.Id;
                            if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                                createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;

                            AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                        }
                        else if (results.Items.Where(a => a.ShiftExpenses.Name.Equals("Misc")).Count() == 0)
                        {
                            CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();
                            createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.MiscText);
                            createOrEditShiftExpensesDto.Name = "Misc";
                            createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                            if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                                createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;

                            AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                        }

                        return Task.CompletedTask;
                    }
                    else
                    {

                        if (!timesheetPerJobListModel.PerDimText.Equals(0) || !Convert.ToString(timesheetPerJobListModel.PerDimText).IsNullOrEmpty())
                        {
                            CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();

                            createOrEditShiftExpensesDto.Name = "Per Dim";
                            createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                            createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.PerDimText);
                            if (!selectedDescription.IsNullOrEmpty() && !selectedDescription.Equals("N/A"))
                                createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(selectedDescription)).FirstOrDefault().Id;
                            AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));

                        }
                        if (!timesheetPerJobListModel.MiscText.Equals(0) || !Convert.ToString(timesheetPerJobListModel.MiscText).IsNullOrEmpty())
                        {
                            CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto
                            {
                                Name = "Misc",
                                ShiftResourcesId = shiftResorceId,
                                Amount = Convert.ToDecimal(timesheetPerJobListModel.MiscText)
                            };
                            if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                                createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;
                            AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                        }

                        return Task.CompletedTask;
                    }

                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetShiftExpensesForViewDto>>("ShiftExpand");
                var exist = response.WhereIf(!getAllShiftExpensesInput.ShiftResourcesIdFilter.Equals(0), e => e.ShiftExpenses.ShiftResourcesId != null && e.ShiftExpenses.ShiftResourcesId.Equals(getAllShiftExpensesInput.ShiftResourcesIdFilter));
                if (exist.Count() > 0)
                {
                    //Per Dim 
                    if (exist.Where(a => a.ShiftExpenses.Name.Equals("Per Dim")).Count() > 0)
                    {
                        CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();
                        createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.PerDimText);
                        createOrEditShiftExpensesDto.Name = "Per Dim";
                        createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                        createOrEditShiftExpensesDto.Id = exist.Where(a => a.ShiftExpenses.Name.Equals("Per Dim")).FirstOrDefault().ShiftExpenses.Id;
                        if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                            createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;

                        AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                    }
                    else if (exist.Where(a => a.ShiftExpenses.Name.Equals("Per Dim")).Count() == 0)
                    {
                        CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();
                        createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.PerDimText);
                        createOrEditShiftExpensesDto.Name = "Per Dim";
                        createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                        if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                            createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;

                        AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                    }
                    //Misc
                    if (exist.Where(a => a.ShiftExpenses.Name.Equals("Misc")).Count() > 0)
                    {
                        CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();
                        createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.MiscText);
                        createOrEditShiftExpensesDto.Name = "Misc";
                        createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                        createOrEditShiftExpensesDto.Id = exist.Where(a => a.ShiftExpenses.Name.Equals("Misc")).FirstOrDefault().ShiftExpenses.Id;
                        if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                            createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;

                        AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                    }
                    else if (exist.Where(a => a.ShiftExpenses.Name.Equals("Misc")).Count() == 0)
                    {
                        CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();
                        createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.MiscText);
                        createOrEditShiftExpensesDto.Name = "Misc";
                        createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                        if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                            createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;

                        AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                    }

                    return;
                }
                else
                {

                    if (!timesheetPerJobListModel.PerDimText.Equals(0) || !Convert.ToString(timesheetPerJobListModel.PerDimText).IsNullOrEmpty())
                    {
                        CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto();

                        createOrEditShiftExpensesDto.Name = "Per Dim";
                        createOrEditShiftExpensesDto.ShiftResourcesId = shiftResorceId;
                        createOrEditShiftExpensesDto.Amount = Convert.ToDecimal(timesheetPerJobListModel.PerDimText);
                        if (!selectedDescription.IsNullOrEmpty() && !selectedDescription.Equals("N/A"))
                            createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(selectedDescription)).FirstOrDefault().Id;
                        AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));

                    }
                    if (!timesheetPerJobListModel.MiscText.Equals(0) || !Convert.ToString(timesheetPerJobListModel.MiscText).IsNullOrEmpty())
                    {
                        CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto = new CreateOrEditShiftExpensesDto
                        {
                            Name = "Misc",
                            ShiftResourcesId = shiftResorceId,
                            Amount = Convert.ToDecimal(timesheetPerJobListModel.MiscText)
                        };
                        if (!timesheetPerJobListModel.SelectedDesctiption.IsNullOrEmpty())
                            createOrEditShiftExpensesDto.ExpenseTypesId = ExpenseTypes.Where(a => a.Name.Equals(timesheetPerJobListModel.SelectedDesctiption)).FirstOrDefault().Id;
                        AsyncRunner.Run(SaveOrUpdateExpenseType(createOrEditShiftExpensesDto));
                    }

                    return;
                }

            }
        }

        private async Task SaveOrUpdateExpenseType(CreateOrEditShiftExpensesDto createOrEditShiftExpensesDto)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _shiftExpensesAppService.CreateOrEdit(createOrEditShiftExpensesDto));
                GetAllShiftExpensesInput getAllShiftExpensesInput = new GetAllShiftExpensesInput();
                await WebRequestExecuter.Execute(async () => await _shiftExpensesAppService.GetAll(getAllShiftExpensesInput), result =>
                {
                    Barrel.Current.Empty("ShiftExpand");
                    Barrel.Current.Add(key: "ShiftExpand", data: result.Items, expireIn: TimeSpan.FromDays(1));
                    var response = Barrel.Current.Get<IReadOnlyList<GetShiftExpensesForViewDto>>("ShiftExpand");
                    return Task.CompletedTask;
                });
            }
            else
            {
                var ress = Barrel.Current.Get<List<CreateOrEditShiftExpensesDto>>("createOrEditShiftExpensesDto");
                if (ress == null)
                    ress = new List<CreateOrEditShiftExpensesDto>();
                ress.Add(createOrEditShiftExpensesDto);
                Barrel.Current.Empty("createOrEditShiftExpensesDto");
                Barrel.Current.Add(key: "createOrEditShiftExpensesDto", data: ress, expireIn: TimeSpan.FromDays(1));
                var ExpenseType = Barrel.Current.Get<IReadOnlyList<GetExpenseTypesForViewDto>>("ExpenseType");
                var ShiftResources = Barrel.Current.Get<IReadOnlyList<GetShiftResourcesForViewDto>>("ShiftResources");
                var ShiftExpand = Barrel.Current.Get<List<GetShiftExpensesForViewDto>>("ShiftExpand");
                var id = 0;
                foreach (var i in ShiftExpand)
                {
                    if (id < i.ShiftExpenses.Id)
                    {
                        id = i.ShiftExpenses.Id;
                    }
                }
                if (createOrEditShiftExpensesDto.Id == null)
                {
                    var res = new GetShiftExpensesForViewDto()
                    {
                        ExpenseTypesName = ExpenseType.WhereIf(!createOrEditShiftExpensesDto.ExpenseTypesId.Equals(0) && !createOrEditShiftExpensesDto.ExpenseTypesId.Equals(null), e => e.ExpenseTypes.Id == createOrEditShiftExpensesDto.ExpenseTypesId).FirstOrDefault().ExpenseTypes.Name,
                        ShiftResourcesName = ShiftResources.WhereIf(!createOrEditShiftExpensesDto.ShiftResourcesId.Equals(0) && !createOrEditShiftExpensesDto.ShiftResourcesId.Equals(null), e => e.ShiftResources.Id == createOrEditShiftExpensesDto.ShiftResourcesId).FirstOrDefault().ShiftResources.Name,
                        ShiftExpenses = new ShiftExpensesDto()
                        {
                            ExpenseTypesId = createOrEditShiftExpensesDto.ExpenseTypesId,
                            ShiftResourcesId = createOrEditShiftExpensesDto.ShiftResourcesId,
                            Name = createOrEditShiftExpensesDto.Name,
                            Id = id + 1,
                            Amount = createOrEditShiftExpensesDto.Amount
                        }
                    };
                    ShiftExpand.Add(res);

                }
                else
                {
                    foreach (var i in ShiftExpand)
                    {
                        if (i.ShiftExpenses.Id == createOrEditShiftExpensesDto.Id)
                        {
                            if (!createOrEditShiftExpensesDto.ExpenseTypesId.Equals(0) && !createOrEditShiftExpensesDto.ExpenseTypesId.Equals(null))
                            {
                                i.ExpenseTypesName = ExpenseType.WhereIf(!createOrEditShiftExpensesDto.ExpenseTypesId.Equals(0) && !createOrEditShiftExpensesDto.ExpenseTypesId.Equals(null), e => e.ExpenseTypes.Id == createOrEditShiftExpensesDto.ExpenseTypesId).FirstOrDefault().ExpenseTypes.Name;
                                i.ShiftExpenses.ExpenseTypesId = createOrEditShiftExpensesDto.ExpenseTypesId;
                            }
                            if (!createOrEditShiftExpensesDto.ShiftResourcesId.Equals(0) && !createOrEditShiftExpensesDto.ShiftResourcesId.Equals(null))
                            {
                                i.ShiftResourcesName = ShiftResources.WhereIf(!createOrEditShiftExpensesDto.ShiftResourcesId.Equals(0) && !createOrEditShiftExpensesDto.ShiftResourcesId.Equals(null), e => e.ShiftResources.Id == createOrEditShiftExpensesDto.ShiftResourcesId).FirstOrDefault().ShiftResources.Name;
                                i.ShiftExpenses.ShiftResourcesId = createOrEditShiftExpensesDto.ShiftResourcesId;
                            }
                            if (!string.IsNullOrWhiteSpace(createOrEditShiftExpensesDto.Name))
                            {
                                i.ShiftExpenses.Name = createOrEditShiftExpensesDto.Name;
                            }
                            if (!createOrEditShiftExpensesDto.Amount.Equals(null) && !createOrEditShiftExpensesDto.Amount.Equals(0))
                                i.ShiftExpenses.Amount = createOrEditShiftExpensesDto.Amount;
                        }
                    }
                }

                Barrel.Current.Empty("ShiftExpand");
                Barrel.Current.Add(key: "ShiftExpand", data: ShiftExpand, expireIn: TimeSpan.FromDays(1));
            }
        }

        private async Task SaveTimesheetPerJobInShiftResourceTable(CreateOrEditShiftResourcesDto createOrEditShiftResourcesDto)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.CreateOrEdit(createOrEditShiftResourcesDto));
                GetAllShiftResourcesInput getAllShiftResourcesInput = new GetAllShiftResourcesInput();
                await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
                {
                    Barrel.Current.Empty("ShiftResources");
                    Barrel.Current.Add(key: "ShiftResources", data: result.Items, expireIn: TimeSpan.FromDays(1));
                    var response = Barrel.Current.Get<IReadOnlyList<GetShiftResourcesForViewDto>>("ShiftResources");
                    return Task.CompletedTask;
                });
            }
            else
            {
                var ress = Barrel.Current.Get<List<CreateOrEditShiftResourcesDto>>("createOrEditShiftResourcesDto");
                if (ress == null)
                    ress = new List<CreateOrEditShiftResourcesDto>();
                ress.Add(createOrEditShiftResourcesDto);
                Barrel.Current.Empty("createOrEditShiftResourcesDto");
                Barrel.Current.Add(key: "createOrEditShiftResourcesDto", data: ress, expireIn: TimeSpan.FromDays(1));
                var JobCategories = Barrel.Current.Get<IReadOnlyList<GetJobCategoriesForViewDto>>("JobCategories");
                var PhaseCode = Barrel.Current.Get<IReadOnlyList<GetJobPhaseCodesForViewDto>>("PhaseCode");
                var Resourceses = Barrel.Current.Get<IReadOnlyList<GetResourcesForViewDto>>("Resourceses");
                var PayTypes = Barrel.Current.Get<IReadOnlyList<GetPayTypesForViewDto>>("PayTypes");
                var TimeSheet = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");
                var Shift = Barrel.Current.Get<IReadOnlyList<GetShiftsForViewDto>>("Shift");
                var ShiftResources = Barrel.Current.Get<List<GetShiftResourcesForViewDto>>("ShiftResources");

                var id = 0;
                foreach (var i in ShiftResources)
                {
                    if (id < i.ShiftResources.Id)
                    {
                        id = i.ShiftResources.Id;
                    }
                }
                //Timesheet Existed already, Edit Task
                if (createOrEditShiftResourcesDto.Id == null)
                {
                    var res = new GetShiftResourcesForViewDto()
                    {
                        JobCategoriesName = JobCategories.WhereIf(!createOrEditShiftResourcesDto.JobCategoriesId.Equals(0) && !createOrEditShiftResourcesDto.JobCategoriesId.Equals(null), e => e.JobCategories.Id == createOrEditShiftResourcesDto.JobCategoriesId).FirstOrDefault().JobCategories.Name,
                        JobPhaseCodesName = PhaseCode.WhereIf(!createOrEditShiftResourcesDto.JobPhaseCodesId.Equals(0) && !createOrEditShiftResourcesDto.JobPhaseCodesId.Equals(null), e => e.JobPhaseCodes.Id == createOrEditShiftResourcesDto.JobPhaseCodesId).FirstOrDefault().JobPhaseCodes.Name,
                        ResourcesName = Resourceses.WhereIf(!createOrEditShiftResourcesDto.ResourcesId.Equals(0) && !createOrEditShiftResourcesDto.ResourcesId.Equals(null), e => e.Resources.Id == createOrEditShiftResourcesDto.ResourcesId).FirstOrDefault().Resources.Name,
                        PayTypesCode = PayTypes.WhereIf(!createOrEditShiftResourcesDto.PayTypesId.Equals(0) && !createOrEditShiftResourcesDto.PayTypesId.Equals(null), e => e.PayTypes.Id == createOrEditShiftResourcesDto.PayTypesId).FirstOrDefault().PayTypes.Code,
                        TimesheetsName = TimeSheet.WhereIf(!createOrEditShiftResourcesDto.TimesheetsId.Equals(0) && !createOrEditShiftResourcesDto.TimesheetsId.Equals(null), e => e.Timesheets.Id == createOrEditShiftResourcesDto.TimesheetsId).FirstOrDefault().Timesheets.Name,
                        ShiftsName = Shift.WhereIf(!createOrEditShiftResourcesDto.ShiftsId.Equals(0) && !createOrEditShiftResourcesDto.ShiftsId.Equals(null), e => e.Shifts.Id == createOrEditShiftResourcesDto.ShiftsId).FirstOrDefault().Shifts.Name,
                        ShiftResources = new ShiftResourcesDto()
                        {
                            HoursWorked = createOrEditShiftResourcesDto.HoursWorked,
                            Name = createOrEditShiftResourcesDto.Name,
                            Id = id + 1,
                            ResourcesId = createOrEditShiftResourcesDto.ResourcesId,
                            TimesheetsId = createOrEditShiftResourcesDto.TimesheetsId,
                            ShiftsId = createOrEditShiftResourcesDto.ShiftsId,
                            JobCategoriesId = createOrEditShiftResourcesDto.JobCategoriesId,
                            JobPhaseCodesId = createOrEditShiftResourcesDto.JobPhaseCodesId,
                            PayTypesId = createOrEditShiftResourcesDto.PayTypesId
                        }

                    };

                    ShiftResources.Add(res);

                }
                else // Create new timesheet Entry 
                {
                    foreach (var i in ShiftResources)
                    {
                        if (i.ShiftResources.Id == createOrEditShiftResourcesDto.Id)
                        {
                            if (!createOrEditShiftResourcesDto.JobCategoriesId.Equals(0) && !createOrEditShiftResourcesDto.JobCategoriesId.Equals(null))
                            {
                                i.JobCategoriesName = JobCategories.WhereIf(!createOrEditShiftResourcesDto.JobCategoriesId.Equals(0) && !createOrEditShiftResourcesDto.JobCategoriesId.Equals(null), e => e.JobCategories.Id == createOrEditShiftResourcesDto.JobCategoriesId).FirstOrDefault().JobCategories.Name;
                                i.ShiftResources.JobCategoriesId = createOrEditShiftResourcesDto.JobCategoriesId;
                            }

                            if (!createOrEditShiftResourcesDto.JobPhaseCodesId.Equals(0) && !createOrEditShiftResourcesDto.JobPhaseCodesId.Equals(null))
                            {
                                i.JobPhaseCodesName = PhaseCode.WhereIf(!createOrEditShiftResourcesDto.JobPhaseCodesId.Equals(0) && !createOrEditShiftResourcesDto.JobPhaseCodesId.Equals(null), e => e.JobPhaseCodes.Id == createOrEditShiftResourcesDto.JobPhaseCodesId).FirstOrDefault().JobPhaseCodes.Name;
                                i.ShiftResources.JobPhaseCodesId = createOrEditShiftResourcesDto.JobPhaseCodesId;
                            }

                            if (!createOrEditShiftResourcesDto.ResourcesId.Equals(0) && !createOrEditShiftResourcesDto.ResourcesId.Equals(null))
                            {
                                i.ResourcesName = Resourceses.WhereIf(!createOrEditShiftResourcesDto.ResourcesId.Equals(0) && !createOrEditShiftResourcesDto.ResourcesId.Equals(null), e => e.Resources.Id == createOrEditShiftResourcesDto.ResourcesId).FirstOrDefault().Resources.Name;
                                i.ShiftResources.ResourcesId = createOrEditShiftResourcesDto.ResourcesId;
                            }
                            if (!createOrEditShiftResourcesDto.PayTypesId.Equals(0) && !createOrEditShiftResourcesDto.PayTypesId.Equals(null))
                            {
                                i.PayTypesCode = PayTypes.WhereIf(!createOrEditShiftResourcesDto.PayTypesId.Equals(0) && !createOrEditShiftResourcesDto.PayTypesId.Equals(null), e => e.PayTypes.Id == createOrEditShiftResourcesDto.PayTypesId).FirstOrDefault().PayTypes.Code;
                                i.ShiftResources.PayTypesId = createOrEditShiftResourcesDto.PayTypesId;
                            }

                            if (!createOrEditShiftResourcesDto.TimesheetsId.Equals(0) && !createOrEditShiftResourcesDto.TimesheetsId.Equals(null))
                            {
                                i.TimesheetsName = TimeSheet.WhereIf(!createOrEditShiftResourcesDto.TimesheetsId.Equals(0) && !createOrEditShiftResourcesDto.TimesheetsId.Equals(null), e => e.Timesheets.Id == createOrEditShiftResourcesDto.TimesheetsId).FirstOrDefault().Timesheets.Name;
                                i.ShiftResources.TimesheetsId = createOrEditShiftResourcesDto.TimesheetsId;
                            }
                            if (!createOrEditShiftResourcesDto.ShiftsId.Equals(0) && !createOrEditShiftResourcesDto.ShiftsId.Equals(null))
                            {
                                i.ShiftsName = Shift.WhereIf(!createOrEditShiftResourcesDto.ShiftsId.Equals(0) && !createOrEditShiftResourcesDto.ShiftsId.Equals(null), e => e.Shifts.Id == createOrEditShiftResourcesDto.ShiftsId).FirstOrDefault().Shifts.Name;
                                i.ShiftResources.ShiftsId = createOrEditShiftResourcesDto.ShiftsId;
                            }
                            if (!createOrEditShiftResourcesDto.HoursWorked.Equals(0) && !createOrEditShiftResourcesDto.HoursWorked.Equals(null))
                                i.ShiftResources.HoursWorked = createOrEditShiftResourcesDto.HoursWorked;
                            if (!string.IsNullOrWhiteSpace(createOrEditShiftResourcesDto.Name))
                                i.ShiftResources.Name = createOrEditShiftResourcesDto.Name;
                        }
                    }
                }


                Barrel.Current.Empty("ShiftResources");
                Barrel.Current.Add(key: "ShiftResources", data: ShiftResources, expireIn: TimeSpan.FromDays(1));

            }
        }

        private async Task PageAppearingAsync()
        {
            Employees.Clear();
            await FetchEmployeesAsync();
            Equipment.Clear();
            await FetchEquipmentAsync();
            TimesheetLineItems.Clear();
            Toolbar_name = "Save";
            selectedcount = 0;
            await SetBusyAsync(PopulateExistedTimeSheet);

            if (!CrossConnectivity.Current.IsConnected && Settings.Entertime == 0)
            {
                InitTimer();
                Settings.Entertime = 1;
            }
            else if (CrossConnectivity.Current.IsConnected)
            {
                Settings.Entertime = 0;
            }
        }

        public int selectedcount = 0;

        public void selected_UsageCommandAsync(int resourceId)
        {
            foreach (var e in TimesheetLineItems)
            {
                if (e.ResourceId == resourceId)
                {
                    if (e.UsageColor == Color.Gray)
                    {
                        e.UsageColor = Color.White;
                        selectedcount--;
                        e.UsageEnabled = true;
                        if (selectedcount == 0)
                            Toolbar_name = "Save";
                    }
                    else
                    {
                        e.UsageColor = Color.Gray;
                        selectedcount++;
                        e.UsageEnabled = false;
                        if (selectedcount == 1)
                            Toolbar_name = "Edit";
                    }
                }
            }
        }

        public void selected_paytype1CommandAsync(int resourceId)
        {
            foreach (var e in TimesheetLineItems)
            {
                if (e.ResourceId == resourceId)
                {
                    if (e.paytype1Color == Color.Gray)
                    {
                        e.paytype1Color = Color.White;
                        selectedcount--;
                        e.paytype1Enabled = true;
                        if (selectedcount == 0)
                            Toolbar_name = "Save";
                    }
                    else
                    {
                        e.paytype1Color = Color.Gray;
                        selectedcount++;
                        e.paytype1Enabled = false;
                        if (selectedcount == 1)
                            Toolbar_name = "Edit";
                    }
                }
            }
        }

        public void selected_paytype2CommandAsync(int resourceId)
        {
            foreach (var e in TimesheetLineItems)
            {
                if (e.ResourceId == resourceId)
                {
                    if (e.paytype2Color == Color.Gray)
                    {
                        e.paytype2Color = Color.White;
                        selectedcount--;
                        e.paytype2Enabled = true;
                        if (selectedcount == 0)
                            Toolbar_name = "Save";
                    }
                    else
                    {
                        e.paytype2Color = Color.Gray;
                        selectedcount++;
                        e.paytype2Enabled = false;
                        if (selectedcount == 1)
                            Toolbar_name = "Edit";
                    }

                }
            }
        }

        public void selected_paytype3CommandAsync(int resourceId)
        {
            foreach (var e in TimesheetLineItems)
            {
                if (e.ResourceId == resourceId)
                {
                    if (e.paytype3Color == Color.Gray)
                    {
                        e.paytype3Color = Color.White;
                        selectedcount--;
                        e.paytype3Enabled = true;
                        if (selectedcount == 0)
                            Toolbar_name = "Save";
                    }
                    else
                    {
                        e.paytype3Color = Color.Gray;
                        selectedcount++;
                        e.paytype3Enabled = false;
                        if (selectedcount == 1)
                            Toolbar_name = "Edit";
                    }

                }
            }
        }

        public void selected_perDimCommandAsync(int resourceId)
        {
            foreach (var e in TimesheetLineItems)
            {
                if (e.ResourceId == resourceId)
                {
                    if (e.perDimColor == Color.Gray)
                    {
                        e.perDimColor = Color.White;
                        selectedcount--;
                        e.perdimEnabled = true;
                        if (selectedcount == 0)
                            Toolbar_name = "Save";
                    }
                    else
                    {
                        e.perDimColor = Color.Gray;
                        selectedcount++;
                        e.perdimEnabled = false;
                        if (selectedcount == 1)
                            Toolbar_name = "Edit";
                    }

                }
            }
        }

        public System.Timers.Timer timer1 = new System.Timers.Timer(1000);

        public void InitTimer()
        {

            timer1.AutoReset = true; // the key is here so it repeats
            timer1.Elapsed += timer1_tick;
            timer1.Start();

        }

        private void timer1_tick(object sender, EventArgs e)
        {
            AsyncRunner.Run(UploadingData());

        }

        public bool isuploaded = false;

        private async Task UploadingData()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                if (!isuploaded)
                {
                    await SetBusyAsync(async () =>
                    {
                        Settings.Entertime = 0;
                        isuploaded = true;
                        var createOrEditTimesheets = Barrel.Current.Get<List<CreateOrEditTimesheetsDto>>("createOrEditTimesheets");
                        if (createOrEditTimesheets != null && createOrEditTimesheets.Count != 0)
                        {
                            foreach (var i in createOrEditTimesheets)
                            {
                                AsyncRunner.Run(WebRequestExecuter.Execute(async () => await _timesheetsAppService.CreateOrEdit(i)));
                            }
                            Barrel.Current.Empty("createOrEditTimesheets");
                            GetAllTimesheetsInput getAllTimesheetsInput = new GetAllTimesheetsInput();
                            await WebRequestExecuter.Execute(async () => await _timesheetsAppService.GetAll(getAllTimesheetsInput), result =>
                            {
                                Barrel.Current.Empty("TimeSheet");
                                Barrel.Current.Add(key: "TimeSheet", data: result.Items, expireIn: TimeSpan.FromDays(1));
                                var response = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");
                                return Task.CompletedTask;
                            });
                        }


                        var createOrEditShifts = Barrel.Current.Get<List<CreateOrEditShiftsDto>>("createOrEditShifts");
                        if (createOrEditShifts != null && createOrEditShifts.Count != 0)
                        {
                            foreach (var i in createOrEditShifts)
                            {
                                AsyncRunner.Run(WebRequestExecuter.Execute(async () => await _shiftsAppService.CreateOrEdit(i)));
                            }
                            Barrel.Current.Empty("createOrEditShifts");

                            GetAllShiftsInput getAllShiftsInput = new GetAllShiftsInput();
                            await WebRequestExecuter.Execute(async () => await _shiftsAppService.GetAll(getAllShiftsInput), result =>
                            {
                                Barrel.Current.Empty("Shift");
                                Barrel.Current.Add(key: "Shift", data: result.Items, expireIn: TimeSpan.FromDays(1));
                                var response = Barrel.Current.Get<IReadOnlyList<GetShiftsForViewDto>>("Shift");
                                return Task.CompletedTask;
                            });
                        }

                        var createOrEditShiftResourcesDto = Barrel.Current.Get<List<CreateOrEditShiftResourcesDto>>("createOrEditShiftResourcesDto");

                        if (createOrEditShiftResourcesDto != null && createOrEditShiftResourcesDto.Count != 0)
                        {
                            foreach (var i in createOrEditShiftResourcesDto)
                            {
                                await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.CreateOrEdit(i));
                                await Task.Delay(1000);
                            }
                            Barrel.Current.Empty("createOrEditShiftResourcesDto");
                            GetAllShiftResourcesInput getAllShiftResourcesInput = new GetAllShiftResourcesInput();
                            await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
                            {
                                Barrel.Current.Empty("ShiftResources");
                                Barrel.Current.Add(key: "ShiftResources", data: result.Items, expireIn: TimeSpan.FromDays(1));
                                var response = Barrel.Current.Get<IReadOnlyList<GetShiftResourcesForViewDto>>("ShiftResources");
                                return Task.CompletedTask;
                            });
                        }

                        var createOrEditShiftExpensesDto = Barrel.Current.Get<List<CreateOrEditShiftExpensesDto>>("createOrEditShiftExpensesDto");

                        if (createOrEditShiftExpensesDto != null && createOrEditShiftExpensesDto.Count != 0)
                        {
                            foreach (var i in createOrEditShiftExpensesDto)
                            {
                                await WebRequestExecuter.Execute(async () => await _shiftExpensesAppService.CreateOrEdit(i));

                            }
                            Barrel.Current.Empty("createOrEditShiftExpensesDto");

                            GetAllShiftExpensesInput getAllShiftExpensesInput = new GetAllShiftExpensesInput();
                            await WebRequestExecuter.Execute(async () => await _shiftExpensesAppService.GetAll(getAllShiftExpensesInput), result =>
                            {
                                Barrel.Current.Empty("ShiftExpand");
                                Barrel.Current.Add(key: "ShiftExpand", data: result.Items, expireIn: TimeSpan.FromDays(1));
                                var response = Barrel.Current.Get<IReadOnlyList<GetShiftExpensesForViewDto>>("ShiftExpand");
                                return Task.CompletedTask;
                            });
                        }

                        timer1.Stop();
                    });

                }

            }
        }


        public void selected_miscCommandAsync(int resourceId)
        {
            foreach (var e in TimesheetLineItems)
            {
                if (e.ResourceId == resourceId)
                {

                    if (e.miscColor == Color.Gray)
                    {
                        e.miscColor = Color.White;
                        e.miscEnabled = true;
                        selectedcount--;
                        if (selectedcount == 0)
                            Toolbar_name = "Save";
                    }
                    else
                    {
                        e.miscColor = Color.Gray;
                        e.miscEnabled = false;
                        selectedcount++;
                        if (selectedcount == 1)
                            Toolbar_name = "Edit";
                    }

                }
            }
        }
    }
}

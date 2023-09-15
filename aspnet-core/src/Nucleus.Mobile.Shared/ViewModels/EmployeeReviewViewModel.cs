using Abp.AutoMapper;
using Abp.Collections.Extensions;
using JetBrains.Annotations;
using MonkeyCache.SQLite;
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Commands;
using Nucleus.Core.Dependency;
using Nucleus.Core.Threading;
using Nucleus.Localization;
using Nucleus.Models.Common;
using Nucleus.Models.Employees;
using Nucleus.PayType;
using Nucleus.PayType.Dtos;
using Nucleus.Resource;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ResourceWorkerInfo;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.ShiftResource;
using Nucleus.ShiftResource.Dtos;
using Nucleus.StatusUpdate;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.Timesheet.Dtos;
using Nucleus.ViewModels.Base;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Nucleus.ViewModels
{
    public class EmployeeReviewViewModel : XamarinViewModel
    {
        private string _title;
        public ObservableRangeCollection<PayTypesDto> PayTypes { get; set; }
        public ObservableRangeCollection<EmployeeReviewListModel> EmployeesToReviewAmount { get; set; }
        public GetAllShiftResourcesInput getAllShiftResourcesInput { get; set; }
      
        public DateTime selectedDate {get;set;}
        public string selectedDT { get; set; }
        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);
        //Service
        private readonly IPayTypesesAppService _payTypesAppService;
        private readonly IShiftResourcesAppService _shiftResourcesAppService;
        private readonly IApplicationContext _applicationContext;
        private readonly IStatusUpdatesAppService _statusUpdatesAppService;
        public EmployeeReviewViewModel(IPayTypesesAppService payTypesAppService, IStatusUpdatesAppService statusUpdatesAppService, IShiftResourcesAppService shiftResourcesAppService, IApplicationContext applicationContext)
        {
            _payTypesAppService = payTypesAppService;
            _shiftResourcesAppService = shiftResourcesAppService;
            _applicationContext = applicationContext;
            _statusUpdatesAppService = statusUpdatesAppService;
            _title = L.Localize("Employees Review");

            getAllShiftResourcesInput = new GetAllShiftResourcesInput();
            EmployeesToReviewAmount = new ObservableRangeCollection<EmployeeReviewListModel>();
          
        }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                RaisePropertyChanged(() => SelectedDate);
            }
        }
        public string SelectedDT
        {
            get => selectedDT;
            set
            {
                selectedDT = value;
                RaisePropertyChanged(() => SelectedDT);
            }
        }
        private async Task PageAppearingAsync()
        {
            EmployeesToReviewAmount.Clear();
            await SetBusyAsync(FetchDate);
        }

        private async Task FetchDate()
        {
            DateTime ds = selectedDate;
            getAllShiftResourcesInput.NameFilter = _applicationContext.LoginInfo.User.Name;
            getAllShiftResourcesInput.ResourcesTypeFilter = "Employees";
            getAllShiftResourcesInput.MinCreatedDateFilter = selectedDate;
            getAllShiftResourcesInput.MaxCreatedDateFilter = selectedDate;
            getAllShiftResourcesInput.MaxResultCount = 1000;

            if(CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
                {

                    var results = result;
                    var sortedList = results.Items.OrderBy(a => a.ResourcesName);
                    var temp = "";
                    foreach (var i in sortedList)
                    {
                        if (!i.ResourcesName.Equals(temp))
                        {
                            temp = i.ResourcesName;

                            EmployeesToReviewAmount.Add(new EmployeeReviewListModel
                            {
                                resourceName = i.ResourcesName,
                                ResourcesId = i.ShiftResources.ResourcesId
                            });

                        }
                        var emp = EmployeesToReviewAmount.Where(a => a.resourceName.Equals(temp)).FirstOrDefault();
                        if (i.PayTypesCode.Equals("REG"))
                            emp.RegAmount = emp.RegAmount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("OT"))
                            emp.OTamount = emp.OTamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("DT"))
                            emp.DTamount = emp.DTamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("OT1.8"))
                            emp.OT18amount = emp.OT18amount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("REG+10"))
                            emp.REG10amount = emp.REG10amount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("REG+15"))
                            emp.REG15amount = emp.REG15amount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("OT+10"))
                            emp.OT10amount = emp.OT10amount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("AWL"))
                            emp.AWLamount = emp.AWLamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("AWOL"))
                            emp.AWOLamount = emp.AWOLamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("ILL"))
                            emp.ILLamount = emp.ILLamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("VAC"))
                            emp.VACamount = emp.VACamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                        if (i.PayTypesCode.Equals("HOL"))
                            emp.HOLamount = emp.HOLamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    }
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetShiftResourcesForViewDto>>("ShiftResources");
                var resourceses = Barrel.Current.Get<IReadOnlyList<GetResourcesForViewDto>>("Resourceses");
                var timeSheet = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");
                var exist = response
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.NameFilter), e => e.ShiftResources.Name == getAllShiftResourcesInput.NameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.ResourcesNameFilter), e => e.ResourcesName == getAllShiftResourcesInput.ResourcesNameFilter)
                            .WhereIf(!string.IsNullOrWhiteSpace(getAllShiftResourcesInput.ShiftsNameFilter), e => e.ShiftsName == getAllShiftResourcesInput.ShiftsNameFilter)
                            .WhereIf(getAllShiftResourcesInput.TimesheetId != 0, e => e.ShiftResources.TimesheetsId == getAllShiftResourcesInput.TimesheetId);

                var sortedList = exist.OrderBy(a => a.ResourcesName);
                var temp = "";
                foreach (var i in sortedList)
                {
                    if (!resourceses.Any(e => e.Resources.Type == getAllShiftResourcesInput.ResourcesTypeFilter && e.Resources.Id == i.ShiftResources.ResourcesId))
                        continue;
                    if (!timeSheet.Any(e => e.Timesheets.Id == i.ShiftResources.TimesheetsId && e.Timesheets.CreatedDate == getAllShiftResourcesInput.MinCreatedDateFilter))
                        continue;
                    if (!i.ResourcesName.Equals(temp))
                    {
                        temp = i.ResourcesName;

                        EmployeesToReviewAmount.Add(new EmployeeReviewListModel
                        {
                            resourceName = i.ResourcesName,
                            ResourcesId = i.ShiftResources.ResourcesId
                        });

                    }
                    var emp = EmployeesToReviewAmount.Where(a => a.resourceName.Equals(temp)).FirstOrDefault();
                    if (i.PayTypesCode.Equals("REG"))
                        emp.RegAmount = emp.RegAmount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("OT"))
                        emp.OTamount = emp.OTamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("DT"))
                        emp.DTamount = emp.DTamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("OT1.8"))
                        emp.OT18amount = emp.OT18amount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("REG+10"))
                        emp.REG10amount = emp.REG10amount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("REG+15"))
                        emp.REG15amount = emp.REG15amount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("OT+10"))
                        emp.OT10amount = emp.OT10amount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("AWL"))
                        emp.AWLamount = emp.AWLamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("AWOL"))
                        emp.AWOLamount = emp.AWOLamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("ILL"))
                        emp.ILLamount = emp.ILLamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("VAC"))
                        emp.VACamount = emp.VACamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                    if (i.PayTypesCode.Equals("HOL"))
                        emp.HOLamount = emp.HOLamount + Convert.ToDecimal(i.ShiftResources.HoursWorked);
                }
            }
            
        }


        //EmployeeConfirmation

        public async Task SaveEmployeeConfirmation()
        {
            GetAllStatusUpdatesInput getAllStatusUpdatesInput = new GetAllStatusUpdatesInput
            {
                MinActualCreateDateTimeFilter = selectedDate,
                MaxActualCreateDateTimeFilter = selectedDate,
                NameFilter= "Confirm Employee Review"
            };
            var a = true;
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _statusUpdatesAppService.GetAll(getAllStatusUpdatesInput), result =>
                {
                    if (result.Items.Count() > 0)
                        a = false;
                    else
                        saveEntry();

                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetStatusUpdatesForViewDto>>("StatusUpdates");
                if (!response.Any(e => e.StatusUpdates.Name == getAllStatusUpdatesInput.NameFilter && e.StatusUpdates.ActualCreateDateTime == getAllStatusUpdatesInput.MinActualCreateDateTimeFilter))
                    saveEntry();
            }
        }

        private async void saveEntry()
        {
           
            CreateOrEditStatusUpdatesDto createorEditStatusUpdatesDto = new CreateOrEditStatusUpdatesDto
            {
                ModifiedOn = DateTime.Now,
                Name = "Confirm Employee Review",
                OriginalstatusId = 3,
                NewStatusesId = 5,
                ModifiedBy = _applicationContext.LoginInfo.User.Id,
                ActualCreateDateTime = selectedDate

            };
            await _statusUpdatesAppService.CreateOrEdit(createorEditStatusUpdatesDto);
        }   
    }
}

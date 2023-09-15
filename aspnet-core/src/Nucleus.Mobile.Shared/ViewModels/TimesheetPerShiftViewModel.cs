using Abp.Application.Services.Dto;
using Abp.Extensions;
using Castle.DynamicProxy.Contributors;
using MonkeyCache.SQLite;
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Commands;
using Nucleus.Core.Threading;
using Nucleus.Models.TimesheetPerJob;
using Nucleus.Models.TimesheetPerShift;
using Nucleus.StatusUpdate;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.Timesheet;
using Nucleus.Timesheet.Dtos;
using Nucleus.ViewModels.Base;
using Nucleus.ViewModels.Helpers;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Nucleus.ViewModels
{
    public class TimesheetPerShiftViewModel : XamarinViewModel
    {
        //Properties
        private string _title;
        public DateTime selectedDateTime;
        public int timesheetId;
        public ObservableRangeCollection<TimesheetPerShiftListViewModel> shiftResourceSubmit { get; set; }
        public ObservableRangeCollection<GetTimesheetsForViewDto> getTimesheetsForViewDtos { get; set; }
        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);
        public GetAllTimesheetsInput getAllTimesheetsInput { get; set; }
        private readonly ITimesheetsAppService _timesheetsAppService;
        private readonly IStatusUpdatesAppService _statusUpdatesAppService;
        private readonly IApplicationContext _applicationContext;
        //Constructor
        public TimesheetPerShiftViewModel(ITimesheetsAppService timesheetsAppService, IStatusUpdatesAppService statusUpdatesAppService, IApplicationContext applicationContext)
        {
            _timesheetsAppService = timesheetsAppService;
            _statusUpdatesAppService = statusUpdatesAppService;
            _applicationContext = applicationContext;
            shiftResourceSubmit = new ObservableRangeCollection<TimesheetPerShiftListViewModel>();
            getTimesheetsForViewDtos = new ObservableRangeCollection<GetTimesheetsForViewDto>();
            getAllTimesheetsInput = new GetAllTimesheetsInput();
            
            populateTimehsheetdetail();
        }

        private async void populateTimehsheetdetail()
        {
           
            if (!Convert.ToString(selectedDateTime).IsNullOrEmpty())
            {
                if(CrossConnectivity.Current.IsConnected)
                {
                    getAllTimesheetsInput.MaxCreatedDateFilter = selectedDateTime;
                    getAllTimesheetsInput.MinCreatedDateFilter = selectedDateTime;
                    await WebRequestExecuter.Execute(async () => await _timesheetsAppService.GetAll(getAllTimesheetsInput), result =>
                    {
                        foreach (var i in result.Items)
                        {
                            getTimesheetsForViewDtos.Add(i);
                            if (i.Timesheets.CreatedDate >= selectedDateTime && i.Timesheets.CreatedDate <= selectedDateTime)
                                timesheetId = i.Timesheets.Id;

                        }
                        return Task.CompletedTask;
                    });
                }
                else
                {
                    var response = Barrel.Current.Get<IReadOnlyList<GetTimesheetsForViewDto>>("TimeSheet");
                    foreach (var i in response)
                    {
                        if (i.Timesheets.CreatedDate >= selectedDateTime && i.Timesheets.CreatedDate <= selectedDateTime)
                            timesheetId = i.Timesheets.Id;

                    }
                }
            }
        }

        private async Task PageAppearingAsync()
        {
            shiftResourceSubmit.Clear();
            populateTimehsheetdetail();
            shiftResourceSubmit.Add(new TimesheetPerShiftListViewModel
            {
                Name = "Timesheet",

            });
            shiftResourceSubmit.Add(new TimesheetPerShiftListViewModel
            {
                Name = "Employees"

            });

            shiftResourceSubmit.Add(new TimesheetPerShiftListViewModel
            {
                Name = "Equipment"
            });


            foreach (var product in shiftResourceSubmit) // MyItems here is ItemsSource of your listview
            {
                product.PropertyChanged += Product_PropertyChanged;
            }

            GetAllStatusUpdatesInput getAllStatusUpdatesInput = new GetAllStatusUpdatesInput
            {
                MaxActualCreateDateTimeFilter = selectedDateTime,
                MinActualCreateDateTimeFilter = selectedDateTime,
                UserNameFilter = _applicationContext.LoginInfo.User.Name
            };

            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _statusUpdatesAppService.GetAll(getAllStatusUpdatesInput), result =>
                {
                    foreach (var i in result.Items)
                    {
                        if (i.StatusesName.Equals("Confirm Employee Review"))
                        {
                            var val = shiftResourceSubmit.Where(a => a.Name.Equals("Employees")).FirstOrDefault();
                            val.Id = i.StatusUpdates.Id;
                            val.IsChecked = true;
                        }
                        if (i.StatusesName.Equals("Confirm Equipment Review"))
                        {
                            var val = shiftResourceSubmit.Where(a => a.Name.Equals("Equipment")).FirstOrDefault();
                            val.Id = i.StatusUpdates.Id;
                            val.IsChecked = true;

                        }
                    }
                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetStatusUpdatesForViewDto>>("StatusUpdates");
                foreach (var i in response)
                {
                    if (i.StatusUpdates.ActualCreateDateTime != selectedDateTime)
                        continue;
                    if (i.StatusesName.Equals("Confirm Employee Review"))
                    {
                        var val = shiftResourceSubmit.Where(a => a.Name.Equals("Employees")).FirstOrDefault();
                        val.Id = i.StatusUpdates.Id;
                        val.IsChecked = true;
                    }
                    if (i.StatusesName.Equals("Confirm Equipment Review"))
                    {
                        var val = shiftResourceSubmit.Where(a => a.Name.Equals("Equipment")).FirstOrDefault();
                        val.Id = i.StatusUpdates.Id;
                        val.IsChecked = true;

                    }
                }
            }

        }


        private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                if (((TimesheetPerShiftListViewModel)sender).Name == "Employees")
                {
                    var val = shiftResourceSubmit.Where(a => a.Name.Equals("Employees")).FirstOrDefault();
                    if (val.IsChecked.Equals(false) && !Convert.ToString(val.Id).IsNullOrEmpty())
                    {
                        removeConfirmationCheck(val);
                    }
                    Settings.isEmployeeChecked = val.IsChecked;

                }
                if (((TimesheetPerShiftListViewModel)sender).Name == "Equipment")
                {
                    var val1 = shiftResourceSubmit.Where(a => a.Name.Equals("Equipment")).FirstOrDefault();
                    if (val1.IsChecked.Equals(false) && !Convert.ToString(val1.Id).IsNullOrEmpty())
                    {
                        removeConfirmationCheck(val1);
                    }
                    Settings.isEquipmentChecked = val1.IsChecked;
                }

            }
        }
        public Command<string> setTitle
        {
            get
            {
                return new Command<string>((title) =>
                {
                    Title = title;
                });
            }
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
        public bool IsEmployeeChecked
        {
            get => Settings.isEmployeeChecked;
            set
            {
                Settings.isEmployeeChecked = value;
                RaisePropertyChanged(() => Settings.isEmployeeChecked);
            }
        }
        public bool IsEquipmentChecked
        {
            get => Settings.isEquipmentChecked;
            set
            {
                Settings.isEquipmentChecked = value;
                RaisePropertyChanged(() => Settings.isEquipmentChecked);
            }

        }
        public Command<string> checkedAndConfirmed
        {

            get
            {
                return new Command<string>((resource) =>
                {
                    if (resource.Contains("Employees"))
                        IsEmployeeChecked = true;
                    if (resource.Contains("Equipment"))
                        IsEquipmentChecked = true;
                });
            }
        }

        public  bool SubmitTimesheet()
        {
            if (timesheetId.Equals(0))
                return false;
            else
            {
                try
                {
                    ChangeStatusUpdate();
                    changetimehseetStatus();
                    return true;
                }
               catch(Exception e)
                {
                    
                    return false;
                }
            }
        }

        public async void ChangeStatusUpdate()
        {
           
            CreateOrEditStatusUpdatesDto createorEditStatusUpdatesDto = new CreateOrEditStatusUpdatesDto
            {
                ModifiedOn = DateTime.Now,
                Name="Submit",
                OriginalstatusId=3,
                TimesheetsId=timesheetId,
                NewStatusesId=4,
                ModifiedBy=_applicationContext.LoginInfo.User.Id,
                ActualCreateDateTime=selectedDateTime

            };
            await WebRequestExecuter.Execute(async () => await _statusUpdatesAppService.CreateOrEdit(createorEditStatusUpdatesDto));
           
        }

        private async void changetimehseetStatus()
        {
            foreach(GetTimesheetsForViewDto gs in getTimesheetsForViewDtos)
            {
                CreateOrEditTimesheetsDto createOrEditTimesheetsDto = new CreateOrEditTimesheetsDto
                {
                    Id=gs.Timesheets.Id,
                    StatusesId=3,
                    SubmitedDate=DateTime.Now,
                    Name=gs.Timesheets.Name,
                    CreatedDate=gs.Timesheets.CreatedDate
                };
                await WebRequestExecuter.Execute(async () => await _timesheetsAppService.CreateOrEdit(createOrEditTimesheetsDto));
            }
           
        }

        internal async void removeConfirmationCheck(TimesheetPerShiftListViewModel chk_button)
        {
            EntityDto createOrEditStatusUpdatesDto = new EntityDto
            {
                Id = chk_button.Id
            };
            await WebRequestExecuter.Execute(async () => await _statusUpdatesAppService.Delete(new EntityDto(createOrEditStatusUpdatesDto.Id)));
        }
    }

}

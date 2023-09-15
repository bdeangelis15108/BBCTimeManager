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
using Nucleus.Shift;
using Nucleus.Shift.Dtos;
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
using System.Threading;  

namespace Nucleus.ViewModels
{
    public class EquipmentReviewViewModel : XamarinViewModel
    {
        private string _title;
        public ObservableRangeCollection<PayTypesDto> PayTypes { get; set; }
        public ObservableRangeCollection<EquipmentReviewListModel> EquipmentToReviewAmount { get; set; }
        public ObservableRangeCollection<string> listofEquipment { get; set; }
        public GetAllShiftResourcesInput getAllShiftResourcesInput { get; set; }
        public GetAllShiftsInput getAllShiftsInput { get; set; }
        public DateTime selectedDate { get; set; }
        public string selectdDT { get; set; }
        public Grid grid { get; set; }
        public Grid mainGrid { get; set; }
        public StackLayout stackLayout { get; set; }
        public ScrollView scrollView {get;set ;}
        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);
        //Service
        private readonly IShiftsAppService _shiftsAppService;
        private readonly IShiftResourcesAppService _shiftResourcesAppService;
        private readonly IApplicationContext _applicationContext;
        private readonly IStatusUpdatesAppService _statusUpdatesAppService;

        public EquipmentReviewViewModel(IShiftsAppService shiftsAppService, IStatusUpdatesAppService statusUpdatesAppService, IShiftResourcesAppService shiftResourcesAppService, IApplicationContext applicationContext)
        {
            _shiftsAppService = shiftsAppService;
            _shiftResourcesAppService = shiftResourcesAppService;
            _applicationContext = applicationContext;
            _statusUpdatesAppService = statusUpdatesAppService;
            _title = L.Localize("Equipment Review");

            getAllShiftResourcesInput = new GetAllShiftResourcesInput();
            getAllShiftsInput = new GetAllShiftsInput();
            EquipmentToReviewAmount = new ObservableRangeCollection<EquipmentReviewListModel>();
            listofEquipment = new ObservableRangeCollection<string>();
            mainGrid = new Grid
            {
                BackgroundColor = Color.White,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }
                    
                },
               
            };

            grid = new Grid
            {
                VerticalOptions=LayoutOptions.Start,
                BackgroundColor = Color.Black,
            };          
            mainGrid.Children.Add(new StackLayout()
            {
               
                BindingContext= scrollView,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            }, 0, 0) ;
            scrollView = new ScrollView {  HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = ScrollOrientation.Horizontal, Content = grid
            };



            //Pageapepare();
            //PopulatePayTypesListOfValues();
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
            get => selectdDT;
            set
            {
                selectdDT = value;
                RaisePropertyChanged(() => SelectedDT);
            }
        }

        private async Task PageAppearingAsync()
        {
            EquipmentToReviewAmount.Clear();
            await SetBusyAsync(FetchData);
           
        }
        private async Task FetchData()
        {
            getAllShiftResourcesInput.NameFilter = _applicationContext.LoginInfo.User.Name;
            getAllShiftResourcesInput.ResourcesTypeFilter = "Equipment";
            getAllShiftResourcesInput.MaxCreatedDateFilter = selectedDate;
            getAllShiftResourcesInput.MinCreatedDateFilter = selectedDate;
            getAllShiftResourcesInput.MaxResultCount = 1000; 
            grid.Children.Add(new Label
            {
                Text = selectedDate.ToString(" dddd\n MMM dd").ToUpper(),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                Margin = -2,
                Padding = 2,
                BackgroundColor = Color.Gray
            }, 0, 0);
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
                {

                var results = result;
                if (results.Items.Count() > 0)
                {
                        var totalResources = results.Items.GroupBy(a => a.ShiftResources.ResourcesId);
                        var totalAssociateJobs = results.Items.GroupBy(a => a.ShiftsName);
                        //Assign Grid Columns and Rows values 
                        defineGridView(totalAssociateJobs.Count(), totalResources.Count());
                        var sortedList = results.Items.OrderBy(a => a.ResourcesName);

                        //Generating table Heading 
                        
                        for (var v=0;v< totalAssociateJobs.Count();v++)
                        {
                            Console.WriteLine("kuh1 " + totalAssociateJobs.ElementAt(v).Key);
                            grid.Children.Add(new Label
                            {
                                Text = totalAssociateJobs.ElementAt(v).Key,
                                HorizontalTextAlignment = TextAlignment.Center,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Color.Black,
                               Padding=2,
                                Margin = -2,
                                BackgroundColor = Color.FromHex("#e2e2e2"),
                            },0,v+1);
                            
                        }
                        //Generating Equipment table Content
                        var i = 1;
                        foreach (var item in totalResources)
                        {
                            var text = sortedList.Where(a => a.ShiftResources.ResourcesId.Equals(item.Key)).FirstOrDefault().ResourcesName;
                            var j = 1;
                            Console.WriteLine("kuh2 " + text);
                            grid.Children.Add(new Label
                            {
                                Text = text.ToString(),
                                TextColor = Color.Black,
                                
                                HorizontalTextAlignment = TextAlignment.Center,
                                FontAttributes = FontAttributes.Bold,
                                Padding = 2,
                                Margin = -2,
                                BackgroundColor = Color.FromHex("#e2e2e2")
                            }, i, 0);
                            foreach (var item1 in  totalAssociateJobs)
                            {
                                decimal hours = 0;
                                if (sortedList.Where(a => a.ShiftResources.ResourcesId.Equals(item.Key) && a.ShiftsName.Equals(item1.Key)).Count()>0)
                                     hours = Convert.ToDecimal(sortedList.Where(a => a.ShiftResources.ResourcesId.Equals(item.Key) && a.ShiftsName.Equals(item1.Key)).FirstOrDefault().ShiftResources.HoursWorked) ;

                                Console.WriteLine("kuh3 " + hours);
                                grid.Children.Add(new Label
                                {
                                    Text = hours.ToString(),
                                    TextColor = Color.Black,
                                    BackgroundColor = Color.White,
                                    Padding = 2,
                                    Margin = -2,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                }, i, j);
                                j++;
                            }
                            i++;   
                        }
                        var resourceType = new ResourcesDto();

                    }
                    else
                    {
                        grid.Children.Add(new Label
                        {
                            Text ="No Data found",
                            TextColor = Color.Black,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontAttributes = FontAttributes.Bold,
                            Margin = -2,
                            Padding = 2,
                            BackgroundColor = Color.White
                        }, 1,0);
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
                
                if (exist.Count() > 0)
                {                    
                        var totalResources = exist.GroupBy(a => a.ShiftResources.ResourcesId);
                        var totalAssociateJobs = exist.GroupBy(a => a.ShiftsName);
                        //Assign Grid Columns and Rows values 
                        defineGridView(totalAssociateJobs.Count(), totalResources.Count());
                        var sortedList = exist.OrderBy(a => a.ResourcesName);

                        //Generating table Heading 

                        for (var v = 0; v < totalAssociateJobs.Count(); v++)
                        {
                            Console.WriteLine("kuh1 " + totalAssociateJobs.ElementAt(v).Key);
                            grid.Children.Add(new Label
                            {
                                Text = totalAssociateJobs.ElementAt(v).Key,
                                HorizontalTextAlignment = TextAlignment.Center,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Color.Black,
                                Padding = 2,
                                Margin = -2,
                                BackgroundColor = Color.Gray,
                            }, 0, v + 1);

                        }
                        //Generating Equipment table Content
                        var i = 1;
                        foreach (var item in totalResources)
                        {
                            var text = sortedList.Where(a => a.ShiftResources.ResourcesId.Equals(item.Key)).FirstOrDefault().ResourcesName;
                            var j = 1;
                            Console.WriteLine("kuh2 " + text);
                            grid.Children.Add(new Label
                            {
                                Text = text.ToString(),
                                TextColor = Color.Black,

                                HorizontalTextAlignment = TextAlignment.Center,
                                FontAttributes = FontAttributes.Bold,
                                Padding = 2,
                                Margin = -2,
                                BackgroundColor = Color.Gray
                            }, i, 0);
                            foreach (var item1 in totalAssociateJobs)
                            {
                                decimal hours = 0;
                                if (sortedList.Where(a => a.ShiftResources.ResourcesId.Equals(item.Key) && a.ShiftsName.Equals(item1.Key)).Count() > 0)
                                    hours = Convert.ToDecimal(sortedList.Where(a => a.ShiftResources.ResourcesId.Equals(item.Key) && a.ShiftsName.Equals(item1.Key)).FirstOrDefault().ShiftResources.HoursWorked);

                                Console.WriteLine("kuh3 " + hours);
                                grid.Children.Add(new Label
                                {
                                    Text = hours.ToString(),
                                    TextColor = Color.Black,
                                    BackgroundColor = Color.White,
                                    Padding = 2,
                                    Margin = -2,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                }, i, j);
                                j++;
                            }
                            i++;
                        }
                        var resourceType = new ResourcesDto();
                } else
                {
                    grid.Children.Add(new Label
                    {
                        Text = "No Data found",
                        TextColor = Color.Black,                        
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold,
                        Margin = -2,
                        Padding = 2,
                        BackgroundColor = Color.White
                    }, 1, 0);
                }

            }
        }
        //Equipment Confirmation
        public async Task SaveEquipmentConfirmation()
        {
            GetAllStatusUpdatesInput getAllStatusUpdatesInput = new GetAllStatusUpdatesInput
            {
                MinActualCreateDateTimeFilter = selectedDate,
                MaxActualCreateDateTimeFilter = selectedDate,
                NameFilter = "Confirm Equipment Review"
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
                Name = "Confirm Equipment Review",
                OriginalstatusId = 3,
                NewStatusesId = 7,
                ModifiedBy = _applicationContext.LoginInfo.User.Id,
                ActualCreateDateTime = selectedDate

            };
            await _statusUpdatesAppService.CreateOrEdit(createorEditStatusUpdatesDto);
        }
        public void defineGridView(int cols,int rows )
        {
            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            for(int j = 0; j < cols; j++)
            {
                if(j == 0)
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) });
                else
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width =new GridLength(1,GridUnitType.Star) });
            }
        }
    }
}

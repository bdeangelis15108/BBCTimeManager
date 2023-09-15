using Abp.Application.Services.Dto;
using Abp.Extensions;
using MonkeyCache.SQLite;
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Commands;
using Nucleus.Core.Threading;
using Nucleus.Localization;
using Nucleus.Models.Employees;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;
using Nucleus.ResourceWorkerInfo;
using Nucleus.ResourceWorkerInfo.Dtos;
using Nucleus.ShiftResource;
using Nucleus.ShiftResource.Dtos;
using Nucleus.ViewModels.Base;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Nucleus.ViewModels
{
    public class EmployeesViewModel : XamarinViewModel
    {
        //Properties
        private string _title;
        private int _currentPage=0;
        public ObservableRangeCollection<EmployeesListModel> Employees { get; set; }
        public ObservableRangeCollection<EmployeesListModel> ReservedEmployees { get; set; }
        public ObservableRangeCollection<EmployeesresevedResorses> ReservedResources { get; set; }
        public CheckBox selectedCheckbox { get; set; }
        private EmployeesListModel _selectedItems { get; set; }
        private CreateorEditReservedEmployee createorEditReservedEmployee { get; set; }
        private GetAllResourceWorkerInfosesInput input { get; set; }
        public List<string> resourceNames { get; set; }
        public int countVal { get; set; }
        public GetAllResourceReservationsesInput getAllResourceReservationsesInput { get; set; }
        public GetAllShiftResourcesInput getAllShiftResourcesInput { get; set; }

        //Services
        private readonly IResourceWorkerInfosesAppService _employeesService;
        private readonly IResourceReservationsesAppService _resourceReservationService;
        private readonly IApplicationContext _applicationContext;
        private readonly IShiftResourcesAppService _shiftResourcesAppService;
        //Method
        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);
        public GetAllResourceReservationsesInput _inputResource;


        //Constructor
        public EmployeesViewModel(IResourceWorkerInfosesAppService employeesService,IResourceReservationsesAppService resourceReservationsesAppService, IApplicationContext applicationContext, IShiftResourcesAppService shiftResourcesAppService)
        {
            // Realtime Data
            _title = L.Localize("Employees");
            _employeesService = employeesService;
            _applicationContext = applicationContext;
            _resourceReservationService = resourceReservationsesAppService;
            _shiftResourcesAppService = shiftResourcesAppService;
            ReservedEmployees = new ObservableRangeCollection<EmployeesListModel>();
            Employees = new ObservableRangeCollection<EmployeesListModel>();
            ReservedResources = new ObservableRangeCollection<EmployeesresevedResorses>();
            createorEditReservedEmployee = new CreateorEditReservedEmployee();
            input = new GetAllResourceWorkerInfosesInput
            {
                SkipCount = 0,
                Sorting = "LastName asc"   
             };
            getAllResourceReservationsesInput = new GetAllResourceReservationsesInput
            {
                
                ResourcesTypeFilter = "Employees"
            };
            resourceNames = new List<string>();
            getAllShiftResourcesInput = new GetAllShiftResourcesInput();
            PopulateReservedResource(
           () =>
           {
               Employees.Clear();
               PopulateReservedEmployees();
           }); 
        }

        public async void PopulateReservedResource(Action callback)
       // public async void PopulateReservedResource()
        {
            //Get reserved Employees data 
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _resourceReservationService.GetMyReservedResource(getAllResourceReservationsesInput), async result =>
                {
                    foreach (var item in result.Items)
                    {
                        ReservedResources.Add(new EmployeesresevedResorses
                        {
                            resourcesDto = item.Resources,
                            ResourcesId = item.ResourcesId,
                            ReservedFrom = item.ReservedFrom,
                            ReservedUntil = item.ReservedUntil,
                            UserId = item.UserId,
                            ReservedBy = item.UserName,
                            Id = item.Id
                        });
                        if (_applicationContext.LoginInfo.User.Name.ToLower().Equals(item.UserName.ToLower()))
                        {
                            input.ResourcesNameFilter = item.Resources.Name;
                            Employees.Clear();
                            await SetBusyAsync(FetchEmployeesAsync);
                        }

                    }

                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetMyResourceReservationsDto>>("ResourceReservationses");
                foreach (var item in response)
                {
                    if(item.Resources.Type == "Employees")
                    {
                        ReservedResources.Add(new EmployeesresevedResorses
                        {
                            resourcesDto = item.Resources,
                            ResourcesId = item.ResourcesId,
                            ReservedFrom = item.ReservedFrom,
                            ReservedUntil = item.ReservedUntil,
                            UserId = item.UserId,
                            ReservedBy = item.UserName,
                            Id = item.Id
                        });
                    }
                }
                return;
            }
                
            
            callback();
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
       
        public  Command<EmployeesListModel> RemoveCommand
        {
            get
            {
                return new Command<EmployeesListModel>(async (reservedEmployees) =>
                {
                    Employees.Where(a => a.RefNumber.Equals(reservedEmployees.RefNumber)).FirstOrDefault().IsChecked = false;
                    // reservedEmployees.IsChecked = false;
                    var itm = ReservedEmployees.Where(b => b.RefNumber.Equals(reservedEmployees.RefNumber)).FirstOrDefault();
                    ReservedEmployees.Remove(itm);
                    var resourceId = reservedEmployees.ResourcesId;
                    var resource=ReservedResources.Where(a=>a.resourcesDto.Id.Equals(resourceId)).FirstOrDefault();
                    if (!Convert.ToString(resource).IsNullOrEmpty())
                    {
                        ReservedResources.Remove(resource);
                        deleteReservedResource((EntityDto)resource);
                        await deleteshiftResource(reservedEmployees.FullName);
                    }
                });
            }

        }

        private async Task deleteshiftResource(string resourceName)
        {
            getAllShiftResourcesInput.ResourcesNameFilter = resourceName;
            getAllShiftResourcesInput.TimesheetsStatusFilter = "Create";
            await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
            {
                var results = result.Items;
                foreach (GetShiftResourcesForViewDto i in results )
                {
                    deleteShiftresourceEntry((EntityDto)i.ShiftResources);
                }
              return   Task.CompletedTask;
            });
             
        }

        private async void deleteShiftresourceEntry(EntityDto i)
        {
            
            await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.Delete(new EntityDto(i.Id)));
        }

        private async void deleteReservedResource(EntityDto model)
        {
            await WebRequestExecuter.Execute(async () => await _resourceReservationService.Delete(new EntityDto(model.Id)));
            await SetBusyAsync(FetchReservedResource);
        }

        public Command<EmployeesListModel> AddCommand
        {
            get {
                    return new Command<EmployeesListModel>((reservedEmployees) =>
                    {
                        if(ReservedEmployees.Where(a=>a.FullName.Equals(reservedEmployees.FullName)).Count() <= 0)
                                ReservedEmployees.Add(reservedEmployees);                        
                    });
            }
        }
        private async  Task PageAppearingAsync()
        {
            Employees.Clear();
        }

        public async Task LoadMoreEmployeeIfNeedsAsync(ResourceWorkerInfosDto shownItem)
        {
            if (IsBusy)
            {
                return;
            }

            if (shownItem != Employees.Last())
            {
                return;
            }

           
            input.SkipCount = 100 * ++_currentPage;
            input.Sorting = "LastName asc";
            input.ResourcesNameFilter = null;
            await SetBusyAsync(FetchEmployeesAsync);
        }

        private async Task FetchEmployeesAsync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _employeesService.GetAll(input), result =>
                {
                    foreach (var emp in result.Items)
                    {
                        bool ischecked = false;
                        bool reservedByOther = false;
                        var reservedBy = "N/A";
                        if (ReservedResources.Where(a => a.resourcesDto.Name.Equals(emp.ResourcesName)).Count() > 0)
                        {
                            ischecked = true;
                            var username= ReservedResources.Where(a => a.resourcesDto.Name.Equals(emp.ResourcesName)).FirstOrDefault().ReservedBy;
                            if (!_applicationContext.LoginInfo.User.Name.ToLower().Equals(username.ToLower()))
                            {
                                reservedBy = "Reserved  By" + "\n" + ReservedResources.Where(a => a.resourcesDto.Name.Equals(emp.ResourcesName)).FirstOrDefault().ReservedBy.ToString();
                                reservedByOther = true;
                                ischecked = false;
                            }
                        }
                        if (Employees.Where(a => a.FullName.Equals(emp.ResourcesName)).Count() <= 0)
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
                for(var i = input.SkipCount; i <= input.SkipCount + input.MaxResultCount; i++)
                {
                    if (input.Filter != null && input.Filter != "" && !response[i].ResourcesName.ToLower().Contains(input.Filter))
                        continue;
                    
                    bool ischecked = false;
                    bool reservedByOther = false;
                    var reservedBy = "N/A";
                    if (ReservedResources.Where(a => a.resourcesDto.Name.Equals(response[i].ResourcesName)).Count() > 0)
                    {
                        ischecked = true;
                        var userID = ReservedResources.Where(a => a.resourcesDto.Name.Equals(response[i].ResourcesName)).FirstOrDefault().ReservedBy;
                        if (!_applicationContext.LoginInfo.User.Name.ToLower().Equals(userID.ToLower()))
                        {
                            reservedBy = "Reserved  By" + "\n" + ReservedResources.Where(a => a.resourcesDto.Name.Equals(response[i].ResourcesName)).FirstOrDefault().ReservedBy.ToString();
                            reservedByOther = true;
                            ischecked = false;
                        }
                    }
                    if (Employees.Where(a => a.FullName.Equals(response[i].ResourcesName)).Count() <= 0)
                    {
                        Employees.Add(new EmployeesListModel
                        {
                            FirstName = response[i].ResourceWorkerInfos.FirstName,
                            LastName = response[i].ResourceWorkerInfos.LastName,
                            RefNumber = response[i].ResourceWorkerInfos.RefNumber,
                            WorkerClasees = response[i].WorkerClaseesName,
                            ResourcesId = response[i].ResourceWorkerInfos.ResourcesId,
                            IsChecked = ischecked,
                            ReservedByName = reservedBy,
                            IsReservedByOther = reservedByOther

                        });
                    }
                }
            }
                
        }
        public async void PopulateReservedEmployees()
        {
          await SetBusyAsync(RefreshEmployeesAsync);
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

        public async void  CreateNewResourceReservedAsync (ObservableRangeCollection<EmployeesListModel> reservedEmployees)
        {
            try
            {
                foreach (EmployeesListModel dto in reservedEmployees)
                {
                    GetAllResourceReservationsesInput input =new GetAllResourceReservationsesInput(){ ResourcesNameFilter = dto.FullName };
                   int i= FilteredResource(dto.FullName);
                    if (i == 0)
                    {
                            createorEditReservedEmployee.ReservedFrom = DateTime.Now;
                            createorEditReservedEmployee.ResourcesId = dto.ResourcesId;
                            createorEditReservedEmployee.UserId = _applicationContext.LoginInfo.User.Id;                            
                            AsyncRunner.Run(GotoresourceReservationAsync(createorEditReservedEmployee));
                    }
                }
                await SetBusyAsync(FetchReservedResource);             
            } catch(Exception e)
            {
                Console.WriteLine( e.Message );
            }
        }

        public async  Task<string> CheckReservationDuplication(ObservableRangeCollection<EmployeesListModel> reservedEmployees)
        {
            var isExist = "";
            await WebRequestExecuter.Execute(async () => await _resourceReservationService.GetMyReservedResource(getAllResourceReservationsesInput), async result =>
            {
                var username = _applicationContext.LoginInfo.User.Name.ToLower();
                foreach (var item in result.Items)
                { 

                    if(reservedEmployees.Where(a=>a.FullName.Equals(item.ResourcesName)).Count() > 0 && item.UserName.ToLower().Equals(username))
                    {
                        isExist = item.ResourcesName + "is Reserved by " + item.UserName;

                    }
                    else
                    {
                        isExist = "";
                    }
                }              
                
            });

            return isExist;
        }

        public int FilteredResource(string input)
        {
            var countVal = ReservedResources.Where(a => a.resourcesDto.Name.Equals(input));
            if (countVal.Count()>0)
                return  1;
            else
                return  0;
            
        }

        private async Task GotoresourceReservationAsync(CreateorEditReservedEmployee resource)
        {
            await WebRequestExecuter.Execute(async () => await _resourceReservationService.CreateOrEdit(resource));
        }
        public string FilterText
        {
            get => input.Filter;
            set
            {
               input.Filter = value;
                AsyncRunner.Run(SearchWithDelayAsync(input.Filter));
            }
        }
        private async Task SearchWithDelayAsync(string filterText)
        {

            if (!string.IsNullOrEmpty(filterText))
            {
                await Task.Delay(PageDefaults.SearchDelayMilliseconds);

                if (filterText != input.Filter)
                {
                    return;
                }
            }
            
            await RefreshEmployeesAsync();
        }
        public async Task RefreshEmployeesAsync()
        {
            Employees.Clear();
            input.SkipCount = 0;
            input.ResourcesNameFilter = null;
            input.MaxResultCount = 100;
;            _currentPage = 0;
            await SetBusyAsync(FetchEmployeesAsync);
        }
    }
}

using Abp.Application.Services.Dto;
using Abp.Extensions;
using MonkeyCache.SQLite;
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Commands;
using Nucleus.Core.Threading;
using Nucleus.Localization;
using Nucleus.Models.Equipment;
using Nucleus.Resource;
using Nucleus.Resource.Dtos;
using Nucleus.ResourceEquipmentInfo.Dtos;
using Nucleus.ResourceReservation;
using Nucleus.ResourceReservation.Dtos;
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
    public class EquipmentsViewModel : XamarinViewModel
    {
        //Properties
        private string _title;
        private int _currentPage=0;
        public ObservableRangeCollection<EquipmentListModel> Equipments { get; set; }
        public ObservableRangeCollection<EquipmentListModel> ReservedEquipment { get; set; }
        public ObservableRangeCollection<EquipmentReservedListModel> ReservedResources { get; set; }
        public List<string> resourceNames { get; set; }
        private CreateOrEditResourceReservationsDto createOrEditResourceReservationsDto { get; set; }
        private GetAllResourcesesInput input { get; set; }
        private EquipmentListModel _selectedItems { get; set; }
        public GetAllResourceReservationsesInput getAllResourceReservationsesInput { get; set; }
        public GetAllShiftResourcesInput getAllShiftResourcesInput { get; set; }

        //Services
        private readonly IResourcesesAppService _equipmentService;
        private readonly IResourceReservationsesAppService _resourceReservationService;
        private readonly IApplicationContext _applicationContext;
        private readonly IShiftResourcesAppService _shiftResourcesAppService;

        //Method
        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);

        //Constructor
        public EquipmentsViewModel(IResourcesesAppService equipmentService, IResourceReservationsesAppService resourceReservationsesAppService, IApplicationContext applicationContext, IShiftResourcesAppService shiftResourcesAppService)
        {
            _title = L.Localize("Equipment");
            _equipmentService = equipmentService;
            _resourceReservationService = resourceReservationsesAppService;
            _applicationContext = applicationContext;
            _shiftResourcesAppService = shiftResourcesAppService;
            resourceNames = new List<string>();
            ReservedEquipment = new ObservableRangeCollection<EquipmentListModel>();
            ReservedResources = new ObservableRangeCollection<EquipmentReservedListModel>();
            Equipments = new ObservableRangeCollection<EquipmentListModel>();
            createOrEditResourceReservationsDto = new CreateOrEditResourceReservationsDto();
            getAllShiftResourcesInput = new GetAllShiftResourcesInput();
            input = new GetAllResourcesesInput
            {
                TypeFilter = "Equipment",
                SkipCount = 0
            };
            getAllResourceReservationsesInput = new GetAllResourceReservationsesInput
            {
               
                ResourcesTypeFilter = "Equipment"
            };

            PopulateReservedResource(
           () =>
           {
               Equipments.Clear();
               PopulateReservedEquipments();
           });
            
        }
        public async void PopulateReservedEquipments()
        {
            await SetBusyAsync(RefreshEquipmentAsync);
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

            await RefreshEquipmentAsync();
        }
        private async Task RefreshEquipmentAsync()
        {
            Equipments.Clear();
            input.SkipCount = 0;            
            _currentPage = 0;
            input.MaxResultCount = 100;
            input.ResourceNumberFilter = null;
            await SetBusyAsync(FetchEquipmentAsync);
        }
        public EquipmentListModel SelectedItems
        {
            get
            {
                return _selectedItems;
            }
            set
            {
                if (_selectedItems != value)
                {
                    _selectedItems = value;
                    HandleSelectedItem();
                }
            }
        }
        public void HandleSelectedItem()
        {
            ReservedEquipment.Add(SelectedItems);

        }


        public Command<EquipmentListModel> RemoveCommand
        {
            get
            {
                return new Command<EquipmentListModel>(async (reservedEquiments) =>
                {
                    Equipments.Where(a => a.ResourceNumber.Equals(reservedEquiments.ResourceNumber)).FirstOrDefault().IsChecked = false;
                    reservedEquiments.IsChecked = false;
                    ReservedEquipment.Remove(reservedEquiments);
                    var resourceId = reservedEquiments.Id;
                    var resource = ReservedResources.Where(a => a.ResourcesDto.Id.Equals(resourceId)).FirstOrDefault();                                       
                    if (!Convert.ToString(resource).IsNullOrEmpty())
                    {
                        ReservedResources.Remove(resource);
                        deleteReservedResource((EntityDto)resource);
                       await  deleteshiftResource(reservedEquiments);
                    }
                });
            }

        }
        private async Task deleteshiftResource(EquipmentListModel resources)
        {
            getAllShiftResourcesInput.ResourcesNameFilter = resources.Name;
            getAllShiftResourcesInput.ResourcesTypeFilter = "Equipment";
            getAllShiftResourcesInput.TimesheetsStatusFilter = "Create";

            await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.GetAll(getAllShiftResourcesInput), result =>
            {
                var results = result.Items.Where(a=>a.ShiftResources.ResourcesId.Equals(resources.Id));
                foreach (GetShiftResourcesForViewDto i in results)
                {
                    deleteShiftresourceEntry((EntityDto)i.ShiftResources);
                }
                return Task.CompletedTask;
            });

        }

        private async void deleteShiftresourceEntry(EntityDto i)
        {

            await WebRequestExecuter.Execute(async () => await _shiftResourcesAppService.Delete(new EntityDto(i.Id)));
        }

        private async void deleteReservedResource(EntityDto model)
        {
            await WebRequestExecuter.Execute(async () => await _resourceReservationService.Delete(new EntityDto(model.Id)));
        }
        public Command<EquipmentListModel> AddCommand
        {
            get
            {
                return new Command<EquipmentListModel>((reservedEquiments) =>
                {
                    if(ReservedEquipment.Where(a=>a.Id.Equals(reservedEquiments.Id)).Count() <=0)
                        ReservedEquipment.Add(reservedEquiments);
                });
            }
        }

        private async Task PageAppearingAsync()
        {
            Equipments.Clear();
           // await SetBusyAsync(RefreshEquipmentAsync);
        }

        private async Task FetchEquipmentAsync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _equipmentService.GetAll(input), result =>
                {
                    var list = result.Items.OrderBy(a => a.Resources.ResourceNumber);
                    foreach (var emp in list)
                    {
                        
                        bool ischecked = false;
                        bool reservedByOther = false;
                        var reservedBy = "N/A";
                        if (ReservedResources.Where(a => a.ResourcesDto.Name.Equals(emp.Resources.Name) && a.ResourcesDto.ResourceNumber.Equals(emp.Resources.ResourceNumber)).Count() > 0)
                        {
                            ischecked = true;
                            var userID = ReservedResources.Where(a => a.ResourcesDto.Name.Equals(emp.Resources.Name)).FirstOrDefault().ReservedByName;
                            if (!_applicationContext.LoginInfo.User.Name.ToLower().Equals(userID.ToLower()))
                            {
                                reservedBy = "Reserved  By" + "\n" + ReservedResources.Where(a => a.ResourcesDto.Name.Equals(emp.Resources.Name)).FirstOrDefault().ReservedByName.ToString();
                                reservedByOther = true;
                                ischecked = false;
                            }
                        }
                        Equipments.Add(new EquipmentListModel
                        {
                            ResourceNumber = emp.Resources.ResourceNumber,
                            Name = emp.Resources.Name,
                            Id = emp.Resources.Id,
                            IsChecked = ischecked,
                            ReservedByName = reservedBy,
                            IsReservedByOther = reservedByOther
                        });
                    }


                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetResourcesForViewDto>>("Resourceses");
                for (var i = input.SkipCount; i <= input.SkipCount + input.MaxResultCount; i++)
                {
                    if (response[i].Resources.Type != "Equipment")
                        continue;
                    bool ischecked = false;
                    bool reservedByOther = false;
                    var reservedBy = "N/A";
                    if (ReservedResources.Where(a => a.ResourcesDto.Name.Equals(response[i].Resources.Name) && a.ResourcesDto.ResourceNumber.Equals(response[i].Resources.ResourceNumber)).Count() > 0)
                    {
                        ischecked = true;
                        var userID = ReservedResources.Where(a => a.ResourcesDto.Name.Equals(response[i].Resources.Name)).FirstOrDefault().ReservedByName;
                        if (!_applicationContext.LoginInfo.User.Name.ToLower().Equals(userID.ToLower()))
                        {
                            reservedBy = "Reserved  By" + "\n" + ReservedResources.Where(a => a.ResourcesDto.Name.Equals(response[i].Resources.Name)).FirstOrDefault().ReservedByName.ToString();
                            reservedByOther = true;
                            ischecked = false;
                        }
                    }
                    Equipments.Add(new EquipmentListModel
                    {
                        ResourceNumber = response[i].Resources.ResourceNumber,
                        Name = response[i].Resources.Name,
                        Id = response[i].Resources.Id,
                        IsChecked = ischecked,
                        ReservedByName = reservedBy,
                        IsReservedByOther = reservedByOther
                    });
                }
            }
                
        }
        public async Task LoadMoreEquipmentIfNeedsAsync(ResourcesDto shownItem)
        {
            if (IsBusy)
            {
                return;
            }

            if (shownItem != Equipments.Last())
            {
                return;
            }


            input.SkipCount = 100 * ++_currentPage;
            input.ResourceNumberFilter = null;
            await FetchEquipmentAsync();
        }

        //Reserved selected Equipments API call to save resources
        public async void CreateNewResourceReservedAsync(ObservableRangeCollection<EquipmentListModel> reservedEmployees)
        {
            try
            {
                foreach (ResourcesDto dto in reservedEmployees)
                {

                    if (CheckResourceExisted(dto))
                    {
                        createOrEditResourceReservationsDto.ReservedFrom = DateTime.Now;
                        createOrEditResourceReservationsDto.ResourcesId = dto.Id;
                        createOrEditResourceReservationsDto.UserId = _applicationContext.LoginInfo.User.Id;
                        AsyncRunner.Run(GotoresourceReservationAsync(createOrEditResourceReservationsDto));
                    }
                }
                await SetBusyAsync(FetchReservedResource);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
        public bool CheckResourceExisted(ResourcesDto name)
        {

            var eqVal=ReservedResources.Where(a => a.ResourcesDto.Name.Equals(name.Name) && a.ResourcesDto.ResourceNumber.Equals(name.ResourceNumber));
            if (eqVal.Count()>0)
                return  false;
            else
                return true;
            
        }
        private async Task GotoresourceReservationAsync(CreateOrEditResourceReservationsDto resource)
        {
            await WebRequestExecuter.Execute(async () => await _resourceReservationService.CreateOrEdit(resource));
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

        public async void PopulateReservedResource(Action callback)
        {
            //Get reserved Employees data 
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _resourceReservationService.GetMyReservedResource(getAllResourceReservationsesInput), async result =>
                {
                    foreach (var item in result.Items)
                    {
                        ReservedResources.Add(new EquipmentReservedListModel
                        {
                            ResourcesDto = item.Resources,
                            ReservedByName = item.UserName,
                            UserId = item.UserId,
                            Id = item.Id,

                    });
                        if (_applicationContext.LoginInfo.User.Name.ToLower().Equals(item.UserName.ToLower()))
                        {
                            input.ResourceNumberFilter = item.Resources.ResourceNumber;
                            Equipments.Clear();
                            await SetBusyAsync(FetchEquipmentAsync);
                        }
                    }                   

                });
            }else
            {
                var response = Barrel.Current.Get<IReadOnlyList<GetMyResourceReservationsDto>>("ResourceReservationses");
                foreach (var item in response)
                {
                    if (item.Resources.Type != "Equipment")
                        continue;
                    ReservedResources.Add(new EquipmentReservedListModel
                    {
                        ResourcesDto = item.Resources,
                        ReservedByName = item.UserName,
                        UserId = item.UserId,
                        Id = item.Id,

                    });

                }
            }
            callback();

        }

     
    }
}

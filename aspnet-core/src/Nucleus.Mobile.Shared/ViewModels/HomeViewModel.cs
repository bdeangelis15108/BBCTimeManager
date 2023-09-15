
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Commands;
using Nucleus.Core.Threading;
using Nucleus.Localization;
using Nucleus.Models.Home;
using Nucleus.PayPeriod;
using Nucleus.PayPeriod.Dtos;
using Nucleus.PayperiodHistory;
using Nucleus.StatusUpdate;
using Nucleus.StatusUpdate.Dtos;
using Nucleus.ViewModels.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nucleus.ViewModels
{
    public class HomeViewModel : XamarinViewModel
    {
        //Properties
        private string _title;
        public ObservableRangeCollection<HomeListModel> PayPeriodList { get; set; }
        public ObservableRangeCollection<HomeListModel> PayPeriodLists { get; set; }
        public DateTime startDate { get; set; }
        public int payPeriodId { get; set; }
        public DateTime endDate { get; set; }
        private GetAllPayPeriodsInput getAllPayPeriodsInput { get; set; }
        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);


        //Services
        private readonly IPayPeriodsAppService _payPeriodsAppService;
        private readonly  IPayperiodHistoriesAppService _payperiodHistoriesAppService;
        private readonly IStatusUpdatesAppService _statusUpdatesAppService;
        private readonly IApplicationContext _applicationContext;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }
        
        public   HomeViewModel(IPayPeriodsAppService payPeriodsAppService, IStatusUpdatesAppService statusUpdatesAppService, IApplicationContext applicationContext, IPayperiodHistoriesAppService payperiodHistoriesAppService)
        {
            _title = L.Localize("Timesheet");
            _payPeriodsAppService = payPeriodsAppService;
            _statusUpdatesAppService = statusUpdatesAppService;
            _applicationContext = applicationContext;
            _payperiodHistoriesAppService = payperiodHistoriesAppService;
            
            PayPeriodList = new ObservableRangeCollection<HomeListModel>();
            PayPeriodLists = new ObservableRangeCollection<HomeListModel>();
            payPeriodId = new int();
            getAllPayPeriodsInput = new GetAllPayPeriodsInput();
        }

        private async Task populatePayrollHistory()
        {
            await WebRequestExecuter.Execute(async () => await _payperiodHistoriesAppService.GetAll(null), async result =>
            {
                payPeriodId =Convert.ToInt32( result.Items.FirstOrDefault().PayperiodHistories.PayPeriodsId);
                await populatePayPeriod();
                
            });
        }

        public async Task populatePayPeriod()
        {
             PayPeriodList.Clear();  
            //Get all Employees data 
            await WebRequestExecuter.Execute(async () => await _payPeriodsAppService.GetAll(getAllPayPeriodsInput), async result =>
            {
                var a = result.Items.FirstOrDefault();
                if (payPeriodId != 0 && payPeriodId.ToString() != null)
                    a = result.Items.Where(j => j.PayPeriods.Id.Equals(payPeriodId)).FirstOrDefault();
                else
                {
                    a = result.Items.Where(j => j.PayPeriods.IsActive is true).FirstOrDefault();
                    
                }
                
                var startofDate = a.PayPeriods.StartDate;
                startDate = startofDate;
                var EndOfDate = a.PayPeriods.StartDate.AddDays(6);
                endDate = a.PayPeriods.EndDate;
                for (DateTime i = startofDate; i <= EndOfDate; i = i.AddDays(1))
                {
                    HomeListModel hlModel = new HomeListModel();
                    hlModel.DateofMonthandYear = i.ToString("MMM")+" "+i.Year.ToString();
                    hlModel.Date = i.Day.ToString();
                    hlModel.Day = i.DayOfWeek.ToString();
                    hlModel.actualDate = i;
                    hlModel.IsSubmitted = true;
                    PayPeriodList.Add(hlModel);

                }
                await FetchPayPeriod();
                //return Task.CompletedTask;
            });
           
        }

        public async Task FetchPayPeriod()
        {
            PayPeriodLists.Clear();
            GetAllStatusUpdatesInput getAllStatusUpdatesInput = new GetAllStatusUpdatesInput
            {
                NameFilter = "Submit",
                UserNameFilter=_applicationContext.LoginInfo.User.Name,
                MaxActualCreateDateTimeFilter=endDate,
                MinActualCreateDateTimeFilter= startDate

            };
            await WebRequestExecuter.Execute(async () => await _statusUpdatesAppService.GetAll(getAllStatusUpdatesInput), async result =>
            {
                int totalPayperiodDays = Convert.ToInt32( (endDate - startDate).TotalDays+1);               
                //Check total submitted timesheet is equal to payperiod days 
                if (result.Items.Count().Equals(totalPayperiodDays))
                {
                    payPeriodId = 0;
                    PayPeriodList.Clear();
                    await populatePayPeriod();
                }
                else
                {
                    foreach (var i in result.Items)
                    {
                        if (PayPeriodList.Where(a => a.actualDate.Equals(i.StatusUpdates.ActualCreateDateTime)).Count() > 0)
                        {
                            var item = PayPeriodList.Where(a => a.actualDate.Equals(i.StatusUpdates.ActualCreateDateTime)).FirstOrDefault();
                            item.IsSubmitted = false;
                        }

                    }
                }
               
                PayPeriodLists.AddRange(PayPeriodList);
                //return Task.CompletedTask;
            });

        }

        public bool isInPayPeriod(DateTime selectedDate)
        {
            if (startDate <= selectedDate && endDate >= selectedDate)
                return false;
            else
                return true;
        }

        private async Task PageAppearingAsync()
        {

            await populatePayrollHistory();
            await SetBusyAsync(FetchPayPeriod);
        }

    }
}

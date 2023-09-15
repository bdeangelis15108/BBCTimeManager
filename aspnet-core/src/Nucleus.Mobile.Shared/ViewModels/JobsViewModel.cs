using MonkeyCache.SQLite;
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Commands;
using Nucleus.Core.Threading;
using Nucleus.EmployeeUnion;
using Nucleus.EmployeeUnion.Dtos;
using Nucleus.Job;
using Nucleus.Job.Dtos;
using Nucleus.JobUnion;
using Nucleus.JobUnion.Dtos;
using Nucleus.Localization;
using Nucleus.ViewModels.Base;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nucleus.ViewModels
{
    public class JobsViewModel : XamarinViewModel
    {
        private string _title;
        private int _currentPage;
        public string unionIdofEmployee;
        public List<string> unionJobcode { get; set; }
        public ObservableRangeCollection<JobsDto> ListofJobs { get; set; }
        private readonly GetAllJobsesInput _input;
        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);

        private readonly IJobsesAppService _jobsesAppService;
        private readonly IApplicationContext _applicationContext;
        private readonly IEmployeeUnionsAppService _employeeUnionsAppService;
        private readonly IJobUnionsAppService _jobUnionsAppService;

        private async Task PageAppearingAsync()
        {
            await SetBusyAsync(FetchJobsAsync);
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
        public JobsViewModel(IJobsesAppService jobsesAppService, IApplicationContext applicationContext, IEmployeeUnionsAppService employeeUnionsAppService, IJobUnionsAppService jobUnionsAppService)
        {
            _jobsesAppService = jobsesAppService;
            _applicationContext = applicationContext;
            _employeeUnionsAppService = employeeUnionsAppService;
            _jobUnionsAppService = jobUnionsAppService;
            _title = L.Localize("Jobs");
            ListofJobs = new ObservableRangeCollection<JobsDto>();
            _input = new GetAllJobsesInput();
            unionJobcode = new List<string>();
        }
        public string FilterText
        {
            get => _input.Filter;
            set
            {
                _input.Filter = value;
                AsyncRunner.Run(SearchWithDelayAsync(_input.Filter));
            }
        }

        private async Task SearchWithDelayAsync(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                await Task.Delay(PageDefaults.SearchDelayMilliseconds);

                if (filter != _input.Filter)
                {
                    return;
                }
            }

            await RefreshJobsAsync();
        }

        public async Task RefreshJobsAsync()
        {
            ListofJobs.Clear();
            _input.SkipCount = 0;
            _currentPage = 0;

            await SetBusyAsync(FetchJobsAsync);
        }

        private async Task FetchJobsAsync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await WebRequestExecuter.Execute(async () => await _jobsesAppService.GetAll(_input), result =>
                {
                    var Jobs = result.Items;
                    foreach (var job in Jobs)
                    {
                        ListofJobs.Add(job.Jobs);

                    }
                    RaisePropertyChanged(() => Title);

                    return Task.CompletedTask;
                });
            }
            else
            {
                var response = string.IsNullOrEmpty(_input.Filter)
                    ?
                      Barrel.Current.Get<IReadOnlyList<GetJobsForViewDto>>("JobDB")
                    : Barrel.Current.Get<IReadOnlyList<GetJobsForViewDto>>("JobDB")
                                    .Where(a => a.Jobs.Code.Contains(_input.Filter) || a.Jobs.Name.Contains(_input.Filter))
                                    .ToList();
                foreach (var job in response)
                {
                    ListofJobs.Add(job.Jobs);

                }
                RaisePropertyChanged(() => Title);

                return;
            }
                
        }

        public async Task LoadMoreJobsIfNeedsAsync(JobsDto shownItem)
        {
            if (IsBusy)
            {
                return;
            }

            if (shownItem != ListofJobs.Last())
            {
                return;
            }
            _input.SkipCount = PageDefaults.PageSize * ++_currentPage;
            await SetBusyAsync(FetchJobsAsync);
        }

        public async Task getallUnionIdsPerEmployees()
        {
            var resourceName= _applicationContext.LoginInfo.User.Name.ToString() + " " + _applicationContext.LoginInfo.User.Surname.ToString();
            GetAllEmployeeUnionsInput input = new GetAllEmployeeUnionsInput
            {
                ResourcesNameFilter = resourceName
            };
            await WebRequestExecuter.Execute(async () => await _employeeUnionsAppService.GetAll(input), result =>
            {
                var results = result;
                if (results.Items.Count() > 0)
                { unionIdofEmployee = results.Items.FirstOrDefault().UnionsNumber;
                    if (unionIdofEmployee.Count() > 0)
                    {
                        return GetJobsperUnionId(unionIdofEmployee);
                    } else
                        return Task.CompletedTask;
                }
                return Task.CompletedTask;
            });
        }

        private async Task GetJobsperUnionId(string unionIdofEmployee)
        {
                GetAllJobUnionsInput input = new GetAllJobUnionsInput
                {
                    UnionsNumberFilter = unionIdofEmployee
                };
                await WebRequestExecuter.Execute(async () => await _jobUnionsAppService.GetAll(input), result =>
                {
                    var jobresults = result;
                    if (jobresults.Items.Count() > 0)
                    {
                        ListofJobs.Clear();
                        foreach (var i in jobresults.Items)
                        {
                            ListofJobs.Add(new JobsDto
                            {
                                Name=i.Jobs.Name,
                                Code=i.Jobs.Code,
                                StartDate=i.Jobs.StartDate,
                                EndDate=i.Jobs.EndDate,
                                Status=i.Jobs.Status,
                                AddressesId=i.Jobs.AddressesId
                            });
                        }
                    }
                    return Task.CompletedTask;
                });
           
        }

        private async Task showMyJobs(List<string> unionJobcode)
        {
            ListofJobs.Clear();
            foreach (var j in unionJobcode)
            {
                _input.CodeFilter = j;
                await SetBusyAsync(FetchJobsAsync);
            }
        }
    }
}

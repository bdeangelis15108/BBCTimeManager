﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Web.Mvc.Alerts;
using Acr.UserDialogs;
using MvvmHelpers;
using Nucleus.ApiClient;
using Nucleus.Authorization.Accounts;
using Nucleus.Authorization.Accounts.Dto;
using Nucleus.Commands;
using Nucleus.Core.Threading;
using Nucleus.Localization;
using Nucleus.Services.Account;
using Nucleus.Services.Storage;
using Nucleus.ViewModels.Base;
using Nucleus.ViewModels.Helpers;
using Nucleus.Views;
using Xamarin.Forms;

namespace Nucleus.ViewModels
{
    public class LoginViewModel : XamarinViewModel
    {
        public ICommand LoginUserCommand => HttpRequestCommand.Create(LoginUserAsync);
        public ICommand ChangeTenantCommand => HttpRequestCommand.Create(ChangeTenantAsync);
        public ICommand ForgotPasswordCommand => HttpRequestCommand.Create(ForgotPasswordAsync);
        public ICommand EmailActivationCommand => HttpRequestCommand.Create(EmailActivationAsync);
        public ICommand PageAppearingCommand => HttpRequestCommand.Create(PageAppearingAsync);

        public string CurrentTenancyNameOrDefault => _applicationContext.CurrentTenant != null
            ? _applicationContext.CurrentTenant.TenancyName
            : L.Localize("NotSelected");

        public bool IsMultiTenancyEnabled => _applicationContext.Configuration.MultiTenancy.IsEnabled;

        private readonly IAccountAppService _accountAppService;
        private readonly IApplicationContext _applicationContext;
        private readonly IDataStorageService _dataStorageService;
        private readonly IAccountService _accountService;

        private string _navigationData;
        private bool _isLoginEnabled;
        private string _tenancyName;
        private ObservableRangeCollection<LanguageInfo> _languages;
        private LanguageInfo _selectedLanguage;
        private bool _isInitialized;
        public bool IsChecked;

        public LoginViewModel(
            IAccountAppService accountAppService,
            IApplicationContext applicationContext,
            IAccountService accountService,
            IDataStorageService dataStorageService)
        {
            _accountAppService = accountAppService;
            _applicationContext = applicationContext;
            _accountService = accountService;
            _dataStorageService = dataStorageService;
            _languages = new ObservableRangeCollection<LanguageInfo>(_applicationContext.Configuration.Localization.Languages);
            _selectedLanguage = _languages.FirstOrDefault(l => l.Name == _applicationContext.CurrentLanguage.Name);
            _isInitialized = false;
            _isLoginEnabled = true;
            if(!Settings.LastUseUserName.Equals(""))
            {
                IsChecked = true;
            }
        }

        public ObservableRangeCollection<LanguageInfo> Languages
        {
            get => _languages;
            set
            {
                _languages = value;
                RaisePropertyChanged(() => Languages);
            }
        }

        public LanguageInfo SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                RaisePropertyChanged(() => SelectedLanguage);
                if (_isInitialized)
                {
                    AsyncRunner.Run(ChangeLanguage());
                }
            }
        }

        private async Task ForgotPasswordAsync()
        {
            await NavigationService.SetMainPage<ForgotPasswordView>();
        }

        private async Task EmailActivationAsync()
        {
            await NavigationService.SetMainPage<EmailActivationView>();
        }

        private async Task ChangeLanguage()
        {
            _applicationContext.CurrentLanguage = _selectedLanguage;

            await UserConfigurationManager.GetAsync(async () =>
            {
                await NavigationService.SetMainPage<LoginView>(clearNavigationHistory: true);
            });
        }

        public override Task InitializeAsync(object navigationData)
        {
            _navigationData = (string)navigationData;
            _isInitialized = true;
            return Task.CompletedTask;
        }

        public string TenancyName
        {
            get => _tenancyName;
            set
            {
                _tenancyName = value;
                RaisePropertyChanged(() => TenancyName);
            }
        }

        public string UserName
        {
            get => _accountService.AbpAuthenticateModel.UserNameOrEmailAddress;
            set
            {
                _accountService.AbpAuthenticateModel.UserNameOrEmailAddress = value;
                SetLoginButtonEnabled();
                RaisePropertyChanged(() => UserName);
                
            }
        }

        public string Password
        {
            get => _accountService.AbpAuthenticateModel.Password;
            set
            {
                _accountService.AbpAuthenticateModel.Password = value;
                SetLoginButtonEnabled();
                RaisePropertyChanged(() => Password);
               
            }
        }

        public void SetLoginButtonEnabled()
        {
            //IsLoginEnabled = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
            IsLoginEnabled = true;
        }

        public bool IsLoginEnabled
        {
            get => _isLoginEnabled;
            set
            {
                _isLoginEnabled = value;
                RaisePropertyChanged(() => IsLoginEnabled);
            }
        }

        private async Task LoginUserAsync()
        {
            rememberCreds();
            await SetBusyAsync(async () =>
            {
                await _accountService.LoginUserAsync();
            });
        }

        private async Task ChangeTenantAsync()
        {
            var promptResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Message = L.Localize("LeaveEmptyToSwitchToHost"),
                Text = _applicationContext.CurrentTenant?.TenancyName ?? "",
                OkText = L.Localize("Ok"),
                CancelText = L.Localize("Cancel"),
                Title = L.Localize("ChangeTenant"),
                Placeholder = L.LocalizeWithThreeDots("TenancyName"),
                MaxLength = AbpTenantBase.MaxTenancyNameLength
            });

            if (!promptResult.Ok)
            {
                return;
            }

            if (string.IsNullOrEmpty(promptResult.Text))
            {
                _applicationContext.SetAsHost();
                ApiUrlConfig.ResetBaseUrl();
                RaisePropertyChanged(() => CurrentTenancyNameOrDefault);
            }
            else
            {
                await SetTenantAsync(promptResult.Text);
            }

            await _dataStorageService.StoreTenantInfoAsync(_applicationContext.CurrentTenant);

        }

        private async Task SetTenantAsync(string tenancyName)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(
                    async () => await _accountAppService.IsTenantAvailable(
                        new IsTenantAvailableInput { TenancyName = tenancyName }),
                    result => IsTenantAvailableExecuted(result, tenancyName)
                );
            });
        }

        private void PopulateLoginInfoFromStorage()
        {
            var loginInfo = _dataStorageService.RetrieveLoginInfo();
            if (loginInfo == null)
            {
                return;
            }

            if (IsChecked.Equals(true))
            {
                UserName = loginInfo.User.UserName;
            }

            if (loginInfo.Tenant != null)
            {
                TenancyName = loginInfo.Tenant.TenancyName;
            }

            if (loginInfo.Tenant == null)
            {
                _applicationContext.SetAsHost();
            }
            else
            {
                _applicationContext.SetAsTenant(TenancyName, loginInfo.Tenant.Id);
            }

            RaisePropertyChanged(() => CurrentTenancyNameOrDefault);
        }

        private async Task PageAppearingAsync()
        {
            PopulateLoginInfoFromStorage();
            await Task.CompletedTask;
        }
        private async Task IsTenantAvailableExecuted(IsTenantAvailableOutput result, string tenancyName)
        {
            var tenantAvailableResult = result;

            switch (tenantAvailableResult.State)
            {
                case TenantAvailabilityState.Available:
                    _applicationContext.SetAsTenant(tenancyName, tenantAvailableResult.TenantId.Value);
                    ApiUrlConfig.ChangeBaseUrl(tenantAvailableResult.ServerRootAddress);
                    RaisePropertyChanged(() => CurrentTenancyNameOrDefault);
                    break;
                case TenantAvailabilityState.InActive:
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(L.Localize("TenantIsNotActive", tenancyName));
                    break;
                case TenantAvailabilityState.NotFound:
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(L.Localize("ThereIsNoTenantDefinedWithName{0}", tenancyName));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //Check RememberMe is checked or not
        public Command<string> RememberMeIsChecked
        {
            get
            {
                return new Command<string>((rememberMe) =>
                {
                    Settings.LastUseUserName = Password;
                    IsChecked = true;
                });
            }
        }
        public Command<string> RememberMeIsUnChecked
        {
            get
            {
                return new Command<string>((rememberMe) =>
                {
                    Settings.LastUseUserName = "";
                    IsChecked = false;

                });
            }
        }
        public Command<string> PopulateUsernamePassword
        {
            get
            {
                return new Command<string>((str) =>
                {


                    Password = Settings.LastUseUserName;                    
                    IsChecked = true;
                });
            }
        }
       //BioMetric Login
       public void rememberCreds()
        {
            if (!Password.IsNullOrEmpty() && !UserName.IsNullOrEmpty())
            {
                Settings.RememberPassword = Password;
                Settings.RememberUserName = UserName;                
            }
           
        }
        public bool PopluateLoginCreds()
        {
            if (Settings.RememberUserName.IsNullOrEmpty() && Settings.RememberPassword.IsNullOrEmpty())
            {
                return true;
            }
            else
            {
                Password = Settings.RememberPassword;
                UserName = Settings.RememberUserName;
                return false;
            }
        }
    }
}

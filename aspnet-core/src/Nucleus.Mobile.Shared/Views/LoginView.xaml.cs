using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using Xamarin.Forms;

namespace Nucleus.Views
{
    public partial class LoginView : ContentPage, IXamarinView
    {
        public LoginView()
        {
            InitializeComponent();
            SetControlFocuses();
            var vm = BindingContext as Nucleus.ViewModels.LoginViewModel;
           // vm.rememberCreds(); //for Biomatric login        
            if (vm.IsChecked == true)
            {
                vm.PopulateUsernamePassword.Execute("Done");
                chk_rememberMe.IsChecked = true;
            }
                  
            
        }

        private void SetControlFocuses()
        {
            UsernameEntry.Completed += (s, e) =>
            {
                if (string.IsNullOrEmpty(PasswordEntry.Text))
                {
                    PasswordEntry.Focus();
                }
                else
                {
                    ExecuteLoginCommand();
                }
            };

            PasswordEntry.Completed += (s, e) =>
            {
                ExecuteLoginCommand();
            };
        }

        private void ExecuteLoginCommand()
        {
            if (LoginButton.IsEnabled)
            {
                LoginButton.Command.Execute(null);
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var vm = BindingContext as Nucleus.ViewModels.LoginViewModel;
            if (checkbox.IsChecked)
            {
                vm.RememberMeIsChecked.Execute("Remember");
            }
            else
            {
                vm.RememberMeIsUnChecked.Execute("Do Not Remember");
            }

        }

        //Biometric Login 
        private async void AuthButton_Clicked(object sender, EventArgs e)
        {
            bool isFingerprintAvailable = await CrossFingerprint.Current.IsAvailableAsync();
            if (!isFingerprintAvailable)
            {
                await DisplayAlert("Error",
                    "Biometric authentication is not available or is not configured.", "OK");
                return;
            }
            AuthenticationRequestConfiguration conf =
        new AuthenticationRequestConfiguration("Authentication",
        "Authenticate access to your personal data");

            var authResult = await CrossFingerprint.Current.AuthenticateAsync(conf);
            //
            if (authResult.Authenticated)
            {
                //Success  
                var vm = BindingContext as Nucleus.ViewModels.LoginViewModel;
                bool result=vm.PopluateLoginCreds();
                if (result)
                    await DisplayAlert("Invalid Login", "Please login first with valid creds in order to login with biomatric in future", "OK");
                else
                    ExecuteLoginCommand();
            }
            else
            {
                await DisplayAlert("Error", "Authentication failed", "OK");
            }
        }

        //ForgetPassword
        public void OnForgetbuttonClick(object sender, EventArgs args)
        {
            DisplayAlert("Warning", "Please contact Administrator in order to reset password", "Ok");
        }
    }
}
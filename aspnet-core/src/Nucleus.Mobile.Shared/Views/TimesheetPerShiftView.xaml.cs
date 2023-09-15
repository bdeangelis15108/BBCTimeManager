using Abp.Extensions;
using Nucleus.Models.TimesheetPerShift;
using Nucleus.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nucleus.Views
{
	public partial class TimesheetPerShiftView : ContentPage, IXamarinView
    {
        public DateTime dt;
        public DateTime selectedDatetime;
        public TimesheetPerShiftView()
		{
			InitializeComponent ();
		}

        public TimesheetPerShiftView(DateTime dt,string checkedConfirmed=null)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, true);
            this.dt = dt;
            var vm = BindingContext as TimesheetPerShiftViewModel;
            vm.selectedDateTime = dt;
            vm.setTitle.Execute(dt.Day.ToString() + " " + dt.ToString("MMM") + " " + dt.Year.ToString());
            selectedDatetime = dt;
        }
       
        protected override bool OnBackButtonPressed()
        {
            //return true to prevent back, return false to just do something before going back. 
            callMainPage();
            return true;
        }
        public async void callMainPage()
        {
            await Navigation.PopToRootAsync();
        }
        public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selecteditem=(TimesheetPerShiftListViewModel)e.SelectedItem;
            if (selecteditem.Name.Equals("Employees"))
                await Navigation.PushAsync(new EmployeeReviewView(selectedDatetime));
            if (selecteditem.Name.Equals("Equipment"))
                await Navigation.PushAsync(new EquipmentReviewView(selectedDatetime));
            if (selecteditem.Name.Equals("Timesheet"))
                await Navigation.PushAsync(new JobsView(true, selectedDatetime));
        }

        private async void lvShiftResoucesSubmit_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selecteditem = (TimesheetPerShiftListViewModel)e.Item;
            if (selecteditem.Name.Equals("Employees"))
                await Navigation.PushAsync(new EmployeeReviewView(selectedDatetime));
            if (selecteditem.Name.Equals("Equipment"))
                await Navigation.PushAsync(new EquipmentReviewView(selectedDatetime));
            if (selecteditem.Name.Equals("Timesheet"))
                await Navigation.PushAsync(new JobsView(true, selectedDatetime));
        }

        private async  void btn_Sumit_Clicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            bool isConfirmedEmp = false;
            bool isConfirmedEqp = false;
            bool isConfirmedtmp = false;
            var vm = BindingContext as TimesheetPerShiftViewModel;
            foreach (var i in vm.shiftResourceSubmit)
            {
                if (i.Name.Equals("Employees") && i.IsChecked.Equals(false))
                {  isConfirmedEmp = true; }
                else if (i.Name.Equals("Equipment") && i.IsChecked.Equals(false))
                {  isConfirmedEqp = true; }
                else if (i.Name.Equals("Timesheet") && i.IsChecked.Equals(false))
                { isConfirmedtmp = true; }
               
            }
            if(isConfirmedtmp.Equals(true) || isConfirmedEmp.Equals(true) || isConfirmedEqp.Equals(true))
            {
               await  DisplayAlert("Submission Failed", "Please confirm all timesheet entries and time review for Employees and Equipment.", "Ok");
            }
            if (isConfirmedEmp.Equals(false) && isConfirmedEqp.Equals(false) && isConfirmedtmp.Equals(false))
            {
                var result = vm.SubmitTimesheet();
                if (result.Equals(true))
                     await Navigation.PopAsync();
                else
                   await DisplayAlert("Submission Failed", "Please confirm all timesheet entries and time review for Employees and Equipment.", "Ok");
            }

        }

    
    }
}
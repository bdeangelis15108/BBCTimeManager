using Nucleus.Models.Home;
using Nucleus.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace Nucleus.Views
{
	public partial class HomeView : ContentPage, IXamarinView
    {
		public HomeView ()
		{
			InitializeComponent ();
			NavigationPage.SetHasBackButton(this, false);
		}
		
		private async void lv_Logdisplay_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var lv = (ListView)sender;
			var item = (HomeListModel)e.Item;
			if ( item.IsSubmitted.Equals(true))
			{
				var ob = lv.BindingContext;
				var vm = BindingContext as HomeViewModel;
				DateTime dt = new DateTime();
				dt = Convert.ToDateTime(((HomeListModel)e.Item).Date + "-" + ((HomeListModel)e.Item).DateofMonthandYear);
				if (vm.isInPayPeriod(dt))
					await DisplayAlert("No Entry Allowed","Your timesheet from last payroll period are incomplete or has not been uploaded. Please complete and submit all timesheet prior entering time entry for this date.", "OK");
				else
					await Navigation.PushAsync(new TimesheetPerShiftView(dt));
            }
            else
            {
                await DisplayAlert("Timesheet Submitted ", "Please contact Administrator in order to access and modify to the timesheet.", "Ok");
            }
            
		}
	}
}
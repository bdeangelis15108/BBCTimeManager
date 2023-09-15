using Nucleus.ViewModels;
using Nucleus.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Nucleus.Views
{
	public partial class EquipmentReviewView : ContentPage, IXamarinView
    {
		public DateTime selectedDT;
		public EquipmentReviewView()
		{
			
			InitializeComponent();
			
		}
		public EquipmentReviewView(DateTime selecteddateTime)
		{

			InitializeComponent();
			var vm = BindingContext as EquipmentReviewViewModel;
			vm.SelectedDate = selecteddateTime;
			vm.SelectedDT = selecteddateTime.ToString(" dddd\n MMM dd").ToUpper();
			selectedDT = selecteddateTime;
			//vm.generateGridView();
			this.Content = vm.scrollView;
			
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			Settings.isEquipmentChecked = true;
			var vm = BindingContext as EquipmentReviewViewModel;
			await vm.SaveEquipmentConfirmation();
			await Navigation.PopAsync();
		}
    }
}
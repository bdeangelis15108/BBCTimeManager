using Nucleus.Job.Dtos;
using Nucleus.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Nucleus.Views
{
	public partial class JobsView : ContentPage, IXamarinView
    {
		public bool IsDateSelected = false;
		public DateTime selectedDatetime;
		public JobsView()
		{
			InitializeComponent();
		
		}
		public JobsView(bool str,DateTime selecteddateTime)
		{
			InitializeComponent();
			IsDateSelected = str;
			this.selectedDatetime = selecteddateTime;
		}

		private async void btn_allJobs_Clicked(object sender, EventArgs e)
		{
			if (!btn_myJobs.IsVisible)
			{
				btn_myJobs.IsVisible = true;
				btn_myJobs.Margin = new Thickness(40, -10, 40, 0);
			}

			else
			{
				btn_myJobs.IsVisible = false;
				var vm = BindingContext as JobsViewModel;
				await vm.RefreshJobsAsync();
			}
		}

		private async void btn_myJobs_Clicked(object sender, EventArgs e)
		{
			if (btn_allJobs.IsVisible)
			{
				btn_allJobs.IsVisible = false;
				btn_myJobs.Margin = new Thickness(40, 10, 40, 0);
				var vm = BindingContext as JobsViewModel;
				await vm.getallUnionIdsPerEmployees();
			}
			else
			{
				btn_allJobs.IsVisible = true;
				btn_myJobs.Margin = new Thickness(40, -10, 40, 0);
			}
			
		}
		public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			await ((JobsViewModel)BindingContext).LoadMoreJobsIfNeedsAsync(e.Item as JobsDto);
		}

		private async void lvJobs_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (IsDateSelected)
			{
				var selectedJob = (JobsDto)e.Item;

				await Navigation.PushAsync(new TimesheetPerJobView(selectedJob,selectedDatetime));
			}
		}
	}
}
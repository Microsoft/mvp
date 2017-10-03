﻿using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Mvp.ViewModels;
using Microsoft.Mvp.Models;
using Microsoft.Mvp.Helpers;

namespace Microsoft.Mvpui
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContributionsPage : ContentPage
    {

        public ContributionsPage()
        {
            InitializeComponent();

            BindingContext = MyProfileViewModel.Instance;

            if (Device.RuntimePlatform == Device.Windows || Device.RuntimePlatform == Device.WinPhone)
                ToolbarAddContribution.Icon = "Assets\\toolbar_add.png";
        }

        public async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ListViewContributions.SelectedItem != null)
            {
                ContributionViewModel.Instance.MyContribution = e.SelectedItem as ContributionModel;
                await Navigation.PushModalAsync(new MVPNavigationPage(
                    new ContributionDetail()
                    {
                        BindingContext = ContributionViewModel.Instance,
                        Title = "Edit an activity"
                    }));

                ListViewContributions.SelectedItem = null;

            }
        }

        public async void OnEdit(object sender, EventArgs eventArgs)
        {
            var mi = ((MenuItem)sender);
            ContributionViewModel.Instance.MyContribution = mi.CommandParameter as ContributionModel;
            await Navigation.PushModalAsync(new MVPNavigationPage(
                new ContributionDetail()
                {
                    BindingContext = ContributionViewModel.Instance,
                    Title = "Edit an activity"
                }));
        }

        public async void OnDelete(object sender, EventArgs eventArgs)
        {
            var mi = ((MenuItem)sender);
            ContributionModel contribution = mi.CommandParameter as ContributionModel;

            string result = await MvpHelper.MvpService.DeleteContributionModel(Convert.ToInt32(contribution.ContributionId, System.Globalization.CultureInfo.InvariantCulture), LogOnViewModel.StoredToken);
            if (result == CommonConstants.OkResult)
            {
                var modelToDelete = MyProfileViewModel.Instance.List.Where(item => item.ContributionId == contribution.ContributionId).FirstOrDefault();
                MyProfileViewModel.Instance.List.Remove(modelToDelete);
            }
        }

        async void AddContribution_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new MVPNavigationPage(new ContributionDetail()));
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListViewContributions.SelectedItem = null;
        }
    }
}
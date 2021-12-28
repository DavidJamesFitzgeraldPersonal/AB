﻿using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PED_Gen_2_Debug_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlPage : ContentPage
    {       
        public ControlPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.ViewModel_ScanPage(Navigation);
            MessagingCenter.Subscribe<ViewModels.ViewModel_ScanPage, string>(this, "Exception", async (sender, arg) =>
            {
                await DisplayAlert("Exception", arg, "OK");
            });
        }
    }
}
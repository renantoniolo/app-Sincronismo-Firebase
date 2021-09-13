using System;
using System.Collections.Generic;
using appSync.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace appSync.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserListView : ContentPage
    {
        public UserListView()
        {
            InitializeComponent();
            this.BindingContext = new UserListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _ = ((UserListViewModel)BindingContext).ThisOnAppearingAsync();
        }
    }
}

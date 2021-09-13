using System;
using System.Collections.Generic;
using appSync.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace appSync.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEditView : ContentPage
    {
        public UserEditView()
        {
            InitializeComponent();
            this.BindingContext = new UserEditViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((UserEditViewModel)BindingContext).ThisOnAppearing();
        }
    }
}

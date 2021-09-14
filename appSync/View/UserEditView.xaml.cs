using System;
using System.Collections.Generic;
using appSync.Model;
using appSync.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace appSync.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEditView : ContentPage
    {
        public UserEditView(User user)
        {
            InitializeComponent();
            this.BindingContext = new UserEditViewModel(user);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((UserEditViewModel)BindingContext).ThisOnAppearing();
        }
    }
}

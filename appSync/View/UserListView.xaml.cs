using System;
using System.Collections.Generic;
using System.Linq;
using appSync.Model;
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

        void CollectionView_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is null)
                return;

            var user = e.CurrentSelection.FirstOrDefault() as User;

            ((UserListViewModel)BindingContext).ClickListCommand.Execute(user);

        }
    }
}

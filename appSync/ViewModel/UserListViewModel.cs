using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using appSync.Interface;
using appSync.Model;
using appSync.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace appSync.ViewModel
{
    public class UserListViewModel : BaseViewModel
    {
        #region Variaveis locais

        private IAlertService alertService;

        #endregion

        #region Ciclo de vida
        public UserListViewModel()
        {
            this.alertService = DependencyService.Get<IAlertService>();
        }

        internal async Task ThisOnAppearingAsync()
        {
            try
            {
                IsLoad = true;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await alertService.ShowAsync("ATENÇÃO", "Não temos conexão com a internet.", "Ok");
                    return;
                }

                ListUser?.Clear();

                var list = await App.firebaseDB.GetUser();

                ListUser = new ObservableCollection<User>(list);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                IsLoad = false;
            }
        }

        #endregion

        #region Propriedades

        private bool isload = true;
        public bool IsLoad
        {
            get { return isload; }

            set
            {
                isload = value;
                this.Notify(nameof(IsLoad));
            }
        }

        private ObservableCollection<User> listUser;
        public ObservableCollection<User> ListUser
        {
            get { return listUser; }
            set
            {
                listUser = value;
                this.Notify(nameof(ListUser));
            }
        }

        #endregion

        #region Command's

        public ICommand AddCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (IsBusy) return;
                        IsBusy = true;

                        await App.NavigationPush(new UserEditView());

                        IsBusy = false;
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        Debug.WriteLine("Error: " + ex.Message);
                    }
                });
            }
        }
        
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (IsBusy) return;
                        IsBusy = true;

                        await ThisOnAppearingAsync();

                        IsBusy = false;
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        Debug.WriteLine("Error: " + ex.Message);
                    }
                });
            }
        }

        #endregion
    }
}

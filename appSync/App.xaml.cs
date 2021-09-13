using System;
using System.Diagnostics;
using System.Threading.Tasks;
using appSync.Interface;
using appSync.Util;
using appSync.View;
using Xamarin.Forms;

namespace appSync
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //registra interface
            DependencyService.Register<IAlertService, AlertService>();
            // inicia o aplicativo
            MainPage = new NavigationPage(new UserListView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #region Firebase

        private static FireBaseHelper instance;
        private static object syncRoot = new object();
        public static FireBaseHelper firebaseDB
        {
            get
            {
                if(instance == null)
                {
                    lock (syncRoot)
                    {
                        instance = new FireBaseHelper();
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Navigation

        public async static Task NavigationPush(Page page)
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao carregar a Navigation Page. " + ex.Message);
            }
        }

        public async static Task NavigationPop()
        {
            try
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao carregar a Navigation Page. " + ex.Message);
            }
        }

        #endregion

    }
}

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using appSync.Interface;
using appSync.Model;
using Xamarin.Forms;

namespace appSync.ViewModel
{
    public class UserEditViewModel : BaseViewModel
    {
        #region Variaveis locais

        private IAlertService alertService;

        #endregion

        public UserEditViewModel()
        {
            this.alertService = DependencyService.Get<IAlertService>();
        }

        internal async Task ThisOnAppearing()
        {

        }

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

        private string inputName;
        public string InputName
        {
            get { return inputName; }
            set
            {
                inputName = value;
                this.Notify(nameof(InputName));
            }
        }

        private int inputAge;
        public int InputAge
        {
            get { return inputAge; }
            set
            {
                inputAge = value;
                this.Notify(nameof(InputAge));
            }
        }

        private string inputEmail;
        public string InputEmail
        {
            get { return inputEmail; }
            set
            {
                inputEmail = value;
                this.Notify(nameof(InputEmail));
            }
        }

        private string inputPassword;
        public string InputPassword
        {
            get { return inputPassword; }
            set
            {
                inputPassword = value;
                this.Notify(nameof(InputPassword));
            }
        }

        private string inputObservation;
        public string InputObservation
        {
            get { return inputObservation; }
            set
            {
                inputObservation = value;
                this.Notify(nameof(InputObservation));
            }
        }

        #endregion

        #region Command's

        public ICommand SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (IsBusy) return;
                        IsBusy = true;

                        if (String.IsNullOrEmpty(inputName) &&
                        String.IsNullOrEmpty(inputEmail) &&
                        String.IsNullOrEmpty(inputPassword))
                        {
                            IsBusy = false;
                            await alertService.ShowAsync("ATENÇÃO", "Preencha todos os campos!", "Ok");
                            return;
                        }

                        IsLoad = true;

                        var user = new User();

                        user.Name = inputName;
                        user.Age = Convert.ToInt32(inputAge);
                        user.Email = inputEmail;
                        //var base64 = Convert.FromBase64String(inputPassword);
                        user.Password = inputPassword;//System.Text.Encoding.UTF8.GetString(base64);

                        // Grava
                        await App.firebaseDB.AdddUser(user);

                        // Remove a page da pilha
                        await App.NavigationPop();

                        IsBusy = false;
                        IsLoad = true;
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        await alertService.ShowAsync("ATENÇÃO", "Falha ao gravar esse usuário.", "Ok");
                        Debug.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        IsLoad = true;
                    }
                });
            }
        }

        #endregion
    }
}

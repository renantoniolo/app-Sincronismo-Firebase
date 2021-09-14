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
        private User user;

        #endregion

        public UserEditViewModel(User user)
        {
            this.user = user;
            this.alertService = DependencyService.Get<IAlertService>();
        }

        internal async Task ThisOnAppearing()
        {
            if(user != null)
            {
                IsEdit = true;
                InputName = user?.Name;
                InputEmail = user?.Email;
                InputAge = Convert.ToInt32(user.Age);
                InputPassword = user?.Password;
                InputObservation = user?.Observation;
            }
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

        private bool isEdit = false;
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                this.Notify(nameof(IsEdit));
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
                        user.Password = inputPassword;

                        // Grava
                        if (this.user is null)
                        {
                            await App.firebaseDB.AdddUser(user);
                        }
                        else // Atualiza
                        {
                            await App.firebaseDB.UpdateUser(user);
                        }

                        // Remove a page da pilha
                        await App.NavigationPop();

                        IsBusy = false;
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        await alertService.ShowAsync("ATENÇÃO", "Falha ao gravar esse usuário.", "Ok");
                        Debug.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        IsLoad = false;
                    }
                });
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (!IsEdit)
                            return;

                        if (IsBusy) return;
                        IsBusy = true;

                        if (String.IsNullOrEmpty(inputEmail))
                        {
                            IsBusy = false;
                            await alertService.ShowAsync("ATENÇÃO", "Impossível excluir sem email!", "Ok");
                            return;
                        }

                        IsLoad = true;

                        if (this.user is null)
                        {
                            IsBusy = false;
                            return;
                        }

                        // delete user selecionado
                        await App.firebaseDB.DeleteUser(this.user);

                        // Remove a page da pilha
                        await App.NavigationPop();

                        IsBusy = false;
                    }
                    catch (Exception ex)
                    {
                        IsBusy = false;
                        await alertService.ShowAsync("ATENÇÃO", "Falha ao gravar esse usuário.", "Ok");
                        Debug.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        IsLoad = false;
                    }
                });
            }
        }

        #endregion
    }
}

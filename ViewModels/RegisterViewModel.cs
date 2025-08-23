using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using SpringOnion.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpringOnion.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authService;

        private string _userId;
        public string UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel(AuthenticationService authService)
        {
            _authService = authService;
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        private async Task RegisterAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var (success, message) = await _authService.RegisterAsync(UserId, Password);

                if (success)
                {
                    if (success)
                    {
                        var toast = Toast.Make(message, ToastDuration.Short, 12);
                        toast.Show();
                        //await App.Current.MainPage.DisplayAlert("Success", message, "OK");
                        Application.Current.MainPage = new AppShellLayer();
                    }
                }
                else
                {
                    var toast = Toast.Make(message, ToastDuration.Short, 12);
                    toast.Show();
                    //await App.Current.MainPage.DisplayAlert("Error", message, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

using System.Threading.Tasks;
using System.Windows.Input;
using SpringOnion.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace SpringOnion.ViewModels
{
    public class LoginViewModel : BaseViewModel
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

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public LoginViewModel(AuthenticationService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
            GoToRegisterCommand = new Command(async () => await GoToRegisterAsync());
        }



        private async Task LoginAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var (success, message) = await _authService.LoginAsync(UserId, Password);

                if (success)
                {
                    var toast = Toast.Make("Logged In", ToastDuration.Short, 12);
                    toast.Show();
                    //await App.Current.MainPage.DisplayAlert("Success", message, "OK");
                    Application.Current.MainPage = new AppShellLayer();
                }
                else
                {
                    var toast = Toast.Make("Error", ToastDuration.Short, 12);
                    toast.Show();
                    //await App.Current.MainPage.DisplayAlert("Error", message, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }
    }
}

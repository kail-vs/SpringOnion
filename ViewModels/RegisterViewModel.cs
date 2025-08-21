using System.Threading.Tasks;
using System.Windows.Input;
using SpringOnion.Services;

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
                    await App.Current.MainPage.DisplayAlert("Success", message, "OK");
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", message, "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

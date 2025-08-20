using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpringOnion.Services;

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

        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthenticationService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            var (success, message) = await _authService.LoginAsync(UserId, Password);

            if (success)
            {
                await App.Current.MainPage.DisplayAlert("Success", message, "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", message, "OK");
            }
        }
    }
}
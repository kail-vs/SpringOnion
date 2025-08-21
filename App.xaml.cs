using SpringOnion.Services;
using SpringOnion.Views;

namespace SpringOnion;

public partial class App : Application
{
    private readonly AuthenticationService _authService;

    public App(AuthenticationService authService)
    {
        InitializeComponent();

        _authService = authService;
        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        base.OnStart();

        var hasToken = await _authService.LoadTokenAsync();

        if (hasToken)
        {
            var (success, _) = await _authService.GetProfileAsync();
            if (success)
            {
                await Shell.Current.GoToAsync("MainPage");
                return;
            }
        }

        await Shell.Current.GoToAsync("LoginPage");
    }
}

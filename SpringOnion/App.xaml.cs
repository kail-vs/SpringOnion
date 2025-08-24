using SpringOnion.Services;

namespace SpringOnion;

public partial class App : Application
{
    public App(AuthenticationService authService)
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}

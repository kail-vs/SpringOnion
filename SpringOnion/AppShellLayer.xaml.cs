using SpringOnion.Views;

namespace SpringOnion;

public partial class AppShellLayer : Shell
{
	public AppShellLayer()
	{
		InitializeComponent();

        Routing.RegisterRoute("Dashboard", typeof(Dashboard));
    }
}
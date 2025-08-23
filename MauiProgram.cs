using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SpringOnion.ViewModels;
using SpringOnion.Services;
using SpringOnion.Views;
using Newtonsoft.Json;
using CommunityToolkit.Maui;

namespace SpringOnion
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").GetAwaiter().GetResult();
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var configData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            builder.Configuration.AddInMemoryCollection(configData!);

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<Dashboard>();
            builder.UseMauiApp<App>().UseMauiCommunityToolkit();


            return builder.Build();
        }
    }
}

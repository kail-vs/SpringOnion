using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace SpringOnion.Views
{
    public partial class Dashboard : ContentPage
    {
        private DateTime _lastBackPress;

        public Dashboard()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
#if ANDROID
            var now = DateTime.Now;

            if ((now - _lastBackPress).TotalSeconds <= 2)
            {
                // Minimize app (send to background)
                var activity = Platform.CurrentActivity;
                activity?.MoveTaskToBack(true);
            }
            else
            {
                _lastBackPress = now;
                var toast = Toast.Make("Press back again to exit", ToastDuration.Short, 12);
                toast.Show();
            }

            return true; // block default Android back nav
#else
            return base.OnBackButtonPressed();
#endif
        }
    }
}

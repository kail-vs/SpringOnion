using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpringOnion.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private string _welcomeMessage = "Hello, user!";
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        public ICommand RefreshCommand { get; }

        public DashboardViewModel()
        {
            RefreshCommand = new Command(OnRefresh);
        }

        private void OnRefresh()
        {
            WelcomeMessage = "Refreshed at " + DateTime.Now.ToString("T");
        }
    }
}

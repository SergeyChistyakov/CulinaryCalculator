using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CulinaryCalculator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            var mainViewModel = new MainPageViewModel(MainPage.Navigation);
            MainPage.BindingContext = mainViewModel;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

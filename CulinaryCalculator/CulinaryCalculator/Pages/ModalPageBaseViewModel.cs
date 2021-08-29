using CulinaryCalculator.Infrastructure;
using System.Windows.Input;
using Xamarin.Forms;

namespace CulinaryCalculator.Pages
{
    public class ModalPageBaseViewModel : BaseViewModel
    {
        public INavigation Navigation { get; }

        public ICommand Cancel { get; }

        public ICommand Save { get; }

        public ModalPageBaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Cancel = new PropertyDependentCommand(this, _ => true, _ => Navigation.PopModalAsync());
            Save = new PropertyDependentCommand(this, _ => true, _ => { DoSave();});
        }

        protected virtual void DoSave()
        {
            Navigation.PopModalAsync();
        }
    }
}

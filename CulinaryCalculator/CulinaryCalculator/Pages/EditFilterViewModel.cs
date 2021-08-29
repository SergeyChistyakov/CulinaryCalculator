using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CulinaryCalculator.Pages
{
    public class EditFilterViewModel : ModalPageBaseViewModel
    {
        private string m_IngridientToAdd;
        private string m_SelectedIngridient;
        public event Action<IEnumerable<Category>, IEnumerable<string>, string> FilterChanged;
        public ICommand AddIngridient { get; }
        public ICommand DeleteIngridient { get; }

        public EditFilterViewModel(INavigation navigation) : base(navigation)
        {
            AddIngridient = new PropertyDependentCommand(this, _ => !string.IsNullOrEmpty(IngridientToAdd), _ => { DoAddIngridient(); });
            DeleteIngridient = new PropertyDependentCommand(this, _ => !string.IsNullOrEmpty(SelectedIngridient), _ => { DoDeleteIngridient(); });
            Categories = new ObservableCollection<object>(RecipesRepository.GetCategories());
        }

        public ObservableCollection<object> Categories { get; }
        public ObservableCollection<object> SelectedCategories { get; set; } = new ObservableCollection<object>();

        public string IngridientToAdd
        {
            get => m_IngridientToAdd;
            set => Set(ref m_IngridientToAdd, value);
        }

        public string SelectedIngridient
        {
            get => m_SelectedIngridient;
            set => Set(ref m_SelectedIngridient, value);
        }
        public ObservableCollection<string> Ingridients { get; set; } = new ObservableCollection<string>();
        public string Substring { get; set; }
        protected override void DoSave()
        {
            FilterChanged?.Invoke(SelectedCategories.Cast<Category>(), Ingridients, Substring);
            base.DoSave();
        }
        private void DoAddIngridient()
        {
            Ingridients.Add(IngridientToAdd);
            IngridientToAdd = string.Empty;
        }

        private void DoDeleteIngridient()
        {
            Ingridients.Remove(SelectedIngridient);
        }
    }
}

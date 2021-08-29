using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CulinaryCalculator.Views
{
    public class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();

        public string Label { get; set; }

        public ICommand SelectCategory { get; }

        public event EventHandler<Category> CategorySelected;

        public CategoriesViewModel()
        {
            Categories = new ObservableCollection<Category>(RecipesRepository.GetCategories());
            Categories.CollectionChanged += Categories_CollectionChanged;
            RecipesRepository.CategoryAdded += RecipesRepository_CategoryAdded;
            Label = GetLabel();
            SelectCategory = new PropertyDependentCommand(this, _ => true, (object id) => CategorySelected?.Invoke(this, Categories.First(c => c.Id == (int)id)));
        }

        private void RecipesRepository_CategoryAdded(object sender, Category category)
        {
            Categories.Add(category);
        }

        private void Categories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Label = GetLabel();
            NotifyOfPropertyChange(Label);
        }      

        private string GetLabel() => $"Категории ({Categories.Count})";
    }
}

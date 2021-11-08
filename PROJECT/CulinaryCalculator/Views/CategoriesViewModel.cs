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

        public ICommand EditCategory { get; }

        public ICommand DeleteCategory { get; }

        public event EventHandler<Category> CategorySelected;

        public event EventHandler<Category> CategoryEdited;

        public CategoriesViewModel()
        {
            Categories = new ObservableCollection<Category>(RecipesRepository.GetCategories());
            Categories.CollectionChanged += Categories_CollectionChanged;
            RecipesRepository.CategoryAdded += RecipesRepository_CategoryAdded;
            Label = GetLabel();
            SelectCategory = new PropertyDependentCommand(this, _ => true, (object id) => CategorySelected?.Invoke(this, Categories.First(c => c.Id == (int)id)));
            EditCategory = new PropertyDependentCommand(this, _ => true, (object id) => CategoryEdited?.Invoke(this, Categories.First(c => c.Id == (int)id)));
            DeleteCategory = new PropertyDependentCommand(this, _ => true, (object id) => DoDeleteCategory((int)id));
        }

        public void Refresh()
        {
            Categories.Clear();
            RecipesRepository.GetCategories().ForEach(item => Categories.Add(item));
            Label = GetLabel();
            NotifyOfPropertyChange(nameof(Label));
        }

        private void RecipesRepository_CategoryAdded(object sender, Category category)
        {
            Categories.Add(category);
        }

        private async void DoDeleteCategory(int id)
        {
            var answer = await App.Current.MainPage.DisplayAlert("Кулинарный калькулятор", "Удалить категорию?", "ОК", "Отмена");
            if (!answer)
            {
                return;
            }

            var category = Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return;
            RecipesRepository.RemoveCategory(category);
            Categories.Remove(category);
            Refresh();
        }

        private void Categories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Label = GetLabel();
            NotifyOfPropertyChange(Label);
        }      

        private string GetLabel() => $"Категории ({Categories.Count})";
    }
}

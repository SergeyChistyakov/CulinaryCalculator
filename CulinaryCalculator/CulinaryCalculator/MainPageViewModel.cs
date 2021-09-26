using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using CulinaryCalculator.Pages;
using CulinaryCalculator.Views;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CulinaryCalculator
{
    public class MainPageViewModel : BaseViewModel
    {
        private RecipesViewModel m_RecipesViewModel;
        private Recipes m_RecipesPage;

        public MainPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Categories.CategorySelected += Categories_CategorySelected;
            Categories.CategoryEdited += Categories_CategoryEdited;
            TemplateViewModel = new MainPageTemplateViewModel(navigation);
            TemplateViewModel.OnCreateCategory = (category) =>
            {
                Categories.Refresh();
                m_RecipesViewModel.Update(new Category[] { category }, null, null);
                Navigation.PushAsync(m_RecipesPage);
            };
            m_RecipesViewModel = new RecipesViewModel(TemplateViewModel, navigation);
            m_RecipesPage = new Recipes();
            m_RecipesPage.BindingContext = m_RecipesViewModel;
            TemplateViewModel.FilterViewModel.FilterChanged += FilterViewModel_FilterChanged; ;
        }

        private async void Categories_CategoryEdited(object sender, Category category)
        {
            var view = new EditCategory();
            var viewModel = new EditCategoryViewModel(view.Navigation, category);
            viewModel.CategorySaved += ViewModel_CategorySaved;
            view.BindingContext = viewModel;
            await Navigation.PushModalAsync(view);
        }

        private void ViewModel_CategorySaved(object sender, Category category)
        {
            Categories.Refresh();
        }

        private void FilterViewModel_FilterChanged(IEnumerable<Category> categories, IEnumerable<string> ingredients, string substring)
        {
            m_RecipesViewModel.Update(categories, ingredients, substring);
            if (!Navigation.NavigationStack.Contains(m_RecipesPage)) Navigation.PushAsync(m_RecipesPage);
        }

        public MainPageTemplateViewModel TemplateViewModel { get; }

        public INavigation Navigation { get; }

        public CategoriesViewModel Categories { get; } = new CategoriesViewModel();

        private void Categories_CategorySelected(object sender, Category e)
        {
            m_RecipesViewModel.Update(new Category[] { e }, null, null);
            Navigation.PushAsync(m_RecipesPage);
        }
    }
}

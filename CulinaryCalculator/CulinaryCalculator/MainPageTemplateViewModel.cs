using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using CulinaryCalculator.Pages;
using System;
using Xamarin.Forms;

namespace CulinaryCalculator
{
    public class MainPageTemplateViewModel : BaseViewModel
    {
        public EditFilterViewModel FilterViewModel { get; }

        public PropertyDependentCommand CreateCategory { get; }

        public PropertyDependentCommand CreateRecipe { get; }

        public PropertyDependentCommand EditSettings { get; }

        public PropertyDependentCommand EditFilter { get; }

        public INavigation Navigation { get; }

        public Action OnCreateRecipe { get; set; }

        public Action<Category> OnCreateCategory { get; set; }

        public MainPageTemplateViewModel(INavigation navigation)
        {
            Navigation = navigation;
            CreateCategory = new PropertyDependentCommand(this, _ => true, ShowCreateCategory);
            CreateRecipe = new PropertyDependentCommand(this, _ => true, ShowCreateRecipe);
            EditSettings = new PropertyDependentCommand(this, _ => true, ShowEditSettings);
            EditFilter = new PropertyDependentCommand(this, _ => true, ShowEditFilter);
            FilterViewModel = new EditFilterViewModel(Navigation);
        }

        private async void ShowCreateCategory(object sender)
        {
            var view = new EditCategory();
            var viewModel = new EditCategoryViewModel(view.Navigation);
            viewModel.CategorySaved += (_, category) => OnCreateCategory(category);
            view.BindingContext = viewModel;
            await Navigation.PushModalAsync(view);
        }
        private async void ShowCreateRecipe(object sender)
        {
            var view = new EditRecipe();
            var viewModel = new EditRecipeViewModel(view.Navigation, OnCreateRecipe);
            view.BindingContext = viewModel;
            await Navigation.PushModalAsync(view);
        }
        private async void ShowEditSettings(object sender)
        {
            var view = new EditSettings();
            var viewModel = new EditSettingsViewModel(view.Navigation);
            view.BindingContext = viewModel;
            await Navigation.PushModalAsync(view);
        }
        private async void ShowEditFilter(object sender)
        {
            var view = new EditFilter();
            view.BindingContext = FilterViewModel;
            await Navigation.PushModalAsync(view);    
        }
    }
}

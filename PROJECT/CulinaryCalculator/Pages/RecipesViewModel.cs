﻿using CulinaryCalculator.Infrastructure;
using Model = CulinaryCalculator.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using CulinaryCalculator.Model;

namespace CulinaryCalculator.Pages
{
    public class RecipesViewModel : BaseViewModel
    {
        public ICommand SelectRecipe { get; }

        public ICommand EditRecipe { get; }

        public ICommand DeleteRecipe { get; }

        private RecipeViewModel SelectedRecipe { get; }

        private IEnumerable<Model.Category> m_categories;
        private IEnumerable<string> m_ingredients;
        private string m_substring;

        public RecipesViewModel(MainPageTemplateViewModel templateViewModel, INavigation navigation)
        {
            TemplateViewModel = templateViewModel;
            TemplateViewModel.OnCreateRecipe = Update;
            Navigation = navigation;
            Recipes = new ObservableCollection<Model.Recipe>();
            SelectedRecipe = new RecipeViewModel();
            SelectRecipe = new PropertyDependentCommand(this, (_) => true, DoSelectRecipe);
            EditRecipe = new PropertyDependentCommand(this, _ => true, (object id) => DoEditRecipe((int)id));
            DeleteRecipe = new PropertyDependentCommand(this, _ => true, (object id) => DoDeleteRecipe((int)id));
        }

        public MainPageTemplateViewModel TemplateViewModel { get; }

        public INavigation Navigation { get; }

        public ObservableCollection<Model.Recipe> Recipes { get; }

        public void Update()
        {
            Recipes.Clear();
            var recipes = Model.RecipesRepository.GetRecipes()
                .Where(r =>
                (string.IsNullOrEmpty(m_substring) || r.Title.Contains(m_substring))
                && (m_categories == null || !m_categories.Any() || m_categories.Contains(r.Category, new Model.CategoryIdEqualityComparer()))
                && (m_ingredients == null || !m_ingredients.Any() || m_ingredients.All(item => r.IngredientItems.Select(iI => iI.Title.ToLower()).Contains(item.ToLower()))));
            foreach (var recipe in recipes)
            {
                Recipes.Add(recipe);
            }
        }

        public void Update(IEnumerable<Model.Category> categories, IEnumerable<string> ingredients, string substring)
        {
            m_categories = categories;
            m_ingredients = ingredients;
            m_substring = substring;
            Update();
        }

        private async void DoSelectRecipe(object param)
        {
            int recipeId = (int)param;
            SelectedRecipe.Load(recipeId);
            var view = new Recipe();
            view.BindingContext = SelectedRecipe;
            await Navigation.PushAsync(view);
        }

        private async void DoDeleteRecipe(int id)
        {
            var answer = await App.Current.MainPage.DisplayAlert("Кулинарный калькулятор", "Удалить рецепт?", "ОК", "Отмена");
            if (!answer)
            {
                return;
            }

            var recipe = RecipesRepository.GetRecipe(id);
            if (recipe == null) return;
            RecipesRepository.RemoveRecipe(recipe);
            Recipes.Remove(recipe);
            await Navigation.PopAsync();
        }

        private async void DoEditRecipe(int id)
        {
            var view = new EditRecipe();
            var viewModel = new EditRecipeViewModel(view.Navigation, Update, id);
            view.BindingContext = viewModel;
            await Navigation.PushModalAsync(view);
        }
    }
}

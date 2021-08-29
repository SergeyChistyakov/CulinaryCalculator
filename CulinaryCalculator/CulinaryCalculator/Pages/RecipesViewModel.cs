using CulinaryCalculator.Infrastructure;
using CulinaryCalculator.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace CulinaryCalculator.Pages
{
    public class RecipesViewModel : BaseViewModel
    {
        ICommand SelectRecipe { get; }

        public RecipesViewModel(MainPageTemplateViewModel templateViewModel, INavigation navigation)
        {
            TemplateViewModel = templateViewModel;
            Navigation = navigation;
            Recipes = new ObservableCollection<Recipe>();
            SelectRecipe = new PropertyDependentCommand(this, (_) => true, DoSelectRecipe);
        }

        public MainPageTemplateViewModel TemplateViewModel { get; }

        public INavigation Navigation { get; }

        public ObservableCollection<Recipe> Recipes { get; }

        public void Update(IEnumerable<Category> categories, IEnumerable<string> ingridients, string substring)
        {
            Recipes.Clear();
            var recipes = RecipesRepository.GetRecipes()
                .Where(r => 
                (string.IsNullOrEmpty(substring) || r.Title.Contains(substring))
                && (categories == null || !categories.Any() || categories.Contains(r.Category, new CategoryIdEqualityComparer()))
                && (ingridients == null || !ingridients.Any() || ingridients.All(item => r.IngredientItems.Select(iI=>iI.Title).Contains(item))));
            foreach (var recipe in recipes)
            {
                Recipes.Add(recipe);
            }
        }

        private void DoSelectRecipe(object param)
        {
            int recipeId = (int)param;
        }
    }
}
